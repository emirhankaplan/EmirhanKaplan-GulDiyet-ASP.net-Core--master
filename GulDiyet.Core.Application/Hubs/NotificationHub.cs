using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public async Task SendEvaluationUpdate(int evaluationId)
        {
            await Clients.All.SendAsync("ReceiveEvaluationUpdate", evaluationId);
        }
    }
}
