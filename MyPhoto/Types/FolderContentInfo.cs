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
        }

        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
