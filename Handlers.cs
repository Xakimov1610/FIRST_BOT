using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot
{
    public class Handlers
    {
        private static ILogger<Handlers> _logger;
        public static MessageBuilder _messageBuilder;

        public Handlers(ILogger<Handlers> logger, MessageBuilder messageBuilder)
        {
            _messageBuilder = messageBuilder;
            _logger = logger;
        }

        public static Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken ctoken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException => $"Error occured with Telegram Client: {exception.Message}",
                _ => exception.Message
            };

            _logger.LogCritical(errorMessage);

            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ctoken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(client, update.Message),
                UpdateType.EditedMessage => BotOnMessageEdited(client, update.EditedMessage),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(client, update.CallbackQuery),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(client, update.InlineQuery),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(client, update.ChosenInlineResult),
                _ => UnknownUpdateHandlerAsync(client, update)
            };

            await handler;
        }

        private static async Task BotOnMessageEdited(ITelegramBotClient client, Message editedMessage)
        {
            throw new NotImplementedException();
        }

        private static async Task UnknownUpdateHandlerAsync(ITelegramBotClient client, Update update)
        {
            throw new NotImplementedException();
        }

        private static async Task BotOnChosenInlineResultReceived(ITelegramBotClient client, ChosenInlineResult chosenInlineResult)
        {
            throw new NotImplementedException();
        }

        private static async Task BotOnInlineQueryReceived(ITelegramBotClient client, InlineQuery inlineQuery)
        {
            throw new NotImplementedException();
        }

        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient client, CallbackQuery callbackQuery)
        {
            throw new NotImplementedException();
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient client, Message message)
        {
            Console.WriteLine($"{message.Text}");
            var a = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Share location?",
                parseMode: ParseMode.Markdown,
                replyMarkup: new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Share", RequestLocation = true }
                                },
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Cancel" } 
                                }
                            }
            });

            
            switch(message.Text)
            {
                case "/start": 
                    break;
            }

        }
    }
}