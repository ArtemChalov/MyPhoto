using System.Windows.Media.Imaging;
using WriteableBitmapEx;

namespace MyPhoto.Types
{
    public class FolderContentInfo
    {
        public FolderContentInfo(string filepath, string filename)
        {
            FilePath = filepath;
            FileName = filename;
            Thumbnail = BitmapImageFactory.CreateThumbnailFromFile(filepath, 64, DesiredSize.Width);
        }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public BitmapImage Thumbnail { get; set; }
    }
}
