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
            string lastpath = "C:\\";
            // Get the last opened directory to open with
            if (Directory.Exists(Properties.Settings.Default.DefaultOpenPath))
                lastpath = Properties.Settings.Default.DefaultOpenPath;

            OpenFileDialog opendialog = new OpenFileDialog
            {
                InitialDirectory = lastpath,
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
                return TestedFileName(opendialog.FileName);
            }

            return null;
        }

        private string TestedFileName(string filepath)
        {
            var dirinfo = new DirectoryInfo(filepath);
            var folder = dirinfo.Parent;

            if (!File.Exists(filepath))
            {
                // Set default result
                filepath = null;
                // Try find out an image in the folder
                foreach (var finfo in folder.GetFiles())
                {
                    if (IsFileImage(finfo.Extension))
                    {
                        filepath = finfo.FullName;
                        break;
                    }
                }
            }

            if (filepath == null)
                MessageBox.Show($" В каталоге:\n {folder.FullName}\nнет изображений.", "Открытие папки", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                // Set tha last opened directory to next open with
                Properties.Settings.Default.DefaultOpenPath = folder.FullName;

            return filepath;
        }

        public static bool IsFileImage(string fileExtention)
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
