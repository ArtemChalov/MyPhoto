using Microsoft.Win32;
using MyPhoto.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnFilemanager;
using UnFilemanager.Interfaces;
using UnFilemanager.Utilities;

namespace MyPhoto.Adapters
{
    class OpenDialogAdapter : IDialogWrapper
    {
        public string Path { get; set; }

        public string[] FileNames { get; private set; }

        public string FileName { get; private set; }

        public string InitialDirectory { get; private set; }

        public bool ShowDialog()
        {
            // Default path
            string lastpath = "C:\\";
            // Get the last opened directory to open with
            if (Directory.Exists(Properties.Settings.Default.DefaultOpenPath))
                lastpath = Properties.Settings.Default.DefaultOpenPath;

            OpenFileDialog opendialog = new OpenFileDialog
            {
                InitialDirectory = lastpath,
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
                InitialDirectory = opendialog.InitialDirectory;
            }

            return res;
        }
    }
}
