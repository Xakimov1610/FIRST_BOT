using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot
{
    public class MessageBuilder
    {
        public string GreetingMessage()
        {
            return "Hello World";
        }

        public ReplyKeyboardMarkup LocationRequestButton()
            => new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Share", RequestLocation = true },
                                    new KeyboardButton(){ Text = "Cancel" } 
                                }
                            }
            };
    }
}