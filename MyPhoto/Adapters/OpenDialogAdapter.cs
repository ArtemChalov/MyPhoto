using Microsoft.Win32;
using System.IO;
using UnFilemanager.Interfaces;
using UnFilemanager.Utilities;

namespace MyPhoto.Adapters
{
    class OpenDialogAdapter : IDialogWrapper
    {
        public string Path { get; set; }

        public string[] FileNames { get; private set; }

        public string FileName { get; private set; }

        public string CurrentDirectory { get; private set; }

        public bool ShowDialog()
        {
            OpenFileDialog opendialog = new OpenFileDialog
            {
                InitialDirectory = Properties.Settings.Default.DefaultOpenPath,
                Filter = FilterExpressionConverter.OpenDialogFilter(App.SupportFilesDictionary),
                FilterIndex = 0,
                Multiselect = true,
                ValidateNames = false,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = "Открытие папки"
            };

            var res = (bool)opendialog.ShowDialog();

            if (res == true)
            {
                Path = opendialog.FileName;
                FileName = opendialog.FileName;
                FileNames = opendialog.FileNames;
                CurrentDirectory = Directory.GetParent(Path).FullName;
                Properties.Settings.Default.DefaultOpenPath = CurrentDirectory;
            }

            return res;
        }
    }
}
