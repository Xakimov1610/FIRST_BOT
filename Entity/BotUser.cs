using System.ComponentModel.DataAnnotations;
using System;
using System.Threading.Tasks;

namespace PrayerTime.Entity
{
    public class BotUser
    {
        [Key]
        public long ChatID {get; set;}
        public string Username {get; set;}
        public string Fullname {get; set;}
        public float Longitude {get; set;}
        public float Latitude {get; set;}
        public bool Notifications {get; set;}
        
        [Obsolete("Used only for Entity binding!")]
        public BotUser() {}

        public BotUser(long chatId, string username, string fullname, float longitude, float latitude)
        {
            ChatID = chatId;
            Username = username;
            Fullname = fullname;
            Longitude = longitude;
            Latitude = latitude;
            Notifications = true;
        }
        public string setNotification()
        {
            if(Notifications)
            {
                Notifications = false;
            }
            else
            {
                Notifications = true;
            }
            return null;
        }
        
    }
}