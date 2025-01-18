using PslibTechSaturdays.Data;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace PslibTechSaturdays.Services
{
    public class PeriodicTasksService : BackgroundService
    {
        private readonly ILogger<PeriodicTasksService> _logger;
        private readonly PeriodicTasksOptions _options;
        public IServiceProvider Services { get; }
        private Timer? _timer = null;

        public PeriodicTasksService(IServiceProvider services, ILogger<PeriodicTasksService> logger, IOptions<PeriodicTasksOptions> options)
        {
            Services = services;
            _logger = logger;
            _options = options.Value;
        }

        public override Task StartAsync(CancellationToken stoppingToken)
        {
            base.StartAsync(stoppingToken);
            _logger.LogInformation("ProcessingTasks Service starting.");
            _timer = new Timer(async (state) =>
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("ProcessingTasks Service cancellation requested.");
                    return;
                }

                _logger.LogInformation("ProcessingTasks Service working on tasks.");
                using (var scope = Services.CreateScope())
                {
                    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var stopwatch = Stopwatch.StartNew();
                    try
                    {
                        var res = await context.Groups
                            .Where(g => g.PlannedOpening < DateTime.Now && g.OpenedAt == null)
                            .ExecuteUpdateAsync(g => g
                                .SetProperty(x => x.OpenedAt, x => DateTime.Now)
                                .SetProperty(x => x.EnrollmentsCountVisible, x => true),
                                stoppingToken);
                        stopwatch.Stop();
                        _logger.LogInformation($"{res} groups opened and enrollments count visibility updated in {stopwatch.ElapsedMilliseconds} ms.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Unable to update groups: {ex.Message}");
                        _logger.LogError(ex.StackTrace);
                    }
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(_options.Seconds));

            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ProcessingTasks Service running.");

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ProcessingTasks Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();

            await base.StopAsync(stoppingToken);
        }
    }

    public class PeriodicTasksOptions
    {
        public int Seconds { get; set; } = 60;
    }
}
