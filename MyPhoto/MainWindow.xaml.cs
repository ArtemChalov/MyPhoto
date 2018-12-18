using FolderContentPresenter;
using MyPhoto.Services;
using MyPhoto.Utilities;
using MyPhoto.Wrappers;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UnFilemanager;

namespace MyPhoto
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Image MainImage;
        private string _FilePath;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ImagePresenterInit();
            MenuInit();
            ServicesInit();
            ViewPortMenuInit();
        }

        #region Init methods

        private void ServicesInit()
        {
            FolderPresenter = new PresenterView();
            AppServices._folderPreview = FolderPresenter.DataContext as PresenterViewModel;
            AppServices._folderPreview.SupportExtentions = App.SupportExtentions;

            AppServices._stateKeeper = new AppStateKeeper();

            AppServices._ImageViewTransformer = new ImgPreviewTransformer(MainImage, 0, 0);
        }

        private void ImagePresenterInit()
        {
            MainImage = new Image();
            ImgViewer = new ScrollViewer
            {
                Padding = new Thickness(2),
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = MainImage
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

            itemVPFactory.CreateAllMenuItems(ViewPortMenu,  AppServices._ImageViewTransformer, MainImage);

            itemVPFactory = null;
        }

        #endregion

        #region Properties

        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value;
                AppServices.UpdateImage(MainImage, value); }
        }

        public ContentControl FolderPresenter { get; set; }

        public ScrollViewer ImgViewer { get; set; }

        public StackPanel MenuList { get; set; }

        public StackPanel ViewPortMenu { get; set; }

        public AppStateKeeper StateKeeper
        {
            get { return AppServices._stateKeeper; }
            set { AppServices._stateKeeper = value; }
        }

        #endregion

        #region Events

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainImage.Width != double.NaN)
                MainImage.Width = double.NaN;
            if (MainImage.Height != double.NaN)
                MainImage.Height = double.NaN;
            if (MainImage.Source != null)
                AppServices._ImageViewTransformer?.ExecuteTransformWith("FitToParent");
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
            saver.SaveFile(MainImage, FilePath);
            AppServices.UpdateImage(MainImage, FilePath);
            saver = null;
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StateKeeper.IsMenuOpened = false;

            SaveDialogWrapper saver = new SaveDialogWrapper();
            saver.SaveFileWithDialog(MainImage);
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
            e.CanExecute = MainImage.Source != null;
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
