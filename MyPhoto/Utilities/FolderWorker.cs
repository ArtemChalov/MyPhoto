using MyPhoto.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhoto.Utilities
{
    class FolderWorker
    {
        public ObservableCollection<FolderContentInfo> UpLoadFolderContent(string filepath)
        {
            var dirInfo = new DirectoryInfo(filepath).Parent;
            ObservableCollection<FolderContentInfo> fileList = new ObservableCollection<FolderContentInfo>();

            foreach (var finfo in dirInfo.GetFiles())
            {
                if (FileWorker.IsFileImage(finfo.Extension))
                    fileList.Add(new FolderContentInfo(finfo.FullName, finfo.Name));
            }
            return fileList;
        }
    }
}
