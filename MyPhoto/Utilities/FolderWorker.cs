using MyPhoto.Types;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MyPhoto.Utilities
{
    class FolderWorker
    {
        public ObservableCollection<FolderContentInfo> UpLoadFolderContent(string filePath, string supportExt)
        {
            var dirInfo = new DirectoryInfo(filePath).Parent;
            ObservableCollection<FolderContentInfo> fileList = new ObservableCollection<FolderContentInfo>();

            foreach (var finfo in dirInfo.GetFiles("*.*").Where((fi) => supportExt.Contains(Path.GetExtension(fi.Name).ToLower())))
            {
                fileList.Add(new FolderContentInfo(finfo.FullName, finfo.Name));
            }
            return fileList;
        }
    }
}
