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
            Thumbnail = new WriteableBitmapFactory().CreateThumbnailFromFile(filepath, 64);
        }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public WriteableBitmap Thumbnail { get; set; }
    }
}
