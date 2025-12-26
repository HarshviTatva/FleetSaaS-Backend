using FleetSaaS.Domain.Common.Messages;

namespace FleetSaaS.API.Extensions
{
    public static class CorsExtension
    {
        private const string CorsPolicyName = Common.CORS_POLICY_NAME;

        public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var corsBaseUrl = configuration["ClientUrls:CorsUrl"];

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    policy.WithOrigins(corsBaseUrl!)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }

        public static void UseCorsPolicy(this WebApplication app)
        {
            app.UseCors(CorsPolicyName);
        }
    }
}
