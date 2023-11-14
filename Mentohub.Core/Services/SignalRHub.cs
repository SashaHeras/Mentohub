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
        public async Task SendAvatarUpdate(string userId, string avatarUrl)
        {
            // Отправка сповіщення про зміну аватарки клієнтам
            await Clients.User(userId).SendAsync("ReceiveAvatarUpdate", avatarUrl);
        }
        public async Task ReceiveEmail(MimeMessage email)
        {
            
            // Отримання ідентифікатора з'єднання користувача
            var connectionId = Context.ConnectionId;

            // Відправка повідомлення конкретному користувачеві
            await Clients.Client(connectionId).SendAsync("EmailReceived", email);
        }
    }
}
