using System.Windows;
using UnFilemanager.Interfaces;

namespace MyPhoto.Adapters
{
    class WrongMessangerAdapter : IUMessanger
    {
        public void ShowMessege(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Asterisk);

        }
    }
}
