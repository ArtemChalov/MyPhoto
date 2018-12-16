using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MVVM
{
    public abstract class BaseViewModel: DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
