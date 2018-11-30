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
        private ImgPreviewTransformer _ImageViewTransformer;
        private bool _IsMenuOpened;
        private string _FilePath;
        private Image _Image;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ImagePresenterInit();
            MenuInit();
            ViewPortMenuInit();
        }

        #region Init methods

        private void ImagePresenterInit()
        {
            _Image = new Image();
            ImgViewer = new ScrollViewer
            {
                Padding = new Thickness(2),
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = _Image
            };
        }

        private void MenuInit()
        {
            MenuItemFactory itemFactory = new MenuItemFactory();

            MenuList = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            ICommand opencmd = new DelegateCommand((obj) => OpenFile());
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uED25", "Открыть", opencmd));
            ICommand savecmd = new DelegateCommand((obj) => SaveFile(), (obj) => _Image.Source != null);
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uE105", "Сохранить", savecmd));
            ICommand saveascmd = new DelegateCommand((obj) => SaveAsFile(), (obj) => _Image.Source != null);
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uEA35", "Сохранить как", saveascmd));

            itemFactory = null;
        }

        private void ViewPortMenuInit()
        {
            ViewPortMenuItemFactory itemVPFactory = new ViewPortMenuItemFactory();

            ViewPortMenu = new StackPanel()
            {
                Margin = new Thickness(5, 5, 0, 5),
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            _ImageViewTransformer = new ImgPreviewTransformer(_Image, 0, 0);

            itemVPFactory.CreateAllMenuItems(ViewPortMenu, _ImageViewTransformer, _Image);

            itemVPFactory = null;
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

        private void UpdateImage(string path)
        {
            _Image.Source = null;

            WriteableBitmapFactory factory = new WriteableBitmapFactory();
            // Create and show an image
            _Image.Source = factory.CreateFromFile(path);
            factory = null;

            _ImageViewTransformer.SetOriginalDimentions((_Image.Source as WriteableBitmap).PixelWidth, (_Image.Source as WriteableBitmap).PixelHeight);

            if (!String.IsNullOrEmpty(Properties.Settings.Default.DefaultPreview))
                _ImageViewTransformer.ExecuteTransformWith(Properties.Settings.Default.DefaultPreview);
        }

        #endregion

        #region Properties

        public ScrollViewer ImgViewer { get; set; }

        public StackPanel MenuList { get; set; }

        public StackPanel ViewPortMenu { get; set; }

        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                _FilePath = value;
                if (value != null) UpdateImage(value);
            }
        }

        public bool IsMenuOpened
        {
            get { return _IsMenuOpened; }
            set { _IsMenuOpened = value; OnPropertyChanged(); }
        }

        #endregion

        #region PropertyChanged interface

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_Image.Width != double.NaN)
                _Image.Width = double.NaN;
            if (_Image.Height != double.NaN)
                _Image.Height = double.NaN;
            if (_Image.Source != null)
                _ImageViewTransformer?.ExecuteTransformWith("FitToParent");
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
