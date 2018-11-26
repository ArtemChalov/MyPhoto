using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyPhoto
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public static WriteableBitmap CreateFromFile(string filePath)
        {
            BitmapImage bmpImage = new BitmapImage();
            WriteableBitmap wBitmap = null;
            try {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    bmpImage.BeginInit();
                    bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                    bmpImage.StreamSource = fs;
                    bmpImage.EndInit();
                }

                wBitmap = new WriteableBitmap(bmpImage);
            }
            catch {
                MessageBox.Show("Файл имеет не верный формат\nили поврежден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally {
                bmpImage.StreamSource = null;
                bmpImage = null;
            }
            return wBitmap;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
