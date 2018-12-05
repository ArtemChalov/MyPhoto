using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WriteableBitmapEx;

namespace MyPhoto.Utilities
{
    class FileWorker
    {
        /// <summary>
        /// Method to open folder with dialog.
        /// </summary>
        /// <param name="dialogfilter">String with a standart dialog filter expression.</param>
        /// <param name="supportExt">String with a file extention pattern like "*.jpg|*.png".</param>
        /// <returns>Full file path or Null if file not found with the defined pattern.</returns>
        public string OpenWithDialog(string dialogfilter, string supportExt)
        {
            // Default path
            string lastpath = "C:\\";
            // Get the last opened directory to open with
            if (Directory.Exists(Properties.Settings.Default.DefaultOpenPath))
                lastpath = Properties.Settings.Default.DefaultOpenPath;

            OpenFileDialog opendialog = new OpenFileDialog
            {
                InitialDirectory = lastpath,
                Filter = dialogfilter,
                FilterIndex = 0,
                ValidateNames = false,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = "Открытие папки"
            };

            var res = opendialog.ShowDialog();

            if (res != null && res == true)
            {
                var filePath = opendialog.FileName;
                if (File.Exists(filePath) && supportExt.Contains(Path.GetExtension(filePath).ToLower())) return filePath;
                else
                    return GetSuitableFileFromDirectory(opendialog.FileName, supportExt);
            }

            return null;
        }

        private string GetSuitableFileFromDirectory(string filePath, string supportExt)
        {
            var dirInfo = new DirectoryInfo(filePath).Parent;
            // Set default result
            filePath = null;

            // Returns the first found file path satisfying support file extentions
            // or Null if not found
            filePath = dirInfo.GetFiles("*.*")
                        .Where((fi) => supportExt.Contains(Path.GetExtension(fi.Name).ToLower()))
                        .FirstOrDefault()?.FullName;

            if (filePath == null)
                MessageBox.Show($" В каталоге:\n {dirInfo.FullName}\nнет изображений.", "Открытие папки", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                // Set tha last opened directory to next open with
                Properties.Settings.Default.DefaultOpenPath = dirInfo.FullName;

            return filePath;
        }

        public string SaveFileWithDialog(Image img)
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

            return null;
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
