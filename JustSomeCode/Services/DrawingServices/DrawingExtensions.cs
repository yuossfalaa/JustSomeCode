using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
namespace JustSomeCode.Services.DrawingServices
{
    public static class DrawingExtensions
    {        
        public static BitmapSource BitmapToBitmapSource(this Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                var result = new BitmapImage { CreateOptions = BitmapCreateOptions.PreservePixelFormat };
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
        /// Normilize point relative to the point
        public static Point Normalize(this Point point, Point origin)
        {
            var normalized = new Point(
                point.X - origin.X,
                point.Y - origin.Y);
            return normalized;
        }
    }
}
