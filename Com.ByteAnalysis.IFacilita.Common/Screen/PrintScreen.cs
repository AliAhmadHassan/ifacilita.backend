using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Com.ByteAnalysis.IFacilita.Common.Screen
{
    public class PrintScreen
    {
        public string CaptureScreen()
        {
            try
            {
                using var bitmap = new Bitmap(1920, 1080);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0,
                    bitmap.Size, CopyPixelOperation.SourceCopy);
                }

                if (!Directory.Exists("Captures"))
                    Directory.CreateDirectory("Captures");

                var pathCapture = Path.Combine(Directory.GetCurrentDirectory(), "Captures", Guid.NewGuid().ToString("N"), ".jpg");

                bitmap.Save(pathCapture, ImageFormat.Jpeg);

                return pathCapture;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao tentar criar o print screen. " + ex.Message);
                return null;
            }
        }
    }
}
