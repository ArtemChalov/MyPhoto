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
            { "JPEG Image", new string[] { ".jpg", ".jpeg" } },
            { "PNG Image", new string[] { ".png" } },
            { "Bitmap Image", new string[] { ".bmp" } },
            { "TIFF Image", new string[] { ".tiff" } },
            { "GIF Image", new string[] { ".gif" } }
        };
        public static Dictionary<string, string[]> SupportSaveDictionary = new Dictionary<string, string[]>
        {
            { "JPEG Image", new string[] { ".jpg", ".jpeg" } },
            { "PNG Image", new string[] { ".png" } },
            { "Bitmap Image", new string[] { ".bmp" } },
            { "TIFF Image", new string[] { ".tiff" } },
            { "GIF Image", new string[] { ".gif" } }
        };
    }
}
