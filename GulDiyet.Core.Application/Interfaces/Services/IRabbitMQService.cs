using GulDiyet.Core.Application.ViewModels.Email;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IRabbitMQService
    {
        void SendMessage(SaveEmailViewModel emailViewModel);
        void StartListening();
    }
}
