using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace MicroclimateSystem.Helpers
{
    public class PdfExporter
    {
        public static void ExportToPdf(CalculationResult result, string filePath)
        {
            using (PdfDocument document = new PdfDocument())
            {
                document.Info.Title = "Отчёт по расчёту микроклимата";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Verdana", 12);

                int top = 50;
                int step = 20;

                gfx.DrawString($"Название помещения: {result.RoomName}", font, XBrushes.Black, new XRect(40, top, page.Width, page.Height), XStringFormats.TopLeft); top += step;
                gfx.DrawString($"Дата: {result.Date}", font, XBrushes.Black, new XRect(40, top, page.Width, page.Height), XStringFormats.TopLeft); top += step;
                gfx.DrawString($"Температура: {result.Temperature} °C", font, XBrushes.Black, new XRect(40, top, page.Width, page.Height), XStringFormats.TopLeft); top += step;
                gfx.DrawString($"Влажность: {result.Humidity} %", font, XBrushes.Black, new XRect(40, top, page.Width, page.Height), XStringFormats.TopLeft); top += step;
                gfx.DrawString($"Скорость вентиляции: {result.VentilationRate} м³/ч", font, XBrushes.Black, new XRect(40, top, page.Width, page.Height), XStringFormats.TopLeft); top += step;
                gfx.DrawString($"Заметки: {result.Notes}", font, XBrushes.Black, new XRect(40, top, page.Width, page.Height), XStringFormats.TopLeft);

                document.Save(filePath);
            }
        }
    }
}
