using GulDiyet.Core.Application.ViewModels.Evaluation;
using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.LaboratoryResult;
using GulDiyet.Core.Application.ViewModels.LaboratoryTests;
using GulDiyet.Core.Application.ViewModels.Patients;

namespace GulDiyet.Core.Application.Helpers
{
    public static class PdfHelper
    {
        public static async Task<byte[]> CreateEvaluationPdf(SaveEvaluationViewModel evaluation)
        {
            return await Task.Run(() =>
            {
                using (var stream = new MemoryStream())
                {
                    var document = new PdfDocument();
                    var page = document.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);
                    var titleFont = new XFont("Verdana", 20);
                    var textFormatter = new XTextFormatter(gfx);

                    gfx.DrawString("Evaluation Report", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);

                    var contentFont = new XFont("Verdana", 12);
                    textFormatter.DrawString($"Evaluation ID: {evaluation.Id}", contentFont, XBrushes.Black, new XRect(40, 60, page.Width, page.Height));
                    textFormatter.DrawString($"Appointment ID: {evaluation.AppointmentId}", contentFont, XBrushes.Black, new XRect(40, 90, page.Width, page.Height));
                    textFormatter.DrawString($"Rating: {evaluation.Rating}", contentFont, XBrushes.Black, new XRect(40, 120, page.Width, page.Height));
                    textFormatter.DrawString($"Feedback: {evaluation.Feedback}", contentFont, XBrushes.Black, new XRect(40, 150, page.Width, page.Height));

                    document.Save(stream, false);
                    return stream.ToArray();
                }
            });
        }

        public static async Task<byte[]> CreateLaboratoryResultPdf(SaveLaboratoryResultViewModel labResult)
        {
            return await Task.Run(() =>
            {
                using (var stream = new MemoryStream())
                {
                    var document = new PdfDocument();
                    var page = document.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);
                    var titleFont = new XFont("Verdana", 20);
                    var textFormatter = new XTextFormatter(gfx);

                    gfx.DrawString("Laboratory Result", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);

                    var contentFont = new XFont("Verdana", 12);
                    textFormatter.DrawString($"Laboratory Result ID: {labResult.Id}", contentFont, XBrushes.Black, new XRect(40, 60, page.Width, page.Height));
                    textFormatter.DrawString($"Result: {labResult.Resultado}", contentFont, XBrushes.Black, new XRect(40, 90, page.Width, page.Height));
                    textFormatter.DrawString($"Is Completed: {labResult.IsCompleted}", contentFont, XBrushes.Black, new XRect(40, 120, page.Width, page.Height));

                    document.Save(stream, false);
                    return stream.ToArray();
                }
            });
        }

        public static async Task<byte[]> CreateLaboratoryTestPdf(SaveLaboratoryTestViewModel labTest)
        {
            return await Task.Run(() =>
            {
                using (var stream = new MemoryStream())
                {
                    var document = new PdfDocument();
                    var page = document.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);
                    var titleFont = new XFont("Verdana", 20);
                    var textFormatter = new XTextFormatter(gfx);

                    gfx.DrawString("Laboratory Test", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);

                    var contentFont = new XFont("Verdana", 12);
                    textFormatter.DrawString($"Laboratory Test ID: {labTest.Id}", contentFont, XBrushes.Black, new XRect(40, 60, page.Width, page.Height));
                    textFormatter.DrawString($"Test Name: {labTest.Name}", contentFont, XBrushes.Black, new XRect(40, 90, page.Width, page.Height));
                    textFormatter.DrawString($"Description: {labTest.Description}", contentFont, XBrushes.Black, new XRect(40, 120, page.Width, page.Height));

                    document.Save(stream, false);
                    return stream.ToArray();
                }
            });
        }

        public static async Task<byte[]> CreatePatientReportPdf(SavePatientViewModel patient)
        {
            return await Task.Run(() =>
            {
                using (var stream = new MemoryStream())
                {
                    var document = new PdfDocument();
                    var page = document.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);
                    var titleFont = new XFont("Verdana", 20);
                    var textFormatter = new XTextFormatter(gfx);

                    gfx.DrawString("Patient Report", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);

                    var contentFont = new XFont("Verdana", 12);
                    textFormatter.DrawString($"Patient ID: {patient.Id}", contentFont, XBrushes.Black, new XRect(40, 60, page.Width, page.Height));
                    textFormatter.DrawString($"Email: {patient.Email}", contentFont, XBrushes.Black, new XRect(40, 90, page.Width, page.Height));
                    textFormatter.DrawString($"Name: {patient.FirstName} {patient.LastName}", contentFont, XBrushes.Black, new XRect(40, 120, page.Width, page.Height));

                    document.Save(stream, false);
                    return stream.ToArray();
                }
            });
        }
    }
}
