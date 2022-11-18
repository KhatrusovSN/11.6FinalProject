using _11._6FinalProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace _11._6FinalProject.Controllers
{
    internal class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;
        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
        {
            if(callbackQuery?.Data == null)
                return;

            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку {callbackQuery.Data}.");

            _memoryStorage.GetSession(callbackQuery.From.Id).OperationCode = callbackQuery.Data switch
            {
                "sumChar" => "sumChar",
                "sumValue" => "sumValue",
                _ => "sumValue",
            };

            string buttomText = callbackQuery.Data switch
            {
                "sumChar" => "cчитать символы.",
                "sumValue" => "cчитать сумму чисел.",
                _ => String.Empty
            };

            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, 
                $"<b> Сейчас бот будет - {buttomText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine} Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
