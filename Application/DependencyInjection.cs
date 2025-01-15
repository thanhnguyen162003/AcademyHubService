using Application.Features.Common;
using Application.Middlewares;
using Application.Services.Authentication;
using FluentValidation;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this WebApplicationBuilder builder)
        {
            // Set up global exception handler
            builder.Services.AddScoped<GlobalException>();

            // Set up AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Set up Validation
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });

            // Add Services
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}
