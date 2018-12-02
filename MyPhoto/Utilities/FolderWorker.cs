using MyPhoto.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhoto.Utilities
{
    class FolderWorker
    {
        public List<FolderContentInfo> UpLoadFolderContent(string filepath)
        {
            var fileInfo = new FileInfo(filepath);

            var dirInfo = new DirectoryInfo(fileInfo.DirectoryName);
            List<FolderContentInfo> fileList = new List<FolderContentInfo>();

            foreach (var finfo in dirInfo.GetFiles())
            {
                if (IsFileImage(finfo.Extension))
                    fileList.Add(new FolderContentInfo(finfo.FullName, finfo.Name));
            }
            return fileList;
        }

        private bool IsFileImage(string fileExtention)
        {
            switch (fileExtention)
            {
                case ".jpg": return true;
                case ".jpeg": return true;
                case ".png": return true;
                case ".bmp": return true;
                case ".tiff": return true;
                case ".gif": return true;
                default: return false;
            }
        }

    }
}
