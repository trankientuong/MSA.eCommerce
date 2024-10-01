using System.Reflection;
using Contracts.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.MassTransit;

public static class Extensions 
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(configure => {
            configure.AddConsumers(Assembly.GetEntryAssembly());

            configure.UsingRabbitMq((context, configurator) => 
            {
                var configuration = context.GetService<IConfiguration>();
                var serviceSetting = configuration.GetSection(nameof(ServiceSetting)).Get<ServiceSetting>();
                RabbitMQSetting rabitMQSetting = configuration.GetSection(nameof(RabbitMQSetting)).Get<RabbitMQSetting>();
                configurator.Host(rabitMQSetting.Host, h => 
                {
                    h.Username("user");
                    h.Password("mypassword");
                });

                configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSetting.ServiceName, false));
                configurator.UseMessageRetry(retryPoilicy =>
                {
                    retryPoilicy.Interval(3, TimeSpan.FromSeconds(10));
                });
            });
        });

        return services;
    }
}