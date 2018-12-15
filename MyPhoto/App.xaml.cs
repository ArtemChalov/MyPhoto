using System.Collections.Generic;
using System.Windows;

namespace MyPhoto
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] SupportExtentions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".tiff", ".gif" };

        public static Dictionary<string, string[]> SupportFilesDictionary = new Dictionary<string, string[]>
            {
                { "Image files",  SupportExtentions },
                { "JPEG", new string[] { ".jpg", ".jpeg" } },
                { "PNG", new string[] { ".png" } },
                { "BitMap", new string[] { ".bmp" } },
                { "TIFF", new string[] { ".tiff" } },
                { "GIF", new string[] { ".gif" } },
            };
    }
}
