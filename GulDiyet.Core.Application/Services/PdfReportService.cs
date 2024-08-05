using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Appointment;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Services
{
    public class PdfReportService : IPdfReportService
    {
        public Task<byte[]> GeneratePdf(AppointmentViewModel appointment)
        {
            // PDF oluşturma mantığını burada olacak
            return Task.FromResult(new byte[0]);
        }
    }
}
