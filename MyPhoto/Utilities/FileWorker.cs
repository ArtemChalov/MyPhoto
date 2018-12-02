using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = "Открытие папки"
            };

            var res = opendialog.ShowDialog();

            if (res != null && res == true)
            {
                if (TestFileName(opendialog.FileName) == null)
                {
                    return null;
                }
                else return opendialog.FileName;
            }

            return null;
        }

        private string TestFileName(string filepath)
        {
            var dirinfo = new DirectoryInfo(filepath);
            var folder = dirinfo.Parent;

            if (Directory.Exists(folder.FullName))
                Properties.Settings.Default.DefaultOpenPath = folder.FullName;

            if (File.Exists(filepath))
                return filepath;

            return null;
        }

        public string SaveFileWithDialog(Image img, string currentpath)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { FileName = "*", DefaultExt = "jpg", ValidateNames = true };
            saveFileDialog.Filter = "All Files |*.*|JPEG Image |*.jpg;*.jpeg|Png Image |*.png|Bitmap Image |*.bmp|Gif Image |*.gif|Tiff Image |*.tiff|Wmf Image |*.wmf";
            saveFileDialog.DefaultExt = "jpg";

            var res = saveFileDialog.ShowDialog();

            if (res != null && res == true)
            {
                if (img.Source is WriteableBitmap source)
                {
                    try
                    {
                        source.SaveToFile(saveFileDialog.FileName);
                        return saveFileDialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                    MessageBox.Show("No source");
            }

            return currentpath;
        }

        public void SaveFile(Image img, string currentpath)
        {
            if (img.Source is WriteableBitmap source)
            {
                try
                {
                    source.SaveToFile(currentpath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("No source");
        }
    }
}
