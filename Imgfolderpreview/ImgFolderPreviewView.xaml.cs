using System.Windows.Controls;

namespace Imgfolderpreview
{
    /// <summary>
    /// Логика взаимодействия для ImgFolderPreviewView.xaml
    /// </summary>
    public partial class ImgFolderPreviewView: UserControl
    {
        public ImgFolderPreviewView()
        {
            InitializeComponent();
            this.DataContext = new ImgFolderPreviewViewModel();
        }
    }
}
