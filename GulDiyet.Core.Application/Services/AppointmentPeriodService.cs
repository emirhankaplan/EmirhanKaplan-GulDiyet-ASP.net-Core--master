using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Services
{
    public class AppointmentPeriodService : IAppointmentPeriodService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentPeriodService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<AppointmentPeriodViewModel>> GetAvailablePeriodsAsync(DateTime date)
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            var periods = new AppointmentPeriodViewModel { Date = date };
            var occupiedPeriods = appointments
                .Where(a => a.Day.Date == date.Date)
                .Select(a => a.Time)
                .ToList();

            periods.AvailablePeriods = periods.AvailablePeriods
                .Where(p => !occupiedPeriods.Contains(p))
                .ToList();

            return new List<AppointmentPeriodViewModel> { periods };
        }
    }
}
