using System;
using System.Threading.Tasks;
using PrayerTime.Entity;

namespace PrayerTime.Services
{
    public interface IStorageService
    {
        Task<(bool IsSuccess, Exception exception)> InsertUserAsync(BotUser user);
        Task<(bool IsSuccess, Exception exception)> UpdateUserAsync(BotUser user);
        Task<bool> ExistsAsync(long chatid);
        Task<bool> ExistsAsync(string username);
        Task<(bool IsSuccess, Exception exception)> RemoveAsync(BotUser user);
        Task<BotUser> GetUserAsync(long chatid);
        Task<BotUser> GetUserAsync(string username);
    }
}