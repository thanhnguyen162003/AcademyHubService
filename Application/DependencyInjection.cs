using Application.Features.Common;
using Application.Middlewares;
using Application.Services.Authentication;
using Application.Services.BackgroundServices.BackgroundTask;
using Application.Services.BackgroundServices.Workers;
using Application.Services.KafkaService.Producer;
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
            builder.Services.AddScoped<IProducerService, ProducerService>();

            builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            builder.Services.AddHostedService<CommonWorkerService>();
        }
    }
}
