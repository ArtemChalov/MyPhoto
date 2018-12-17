using FolderContentPresenter;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using UnFilemanager.Filters;

namespace MyPhoto.Services
{
    class FolderService
    {
        public async void UploadFolderContentAsync(AppStateKeeper stateKeeper, string filePath, PresenterViewModel folderPreview)
        {
            stateKeeper.FolderContentIsOld = false;

            if (filePath != null)
            {
                ExtentionSupport support = new ExtentionSupport();

                var result = await Task<string[]>.Factory.StartNew(() =>
                {
                    return support.GetSupportedFiles(Directory.GetParent(filePath).FullName, App.SupportExtentions);
                });

                support = null;

                folderPreview.PathCollection = new ObservableCollection<string>(result);
            }
        }
    }
}
