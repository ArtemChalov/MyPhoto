using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imgfolderpreview
{
    public class FolderPresenter
    {
        public List<FileInfo> FileList;

        public FolderPresenter()
        {
            FileList = new List<FileInfo>();
        }

        public void LoadImagesFileList(string folderPath)
        {
            var dirInfo = new DirectoryInfo(folderPath);
            FileList.Clear();

            foreach (var finfo in dirInfo.GetFiles()) {
                if (IsFileImage(finfo.Name))
                    FileList.Add(finfo);
            }
        }

        private bool IsFileImage(string fileName)
        {
            var index = fileName.LastIndexOf('.');
            var count = fileName.Length - index;

            string fileExtention = fileName.Substring(index, count).ToLower();

            switch (fileExtention) {
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
