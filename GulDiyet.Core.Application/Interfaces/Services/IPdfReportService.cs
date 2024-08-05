using GulDiyet.Core.Application.ViewModels.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IPdfReportService
    {       
        Task<byte[]> GeneratePdf(AppointmentViewModel appointment);
    }
}

