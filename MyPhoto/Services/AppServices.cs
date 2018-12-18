using FolderContentPresenter;
using MyPhoto.Utilities;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using UnFilemanager.Filters;
using WriteableBitmapEx;

namespace MyPhoto.Services
{
    class AppServices
    {
        public static AppStateKeeper _stateKeeper;
        public static PresenterViewModel _folderPreview;
        public static ImgPreviewTransformer _ImageViewTransformer;

        public static async void UpdateImage(Image image, string filePath)
        {
            image.Source = null;

            if (filePath == null) return;

            LoadPreview(image, filePath);

            // Create and show the full size image
            image.Source = await WriteableBitmapFactory.CreateFromFileAsync(filePath);
            _ImageViewTransformer.SetOriginalDimentions((image.Source as WriteableBitmap).PixelWidth, (image.Source as WriteableBitmap).PixelHeight);
            if (!String.IsNullOrEmpty(Properties.Settings.Default.DefaultPreview))
                _ImageViewTransformer.ExecuteTransformWith(Properties.Settings.Default.DefaultPreview);

            if (_stateKeeper.FolderContentIsOld)
                AppServices.UploadFolderContentAsync(filePath);
        }

        public static async void UploadFolderContentAsync(string filePath)
        {
            if (_stateKeeper == null || _folderPreview == null) throw new NullReferenceException("Method UploadFolderContentAsync.");

            _stateKeeper.FolderContentIsOld = false;

            if (filePath != null)
            {
                ExtentionSupport support = new ExtentionSupport();

                var result = await Task<string[]>.Factory.StartNew(() =>
                {
                    return support.GetSupportedFiles(Directory.GetParent(filePath).FullName, App.SupportExtentions);
                });

                support = null;

                _folderPreview.PathCollection = new ObservableCollection<string>(result);
            }
        }

        // Create and show a preview image
        // to get fast load
        private static void LoadPreview(Image image, string filePath)
        {
            var previewheight = (int)((image.Parent as FrameworkElement).ActualHeight - 3.5);
            image.Source = BitmapImageFactory.CreateThumbnailFromFile(filePath, previewheight, WriteableBitmapEx.DesiredSize.Height);
            _ImageViewTransformer.SetOriginalDimentions((image.Source as BitmapImage).PixelWidth, (image.Source as BitmapImage).PixelHeight);
            if (!String.IsNullOrEmpty(Properties.Settings.Default.DefaultPreview))
                _ImageViewTransformer.ExecuteTransformWith(Properties.Settings.Default.DefaultPreview);
        }
    }
}
