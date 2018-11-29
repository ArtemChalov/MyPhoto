using Microsoft.Win32;
using System;
using System.IO;

namespace MyPhoto.Utilities
{
    class FileWorker
    {
        public string OpenFileWithDialog()
        {
            string defaultPath = "C:\\";
            if (Directory.Exists(Properties.Settings.Default.DefaultOpenPath))
            {
                defaultPath = Properties.Settings.Default.DefaultOpenPath;
            }

            OpenFileDialog opendialog = new OpenFileDialog
            {
                InitialDirectory = defaultPath,
                Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp|All files|*.*",
                FilterIndex = 0,
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Открытие папки"
            };

            var res = opendialog.ShowDialog();

            if (res != null && res == true)
            {
                if (!String.IsNullOrEmpty(opendialog.FileName)){
                    var ind = opendialog.FileName.LastIndexOf('\\');
                    string newFolderPath = opendialog.FileName.Substring(0, ind + 1);
                    Properties.Settings.Default.DefaultOpenPath = newFolderPath;

                    return opendialog.FileName;
                }
            }

            return null;
        }
    }
}
