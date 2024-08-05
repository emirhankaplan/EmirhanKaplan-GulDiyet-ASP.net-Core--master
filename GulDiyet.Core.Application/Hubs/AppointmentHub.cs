using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Hubs
{
    public class AppointmentHub : Hub
    {
        public async Task SendAppointmentUpdate(int appointmentId)
        {
            await Clients.All.SendAsync("ReceiveAppointmentUpdate", appointmentId);
        }

        public async Task NotifyEvaluation(int evaluationId)
        {
            await Clients.All.SendAsync("ReceiveEvaluationNotification", evaluationId);
        }
    }
}

