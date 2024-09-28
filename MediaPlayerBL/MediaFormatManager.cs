using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MediaInfo.DotNetWrapper; 

namespace MediaPlayerBL
{
    public static class MediaFormatManager
    {
        public static bool IsImageFormat(string format)
        {
            return Enum.TryParse<ImageFormats>(format, true, out _);
        }

        public static bool IsVideoFormat(string format)
        {
            return Enum.TryParse<VideoFormats>(format, true, out _);
        }

        public static BitmapImage CreateBitmap(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
        }

        public static Uri CreateVideoUri(string filePath)
        {
            return new Uri(filePath, UriKind.RelativeOrAbsolute);
        }

        public static async Task<int> GetVideoDurationAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                var mediaInfo = new MediaInfoWrapper(filePath);
                return (int)mediaInfo.Duration;
            });
        }
    }
}
