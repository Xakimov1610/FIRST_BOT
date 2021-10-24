using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrayerTime.Services;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using PrayerTime.Entity;

namespace PrayerTime
{
    public class Handlers
    {
        private readonly ILogger<Handlers> _logger;
        private readonly IStorageService _storage;
        private readonly TimingsByLLService _timings;
        public Handlers(ILogger<Handlers> logger, IStorageService storage, TimingsByLLService timings)
        {
            _logger = logger;
            _storage = storage;
            _timings = timings;
        }

        public Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken ctoken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException => $"Error occured with Telegram Client: {exception.Message}",
                _ => exception.Message
            };
            _logger.LogCritical(errorMessage);
            return Task.CompletedTask;
        }
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ctoken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageRecieved(client, update.Message),
                UpdateType.EditedMessage => BotOnMessageEdited(client, update.EditedMessage),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(client, update.CallbackQuery),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(client, update.InlineQuery),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(client, update.ChosenInlineResult),
                _ => UnknownUpdateHandlerAsync(client, update)
            };
            try
            {
                await handler;
            }
            catch(Exception e)
            {
                _logger.LogWarning(e.Message);
            }
        }

        private async Task UnknownUpdateHandlerAsync(ITelegramBotClient client, Update update)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnChosenInlineResultReceived(ITelegramBotClient client, ChosenInlineResult chosenInlineResult)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnInlineQueryReceived(ITelegramBotClient client, InlineQuery inlineQuery)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnCallbackQueryReceived(ITelegramBotClient client, CallbackQuery callbackQuery)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnMessageEdited(ITelegramBotClient client, Message editedMessage)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnMessageRecieved(ITelegramBotClient client, Message message)
        {
            if(message.Text == "/start")
            {
                var user = new BotUser(
                    chatId: message.Chat.Id,
                    username: message.From.Username,
                    fullname: $"{message.From.FirstName} {message.From.LastName}",
                    longitude: 0,
                    latitude: 0
                );
                var result = await _storage.InsertUserAsync(user);
                if(result.IsSuccess)
                {
                    _logger.LogInformation($"New user added: {user.ChatID} {user.Username}");
                }
                else
                {
                    _logger.LogInformation("User already exists");
                }
            }
            if(message.Location != null)
            {
                await client.SendTextMessageAsync(
                    message.Chat.Id,
                    "Lokatsiyangiz muvaffaqiyatli qabul qilindi",
                    replyToMessageId: message.MessageId,
                    replyMarkup: Buttons.MenuButtons()
                );
                var user = await _storage.GetUserAsync(message.Chat.Id);
                user.Longitude = message.Location.Longitude;
                user.Latitude = message.Location.Latitude;
                await _storage.UpdateUserAsync(user);
            }
            else
            {
                var _user = await _storage.GetUserAsync(message.Chat.Id);
                var a = message.Text switch
                {
                    "/start"    => await client.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Botga xush kelibsiz.\nLokatsiyangizni jo'natmasangiz bot ishlamaydi.",
                                    ParseMode.Markdown,
                                    replyMarkup: Buttons.GetLocationButton()),
                    "Sozlamalar"  => await client.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Sozlamalardan birini tanlang",
                                    ParseMode.Markdown,
                                    replyMarkup: Buttons.SettingsButtons(_user.Notifications)),
                    "Bugungi namoz vaqtlari"     => await client.SendTextMessageAsync(
                                    message.Chat.Id,
                                    await _timings.getTodayTimings(_user.Longitude ,_user.Latitude),
                                    ParseMode.Markdown,
                                    replyMarkup: Buttons.MenuButtons()),
                    "Ertangi namoz vaqtlari"  => await client.SendTextMessageAsync(
                                    message.Chat.Id,
                                    await _timings.getTomorrowTimings(_user.Longitude, _user.Latitude),
                                    ParseMode.Markdown,
                                    replyMarkup: Buttons.MenuButtons()),
                    "Menyuga qaytish" => await client.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Back to menu",
                                    ParseMode.Markdown,
                                    replyMarkup: Buttons.MenuButtons()),
                    "Bildirishnomalarni yoqish" => await client.SendTextMessageAsync(
                                    message.Chat.Id,
                                    $"Bildirishnomalar yoqildi{_user.setNotification()}",
                                    ParseMode.Markdown,
                                    replyMarkup: Buttons.SettingsButtons(_user.Notifications)),
                    "Bildirishnomalarni o'chirish" => await client.SendTextMessageAsync(
                                    message.Chat.Id,
                                    $"Bildirishnomalar o'chirildi{_user.setNotification()}",
                                    ParseMode.Markdown,
                                    replyMarkup: Buttons.SettingsButtons(_user.Notifications)),
                    _           => await client.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Hozircha shu.",
                                    ParseMode.Markdown)
                };
            }
        }
    }
}