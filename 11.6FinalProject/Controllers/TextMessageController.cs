using _11._6FinalProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace _11._6FinalProject.Controllers
{
    internal class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;
        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Считаем символы" , $"sumChar"),
                        InlineKeyboardButton.WithCallbackData($" Считаем сумму чисел" , $"sumValue")
                    });
                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот счетовод.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Можно написать сообщение и бот посчитает или ввести цифры и он выведет сумму.{Environment.NewLine}",
                        cancellationToken: ct,
                        parseMode: ParseMode.Html,
                        replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:
                    string operationCode = _memoryStorage.GetSession(message.Chat.Id).OperationCode;

                    if (operationCode == "sumChar")
                    {
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Кол-во символов в вашей строке = {message.Text.Length}", cancellationToken: ct);
                    }
                    else if (operationCode == "sumValue")
                    {
                        int result = 0;
                        try
                        {
                            string[] str = message.Text.Split(' ');
                            foreach (var s in str)
                            {
                                result += Int32.Parse(s);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Ошибка ввода - введите числа через пробел.", cancellationToken: ct);
                            return;
                        }
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел: {result}", cancellationToken: ct);
                    }
                    break;
            }
        }
    }
}
