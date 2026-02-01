namespace Project.Api.Application.BackgroundServices;

public sealed class ExampleBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ExampleBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                using var unitOfWork = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

                var booksCount = await unitOfWork.Books.GetAll().CountAsync(stoppingToken);

                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Books Count: {booksCount}", booksCount);
            }
        }
        catch (OperationCanceledException ex)
        {
            throw new ArgumentException($"Background service was cancelled\n{ex.Message}");
        }
    }
}