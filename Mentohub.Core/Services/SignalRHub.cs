using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services
{
    public class SignalRHub:Hub
    {
        private readonly UserManager<CurrentUser> _userManager;
        public SignalRHub(UserManager<CurrentUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SendAvatarUpdate(string userId, string avatarUrl)
        {
            // Отправка сповіщення про зміну аватарки клієнтам
            await Clients.User(userId).SendAsync("ReceiveAvatarUpdate", avatarUrl);
        }
        public async Task ReceiveEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Отримання ідентифікатора з'єднання користувача
                var connectionId = Context.ConnectionId;

            // Відправка повідомлення конкретному користувачеві
            await Clients.Client(connectionId).SendAsync("EmailReceived", email);

            }
                
        }
    }
}
