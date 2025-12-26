using FleetSaaS.API.Middleware;
using FleetSaaS.Domain.Common.Messages;
using Serilog;

namespace FleetSaaS.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<TenantResolutionMiddleware>();

            // Serilog request logging
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = Common.SERILOG_MESSAGE;

                options.EnrichDiagnosticContext = (diagnostics, httpContext) =>
                {
                    diagnostics.Set("RequestPath", httpContext.Request.Path);
                    diagnostics.Set("RequestMethod", httpContext.Request.Method);
                    diagnostics.Set("TraceId", httpContext.TraceIdentifier);
                };
            });

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}


            app.UseHttpsRedirection();
           
            app.UseCorsPolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
