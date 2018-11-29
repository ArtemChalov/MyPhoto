using MVVM;
using MyPhoto.Utilities;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WriteableBitmapEx;

namespace MyPhoto
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window, INotifyPropertyChanged
    {
        private Point originalPoint;
        private Thickness originalMargin;
        private ControlTransformer _ImageViewTransformer;
        private ICommand _ViewTransformCmd;
        private bool _IsMenuOpened;
        private string _FilePath;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            MenuInit();
            TransformerInit();
        }

        #region Init methods

        private void TransformerInit()
        {
            if (image.Source != null)
                _ImageViewTransformer =
                    new ControlTransformer(image, (image.Source as WriteableBitmap).PixelWidth, (image.Source as WriteableBitmap).PixelHeight);
        }

        private void MenuInit()
        {
            MenuItemFactory itemFactory = new MenuItemFactory();
            MenuList = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uED25", "Открыть", OpenFile));
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uE105", "Сохранить", SaveFile));
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uEA35", "Сохранить как", SaveAsFile));
            itemFactory = null;
        }

        #endregion

        #region Private methods

        private void SaveAsFile()
        {
            // Close menu panel
            IsMenuOpened = false;
            MessageBox.Show("Сохранить как");
        }

        private void SaveFile()
        {
            // Close menu panel
            IsMenuOpened = false;
            MessageBox.Show("Сохранить");
        }

        private void OpenFile()
        {
            // Close menu panel
            IsMenuOpened = false;
            FilePath = (new FileWorker().OpenFileWithDialog());
        }

        #endregion

        #region Properties

        public bool IsMenuOpened
        {
            get { return _IsMenuOpened; }
            set { _IsMenuOpened = value; OnPropertyChanged(); }
        }

        public StackPanel MenuList { get; set; }

        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                _FilePath = value;
                if (value != null)
                {
                    // Show an image by path
                    image.Source = (new WriteableBitmapFactory().CreateFromFile(value));
                }
            }
        }

        #endregion

        #region Commands

        public ICommand ViewTransformCmd
        {
            get
            {
                return _ViewTransformCmd ??
                (_ViewTransformCmd = new DelegateCommand((obj) =>
                _ImageViewTransformer.ExecuteTrasforWith(obj as string), (obj) => image.Source != null));
            }
        }

        #endregion

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

        #region PropertyChanged interface

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed) {
                image.CaptureMouse();
                image.Width = image.ActualWidth;
                image.Height = image.ActualHeight;
                originalPoint = e.GetPosition(image.Parent as IInputElement);
                originalMargin = image.Margin;
                Mouse.OverrideCursor = Cursors.ScrollAll;
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed) {
                Point newPoint = e.GetPosition(image.Parent as IInputElement);
                Thickness newMargin = new Thickness();
                newMargin.Left = originalMargin.Left + newPoint.X - originalPoint.X; ;
                newMargin.Top = originalMargin.Top + newPoint.Y - originalPoint.Y;
                newMargin.Right = -newMargin.Left;
                newMargin.Bottom = -newMargin.Top;
                image.Margin = newMargin;
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            image.ReleaseMouseCapture();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (image.Width != double.NaN)
                image.Width = double.NaN;
            if (image.Height != double.NaN)
                image.Height = double.NaN;
        }

        private void Menubtn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsMenuOpened) IsMenuOpened = true;
        }

        private void Closemenubtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsMenuOpened) IsMenuOpened = false;
        }

        #endregion
    }
}
