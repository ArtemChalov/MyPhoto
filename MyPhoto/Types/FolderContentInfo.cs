
namespace MyPhoto.Types
{
    public struct FolderContentInfo
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
