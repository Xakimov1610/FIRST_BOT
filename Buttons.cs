using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrayerTime
{
    public class Buttons
    {
        public static IReplyMarkup GetLocationButton()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Lokatsiyani jo'natish", RequestLocation = true}}
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup MenuButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Bugungi namoz vaqtlari"}, new KeyboardButton {Text = "Ertangi namoz vaqtlari"}},
                    new List<KeyboardButton>{ new KeyboardButton {Text = "Keyingi namoz vaqti"}, new KeyboardButton {Text = "Sozlamalar"}}
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup SettingsButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Lokatsiyani yangilash", RequestLocation = true }, new KeyboardButton {Text = "Bildirishnomalarni yoqish"}},
                    new List<KeyboardButton>{ new KeyboardButton {Text = "Menyuga qaytish"}}
                },
                ResizeKeyboard = true
            };
        }
    }
}