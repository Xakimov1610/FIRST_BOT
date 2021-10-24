using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using PrayerTime.Entity;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace PrayerTime.Services
{
    public class InternalStorageService : IStorageService
    {
        private readonly List<BotUser> _users;
        private readonly ILogger<InternalStorageService> _logger;
        public InternalStorageService(ILogger<InternalStorageService> logger)
        {
            _users = new List<BotUser>();
            _logger = logger;
        }
        public Task<bool> ExistsAsync(long chatid)
            => Task.FromResult<bool>(_users.Any(u => u.ChatID == chatid));

        public Task<bool> ExistsAsync(string username)
            => Task.FromResult<bool>(_users.Any(u => u.Username == username));

        public Task<BotUser> GetUserAsync(long chatid)
            => Task.FromResult<BotUser>(_users.FirstOrDefault(u => u.ChatID == chatid));

        public Task<BotUser> GetUserAsync(string username)
            => Task.FromResult<BotUser>(_users.FirstOrDefault(u => u.Username == username));

        public async Task<(bool IsSuccess, Exception exception)> InsertUserAsync(BotUser user)
        {
            if(await ExistsAsync(user.ChatID))
            {
                return (false, new Exception("User already exists!"));
            }
            _users.Add(user);
            return (true, null);
        }

        public async Task<(bool IsSuccess, Exception exception)> RemoveAsync(BotUser user)
        {
            if(await ExistsAsync(user.ChatID))
            {
                var savedUser = await GetUserAsync(user.ChatID);
                _users.Remove(savedUser);
                return (true, null);
            }
            return (false, new Exception("User doesn't exist!"));
        }

        public async Task<(bool IsSuccess, Exception exception)> UpdateUserAsync(BotUser user)
        {
            if(await ExistsAsync(user.ChatID))
            {
                var savedUser = await GetUserAsync(user.ChatID);
                _users.Remove(savedUser);
                _users.Add(user);
                return (true, null);
            }
            return (false, new Exception("User doesn't exist!"));
        }
    }
}