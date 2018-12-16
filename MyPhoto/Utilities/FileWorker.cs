using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WriteableBitmapEx;

namespace MyPhoto.Utilities
{
    class FileWorker
    {
        public string SaveFileWithDialog(Image img)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { FileName = "*", DefaultExt = "jpg", ValidateNames = true };
            saveFileDialog.Filter = "All Files |*.*|JPEG Image |*.jpg;*.jpeg|Png Image |*.png|Bitmap Image |*.bmp|Gif Image |*.gif|Tiff Image |*.tiff|Wmf Image |*.wmf";
            saveFileDialog.DefaultExt = "jpg";

            var res = saveFileDialog.ShowDialog();

            if (res == true)
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
