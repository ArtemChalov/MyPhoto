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

namespace MyPhoto.Adapters
{
    class OpenDialogAdapter : IDialogWrapper
    {
        Dictionary<string, string[]> _supportExtentions;

        public OpenDialogAdapter(Dictionary<string,string[]> supportExtentions)
        {
            _supportExtentions = supportExtentions;
        }
        public string Path { get; set; }

        public bool ShowDialog()
        {
            // Default path
            string lastpath = "C:\\";
            // Get the last opened directory to open with
            if (Directory.Exists(Properties.Settings.Default.DefaultOpenPath))
                lastpath = Properties.Settings.Default.DefaultOpenPath;
            var ss = OpenDialogFilterCombainer.CombainFilter(_supportExtentions);

            OpenFileDialog opendialog = new OpenFileDialog
            {
                InitialDirectory = lastpath,
                Filter = ss,
                FilterIndex = 0,
                ValidateNames = false,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = "Открытие папки"
            };

            var res = (bool)opendialog.ShowDialog();

            if (res == true) Path = opendialog.FileName;

            return res;
        }
    }
}
