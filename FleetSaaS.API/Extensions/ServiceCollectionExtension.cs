using FleetSaaS.API.Middleware;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IRepositories.Generic;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Application.Mapping;
using FleetSaaS.Application.Services;
using FleetSaaS.Application.Validators;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Infrastructure.Common;
using FleetSaaS.Infrastructure.Data;
using FleetSaaS.Infrastructure.Repositories;
using FleetSaaS.Infrastructure.Repositories.Generic;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Serilog;
using System.Text;

namespace FleetSaaS.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        //logging
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });
        }

        //services
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var services = builder.Services;
            var configuration = builder.Configuration;

            // DB
            services.AddDatabaseConnection(configuration);

            // CORS
            services.AddCorsPolicy(configuration);

            // Controllers
            services.AddControllers();

            // Fluent validation
            services.AddFluentValidationAutoValidation();

            // Auto-mapper
            services.AddAutoMapper(typeof(CompanyMappingProfile).Assembly);

            // HttpContext
            services.AddHttpContextAccessor();

            // Tenant
            services.AddScoped<ITenantProvider, TenantProvider>();

            // App services (repositories, services)
            services.RegisterServices();

            // FluentValidation 
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

            services.AddSwagger(configuration);

            services.AddTransient<GlobalExceptionHandler>();
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            //Swagger
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FleetSaaS.API",
                    Version = "v1"
                });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token as: Bearer {token}"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                        )
                    };
                });

            return services;
        }

        public static IServiceCollection AddDatabaseConnection(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")
                ));

            return services;
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            //services.AddScoped<FluentValidationFilter>();
            //services
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICompanyService,CompanyService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IDispatcherService, DispatcherService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IDashboardService, DashboardService>();

            //repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IDispatcherRepository, DispatcherRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<ICommonRepository,CommonRepository>();
        }
    }
}
