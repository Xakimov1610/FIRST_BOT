using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace bot
{
    class Program
    {
        static Task Main(string[] args)
            => CreateHostBuilder(args)
                .Build()
                .RunAsync();

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices(Configure);

        private static void Configure(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<TelegramBotClient>(b => new TelegramBotClient("2059933075:AAHeiko4TqgGNihwHFQk1AS1sanrY-4w9qI`"));
            services.AddHostedService<Bot>();
        }
    }
}
