namespace Project.Api.Application.BackgroundServices;

public sealed class DailyBookMetricsBackgroundService(IServiceScopeFactory scopeFactory, ILogger<DailyBookMetricsBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var now = DateTime.UtcNow;
        var nextMidnight = now.Date.AddDays(1);
        var delayUntilMidnight = nextMidnight - now;
        
        //if (delayUntilMidnight.TotalMilliseconds > 0)
        //{
        //    logger.LogInformation($"Daily metrics service will start at {nextMidnight:yyyy-MM-dd HH:mm:ss} UTC");
        //    await Task.Delay(delayUntilMidnight, stoppingToken);
        //}

        using var timer = new PeriodicTimer(TimeSpan.FromDays(1));

        try
        {
            do
            {
                using var scope = scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var booksCount = await unitOfWork.Books
                    .GetWhereAll(b => !b.IsDeleted)
                    .CountAsync(stoppingToken);

                var metricData = new
                {
                    count = booksCount,
                    date = DateTime.UtcNow.ToString("yyyy-MM-dd")
                };
                var jsonValue = JsonSerializer.Serialize(metricData);

                var metric = new Metric
                {
                    Key = "daily_book_count",
                    Value = jsonValue,
                    MeasuredAt = DateTime.UtcNow
                };

                unitOfWork.Metrics.Add(metric);
                await unitOfWork.SaveChangesAsync();

                logger.LogInformation($"✅ Daily book metrics recorded: {booksCount} books on {DateTime.UtcNow:yyyy-MM-dd}");
            }
            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Daily book metrics service was cancelled");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "X Error occurred in daily book metrics service");
        }
    }
}
