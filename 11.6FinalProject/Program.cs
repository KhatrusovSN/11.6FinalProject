using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using _11._6FinalProject.Controllers;
using _11._6FinalProject.Configuration;
using _11._6FinalProject.Services;

namespace _11._6FinalProject
{
    internal class Program
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()  // Объект, отвечающий за постоянный жизненный цикл приложения
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Services Start.");
            await host.RunAsync(); // Запускаем сервис
            Console.WriteLine("Services Stop.");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddTransient<DefaultMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<TextMessageController>();

            services.AddSingleton<IStorage, MemoryStorage>();
            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "5792526387:AAEM4gsvrVM2B8JuPDxf9oxhfD_VVRXnTFA"
            };
        }
    }
}