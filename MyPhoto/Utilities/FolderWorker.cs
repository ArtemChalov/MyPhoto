using MyPhoto.Types;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MyPhoto.Utilities
{
    class FolderWorker
    {
        /// <summary>
        /// Method to find out the all files in a directory.
        /// </summary>
        /// <param name="filePath">The file path that determine a folder to find out the all files in with defined pattern.</param>
        /// <param name="supportExt">String with a file extention pattern like "*.jpg|*.png".</param>
        /// <returns>An ObservableCollection&lt;FolderContentInfo&gt; instance.</returns>
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
