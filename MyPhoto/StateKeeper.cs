using MVVM;

namespace MyPhoto
{
    public class AppStateKeeper : BaseViewModel
    {
        static bool _IsMenuOpened = false;

        public bool IsMenuOpened
        {
            get { return _IsMenuOpened; }
            set { _IsMenuOpened = value; OnPropertyChanged(); }
        }

        public bool FolderContentIsOld { get; set; } = true;
    }
}
