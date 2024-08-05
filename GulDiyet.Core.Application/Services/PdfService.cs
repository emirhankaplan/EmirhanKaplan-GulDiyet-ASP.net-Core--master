using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.DietPlan;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace GulDiyet.Core.Application.Services
{
    public class PdfService : IPdfService
    {
        public async Task<byte[]> GenerateDietPlanPdf(List<DietPlanViewModel> dietPlans)
        {
            return await Task.Run(() =>
            {
                using (var stream = new MemoryStream())
                {
                    var document = new PdfDocument();
                    foreach (var dietPlan in dietPlans)
                    {
                        var page = document.AddPage();
                        var gfx = XGraphics.FromPdfPage(page);
                        var titleFont = new XFont("Verdana", 20);
                        gfx.DrawString("Diet Plan", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);

                        var contentFont = new XFont("Verdana", 12);
                        gfx.DrawString($"Patient: {dietPlan.PatientName}", contentFont, XBrushes.Black, new XRect(40, 60, page.Width, page.Height));
                        gfx.DrawString($"Details: {dietPlan.PlanDetails}", contentFont, XBrushes.Black, new XRect(40, 90, page.Width, page.Height));
                    }
                    document.Save(stream, false);
                    return stream.ToArray();
                }
            });
        }
    }
}
