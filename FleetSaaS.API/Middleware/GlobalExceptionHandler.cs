using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Common.Response;
using FleetSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Net;
using System.Security.Authentication;

namespace FleetSaaS.API.Middleware
{
    public class GlobalExceptionHandler() : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        /// <summary>
        /// Handles the exception and generates an error response.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception to handle.</param>
        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier;

            (int httpStatusCode, IReadOnlyList<string> messages, IReadOnlyDictionary<string, object>? metadata) = GetExceptionInfo(exception);

            ErrorApiResponse errorApiResponse = new()
            {
                Result = false,
                HttpStatusCode = httpStatusCode,
                Messages = messages,
                Metadata = metadata
            };

            httpContext.Response.ContentType = SystemConstant.APPLICATION_JSON;
            httpContext.Response.StatusCode = errorApiResponse.HttpStatusCode;

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string jsonResponse = JsonConvert.SerializeObject(errorApiResponse, serializerSettings);
            await httpContext.Response.WriteAsync(jsonResponse);
        }


        /// <summary>
        /// Retrieves the appropriate HTTP status code and message for the given exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>A tuple containing the HTTP status code, message and metadata</returns>
        public (int, IReadOnlyList<string>, IReadOnlyDictionary<string, object>?) GetExceptionInfo(Exception exception)
        {
            List<string> messages = new();
            int httpStatusCode = (int)HttpStatusCode.InternalServerError;

            void AddStatusCodeAndMessage(HttpStatusCode statusCode, string message)
            {
                httpStatusCode = (int)statusCode;
                messages.Add(message);
            }

            switch (exception)
            {
                case UnauthorizedAccessException unauthorizedAccessException:
                    AddStatusCodeAndMessage(HttpStatusCode.Unauthorized, unauthorizedAccessException.Message);
                    break;

                case AuthenticationException authenticationException:
                    AddStatusCodeAndMessage(HttpStatusCode.Unauthorized, authenticationException.Message);
                    break;

                case DbUpdateException dbUpdateException:
                    AddStatusCodeAndMessage(HttpStatusCode.BadRequest, dbUpdateException.Message);
                    break;

                case AppDomainUnloadedException appDomainUnloadedException:
                    AddStatusCodeAndMessage(HttpStatusCode.NoContent, appDomainUnloadedException.Message);
                    break;
               
                default:
                    messages.Add(exception.Message);
                    break;
            }

            return (httpStatusCode, messages, null);
        }
    }
}
