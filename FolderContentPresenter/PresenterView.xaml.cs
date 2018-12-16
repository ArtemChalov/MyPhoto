using System.Windows.Controls;

namespace FolderContentPresenter
{
    /// <summary>
    /// Логика взаимодействия для PresenterView.xaml
    /// </summary>
    public partial class PresenterView : UserControl
    {
        public PresenterView()
        {
            InitializeComponent();
            this.DataContext = new PresenterViewModel();
        }
    }
}
