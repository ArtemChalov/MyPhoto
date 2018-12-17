using FolderContentPresenter;
using MyPhoto.Types;
using MyPhoto.Utilities;
using MyPhoto.Wrappers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UnFilemanager;
using UnFilemanager.Filters;
using WriteableBitmapEx;

namespace MyPhoto
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Image _Image;
        private string _FilePath;
        private ObservableCollection<FolderContentInfo> _FolderContent;
        private ImgPreviewTransformer _ImageViewTransformer;
        private FolderContentInfo _SelectedPreviewImage;
        private PresenterViewModel _folderPreview;

        AppStateKeeper _StateKeeper;

        //private bool _IsMenuOpened;
        //private bool _FolderContentIsOld;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ImagePresenterInit();
            FolderPresenterInit();
            MenuInit();
            ViewPortMenuInit();
            StateKeeper = new AppStateKeeper();
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

        private void FolderPresenterInit()
        {
            FolderPresenter = new PresenterView();
            _folderPreview = FolderPresenter.DataContext as PresenterViewModel;
            _folderPreview.SupportExtentions = App.SupportExtentions;
        }

        private void MenuInit()
        {
            MenuItemFactory itemFactory = new MenuItemFactory();

            MenuList = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uED25", ApplicationCommands.Open.Text, ApplicationCommands.Open));
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uE105", ApplicationCommands.Save.Text, ApplicationCommands.Save));
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uEA35", ApplicationCommands.SaveAs.Text, ApplicationCommands.SaveAs));
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uE8C8", ApplicationCommands.Copy.Text, ApplicationCommands.Copy));
            MenuList.Children.Add(itemFactory.CreateHSeparator((Color)(new ColorConverter().ConvertFrom("#FF2C628B"))));
            MenuList.Children.Add(itemFactory.CreateMenuItem("\uE107", ApplicationCommands.Delete.Text, ApplicationCommands.Delete));

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

        private async void UploadImage(string path)
        {
            _Image.Source = null;

            if (path == null) return;

            var previewheight = (int)(ImgViewer.ActualHeight - 3.5);

            // Create and show a preview image
            // to get fast load
            _Image.Source = BitmapImageFactory.CreateThumbnailFromFile(path, previewheight, WriteableBitmapEx.DesiredSize.Height);
            _ImageViewTransformer.SetOriginalDimentions((_Image.Source as BitmapImage).PixelWidth, (_Image.Source as BitmapImage).PixelHeight);
            if (!String.IsNullOrEmpty(Properties.Settings.Default.DefaultPreview))
                _ImageViewTransformer.ExecuteTransformWith(Properties.Settings.Default.DefaultPreview);

            // Create and show the full size image
            _Image.Source = await WriteableBitmapFactory.CreateFromFileAsync(path);
            _ImageViewTransformer.SetOriginalDimentions((_Image.Source as WriteableBitmap).PixelWidth, (_Image.Source as WriteableBitmap).PixelHeight);
            if (!String.IsNullOrEmpty(Properties.Settings.Default.DefaultPreview))
                _ImageViewTransformer.ExecuteTransformWith(Properties.Settings.Default.DefaultPreview);

            if (StateKeeper.FolderContentIsOld) UploadFolderContentAsync();
        }

        private async void UploadFolderContentAsync()
        {
            StateKeeper.FolderContentIsOld = false;
            _SelectedPreviewImage = null;
            if (FilePath != null)
            {
                ExtentionSupport support = new ExtentionSupport();

                var result = await Task<string[]>.Factory.StartNew(() =>
                {
                    return support.GetSupportedFiles(Directory.GetParent(FilePath).FullName, App.SupportExtentions);
                });

                support = null;

                _folderPreview.PathCollection = new ObservableCollection<string>(result);

                //_folderPreview.UpdateFolderContent(Directory.GetParent(FilePath).FullName);

                //FolderContent = null;
                //FolderContent = new FolderWorker().UpLoadFolderContent(FilePath, _SupportExtentions);
                //foldercontent.Focus();
                // Highlight the showed image on the folder presenter panel
                //_SelectedPreviewImage = FolderContent.First<FolderContentInfo>(cont => cont.FilePath == FilePath);
                //OnPropertyChanged("SelectedPreviewImage");
            }
        }

        #endregion

        #region Properties

        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; UploadImage(value); }
        }

        public ContentControl FolderPresenter { get; set; }

        public ObservableCollection<FolderContentInfo> FolderContent
        {
            get { return _FolderContent; }
            set { _FolderContent = value; OnPropertyChanged(); }
        }

        public FolderContentInfo SelectedPreviewImage
        {
            get { return _SelectedPreviewImage; }
            set
            {
                _SelectedPreviewImage = value;
                if (value.FilePath != null)
                    FilePath = value.FilePath;
            }
        }

        public ScrollViewer ImgViewer { get; set; }

        public StackPanel MenuList { get; set; }

        public StackPanel ViewPortMenu { get; set; }

        public AppStateKeeper StateKeeper
        {
            get { return _StateKeeper; }
            set { _StateKeeper = value; }
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
            if (!StateKeeper.IsMenuOpened) StateKeeper.IsMenuOpened = true;
        }

        private void Closemenubtn_Click(object sender, RoutedEventArgs e)
        {
            if (StateKeeper.IsMenuOpened) StateKeeper.IsMenuOpened = false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
            base.OnClosed(e);
        }

        #endregion

        #region ApplicationCommand

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StateKeeper.IsMenuOpened = false;
            StateKeeper.FolderContentIsOld = true;

            // From UnFilemanager.dll
            OpenManager manager = new OpenManager(new OpenDialogWrapper(), 
                                                    UnFMFilters.SupportedExtentions, // From UnFilemanager.dll
                                                    App.SupportExtentions);
            var (dialogresult, filePath, filePaths) = manager.GetDialogData(new MistakeMessanger());

            if (dialogresult )
                FilePath = filePath;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StateKeeper.IsMenuOpened = false;

            SaveDialogWrapper saver = new SaveDialogWrapper();
            saver.SaveFile(_Image, FilePath);
            UploadImage(FilePath);
            saver = null;
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StateKeeper.IsMenuOpened = false;

            SaveDialogWrapper saver = new SaveDialogWrapper();
            saver.SaveFileWithDialog(_Image);
            throw new NotImplementedException();
            //UploadFolderContent();
            //saver = null;
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StateKeeper.IsMenuOpened = false;
            if (File.Exists(FilePath))
            {
                string extention = FilePath.Substring(FilePath.LastIndexOf('.'));
                string newfile = FilePath.Substring(0, FilePath.LastIndexOf('.'));
                if (!newfile.Contains("_копия"))
                    newfile = FilePath.Substring(0, FilePath.LastIndexOf('.')) + "_копия0";
                while (File.Exists(newfile + extention))
                {
                    var root = newfile.Substring(0, newfile.LastIndexOf('я') + 1);
                    var snum = newfile.Substring(newfile.LastIndexOf('я') + 1);
                    Int32.TryParse(snum, out int inum);
                    inum++;
                    newfile = root + inum.ToString();
                }

                newfile += extention;

                File.Copy(FilePath, newfile);
                if (File.Exists(newfile))
                {
                    _FilePath = newfile;
                    throw new NotImplementedException();

                   // UploadFolderContent();
                }
            }
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Image.Source != null;
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StateKeeper.IsMenuOpened = false;
            if (File.Exists(FilePath))
            {
                throw new NotImplementedException();
                //if (foldercontent.Items.Count > 0)
                //{
                //    FolderContentInfo item = SelectedPreviewImage;
                //    MessageBoxResult result = MessageBox.Show($" Файл\n{item.FileName}\nбудет удален с диска!", "Удаление файла", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

                //    if (result == MessageBoxResult.OK)
                //    {
                //        File.Delete(FilePath);
                //        if (foldercontent.SelectedIndex > 0)
                //            foldercontent.SelectedIndex = foldercontent.SelectedIndex - 1;
                //        FolderContent.Remove(item);
                //    }
                //}
            }
            else MessageBox.Show("Неверный путь к файлу.");
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = FilePath != null;
        }

        #endregion

        #region PropertyChanged interface

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
