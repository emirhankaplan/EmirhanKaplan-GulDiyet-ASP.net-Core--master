using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GulDiyet.Core.Application.ViewModels.Appointment;
using GulDiyet.Core.Domain.Entities;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IAppointmentFeedbackService
    {
        Task SaveFeedbackAsync(AppointmentFeedbackViewModel feedbackVm);
    }
}