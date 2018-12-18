using MVVM;
using System.Collections.ObjectModel;

namespace FolderContentPresenter
{
    public class PresenterViewModel : BaseViewModel
    {
        private ObservableCollection<string> _PathCollection;

        public delegate void FolderPresenterEventHandler(object sender, FolderPresenterEventArgs e);

        #region Properties

        private string _SelectedPath;

        public string SelectedPath
        {
            get { return _SelectedPath; }
            set { _SelectedPath = value;
                OnPropertyChanged();
                if (!string.IsNullOrEmpty(value))
                    OnPathSelected?.Invoke(this, new FolderPresenterEventArgs(value));
            }
        }

        public ObservableCollection<string> PathCollection
        {
            get { return _PathCollection; }
            set { _PathCollection = value; OnPropertyChanged(); }
        }

        public string[] SupportExtentions { get; set; } = new string[] { ".*" };

        #endregion

        #region Events

        public event FolderPresenterEventHandler OnPathSelected;

        #endregion
    }
}
