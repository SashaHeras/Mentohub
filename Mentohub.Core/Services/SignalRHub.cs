using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services
{
    public class SignalRHub:Hub
    {
        public async Task SendAvatarUpdate(string userId, string avatarUrl)
        {
            // Отправка сповіщення про зміну аватарки клієнтам
            await Clients.User(userId).SendAsync("ReceiveAvatarUpdate", avatarUrl);
        }
    }
}
