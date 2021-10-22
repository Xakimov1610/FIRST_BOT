using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace bot
{
    public class Bot : BackgroundService
    {
        private readonly TelegramBotClient _botClient;

        public readonly ILogger<Bot> _logger;

        public Bot(TelegramBotClient client, ILogger<Bot> logger)
        {
            _botClient = client;
            _logger = logger;
            _botClient.StartReceiving(new DefaultUpdateHandler(Handlers.HandleUpdateAsync, Handlers.HandleErrorAsync), new CancellationToken());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var me = await _botClient.GetMeAsync();
            _logger.LogInformation($"{me.Username} has connected successfully!");
        }
    }
}