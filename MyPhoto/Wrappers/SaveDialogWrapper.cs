using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using UnFilemanager.Utilities;
using WriteableBitmapEx;

namespace MyPhoto.Wrappers
{
    class SaveDialogWrapper
    {
        public string SaveFileWithDialog(Image img)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { FileName = "*", DefaultExt = "jpg", ValidateNames = true };
            saveFileDialog.Filter = FilterExpressionConverter.OpenDialogFilter(App.SupportSaveDictionary);
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
