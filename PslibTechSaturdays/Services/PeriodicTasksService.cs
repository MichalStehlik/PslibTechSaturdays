using PslibTechSaturdays.Data;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace PslibTechSaturdays.Services
{
    public class PeriodicTasksService : BackgroundService
    {
        private readonly ILogger<PeriodicTasksService> _logger;
        private readonly PeriodicTasksOptions _options;
        private readonly ApplicationDbContext _context;
        public IServiceProvider Services { get; }
        private Timer? _timer = null;

        public PeriodicTasksService(IServiceProvider services, ILogger<PeriodicTasksService> logger, IOptions<PeriodicTasksOptions> options/*, ApplicationDbContext context*/)
        {
            Services = services;
            _logger = logger;
            _options = options.Value;
            //_context = context;
        }

        public override Task StartAsync(CancellationToken stoppingToken)
        {
            base.StartAsync(stoppingToken);
            _logger.LogInformation("ProcessingTasks Service starting.");
            _timer = new Timer(async (state) =>
            {
                _logger.LogInformation("ProcessingTasks Service working on tasks.");
                using (var scope = Services.CreateScope())
                {
                    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    try
                    {
                        var res = await context.Groups.Where(g => g.PlannedOpening < DateTime.Now && g.OpenedAt == null).ExecuteUpdateAsync(g => g.SetProperty(x => x.OpenedAt, x => DateTime.Now));
                        _logger.LogInformation(res + " group opened.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Unable to open groups - " + ex.Message);
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
            await base.StopAsync(stoppingToken);
        }
    }

    public class PeriodicTasksOptions
    {
        public int Seconds { get; set; } = 60;
    }
}
