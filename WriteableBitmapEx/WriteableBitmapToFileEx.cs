using Dialogs.Windows;
using System.IO;
using System.Windows.Media.Imaging;

namespace MyPhoto.Utilities
{
    public static class WriteableBitmapToFileEx
    {
        public static void SaveToFile(this WriteableBitmap source, string filename)
        {
            var index = filename.LastIndexOf('.');
            var count = filename.Length - index;
            string fileExtention = filename.Substring(index, count).ToLower();

            BitmapEncoder BitmapEncoderGuid;
            switch (fileExtention)
            {
                case ".jpg":
                    BitmapEncoderGuid = new JpegBitmapEncoder();
                    break;
                case ".jpeg":
                    BitmapEncoderGuid = new JpegBitmapEncoder();
                    break;
                case ".png":
                    BitmapEncoderGuid = new PngBitmapEncoder();
                    break;
                case ".bmp":
                    BitmapEncoderGuid = new BmpBitmapEncoder();
                    break;
                case ".tiff":
                    BitmapEncoderGuid = new TiffBitmapEncoder();
                    break;
                case ".gif":
                    BitmapEncoderGuid = new GifBitmapEncoder();
                    break;
                default:
                    return;
            }

            if (BitmapEncoderGuid is JpegBitmapEncoder bitmapEncoder)
            {
                QualityDialogResult qualityDialog = new QualityDialogResult();
                if (qualityDialog.ShowDialog() == true)
                {
                    bitmapEncoder.QualityLevel = (int)(qualityDialog.Quality * 10);
                }
            }

            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                BitmapEncoderGuid.Frames.Add(BitmapFrame.Create(source));
                BitmapEncoderGuid.Save(stream);
            }
        }
    }
}
