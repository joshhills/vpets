using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VPets.Domain.Services;

namespace VPets.Services
{
    /// <summary>
    /// Create a background service to degrade pet metrics over time -
    /// this is less efficient than a queued service, and may not scale.
    /// </summary>
    public class PetMetricStateService : BackgroundService
    {
        /// <summary>
        /// How long to tick.
        /// </summary>
        private const int TASK_DELAY_SECONDS = 10;

        /// <summary>
        /// Timer control firing the work operation.
        /// </summary>
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
                // Get access to scoped services for the duration of the call.
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IPetService>();

                await scopedProcessingService.DegradeMetrics();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Called on start-up, initialise timer.
            metricDegradationTimer = new Timer(async e => await DoWork(),
                null, TimeSpan.Zero, TimeSpan.FromSeconds(TASK_DELAY_SECONDS));

            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            metricDegradationTimer?.Change(Timeout.Infinite, 0);

            await Task.CompletedTask;
        }
    }
}
