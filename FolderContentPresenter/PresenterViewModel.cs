using MVVM;
using System;
using System.Collections.ObjectModel;
using UnFilemanager.Filters;

namespace FolderContentPresenter
{
    public class PresenterViewModel : BaseViewModel
    {
        private ObservableCollection<string> _PathCollection;

        #region Static properties

        public static int ToolTipImageWidth { get; set; } = 256;

        #endregion

        #region Properties

        private string _SelectedPath;

        public string SelectedPath
        {
            get { return _SelectedPath; }
            set { _SelectedPath = value;
                OnPropertyChanged();
                if (!string.IsNullOrEmpty(value))
                    OnPathSelected?.Invoke(value);
            }
        }

        public Action<string> OnPathSelected;

        public ObservableCollection<string> PathCollection
        {
            get { return _PathCollection; }
            set { _PathCollection = value; OnPropertyChanged(); }
        }

        public string[] SupportExtentions { get; set; } = new string[] { ".*" };

        #endregion

        public void UpdateFolderContent(string directory)
        {
            PathCollection = null;

            ExtentionSupport support = new ExtentionSupport();

            var list = support.GetSupportedFiles(directory, SupportExtentions);

            PathCollection = new ObservableCollection<string>(list);

            support = null;
        }
    }
}
