using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VPets.Domain.Repositories;
using VPets.Domain.Services;

namespace VPets.Services
{
    public class PetMetricStateService : BackgroundService
    {
        private const int TASK_DELAY_SECONDS = 10;

        private Timer metricDegradationTimer;

        public IServiceProvider Services { get; }

        public PetMetricStateService(IServiceProvider services)
        {
            Services = services;
        }

        private async Task DoWork()
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IPetService>();

                await scopedProcessingService.DegradeMetrics();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            metricDegradationTimer = new Timer(async e => await DoWork(), null, TimeSpan.Zero, TimeSpan.FromSeconds(TASK_DELAY_SECONDS));

            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            metricDegradationTimer?.Change(Timeout.Infinite, 0);

            await Task.CompletedTask;
        }
    }
}
