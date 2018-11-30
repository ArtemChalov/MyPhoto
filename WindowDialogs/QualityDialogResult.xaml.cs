using System.Windows;

namespace Dialogs.Windows
{
    /// <summary>
    /// Логика взаимодействия для QualityDialogResult.xaml
    /// </summary>
    public partial class QualityDialogResult : Window
    {
        public QualityDialogResult()
        {
            InitializeComponent();
        }

        public double Quality { get { return QSlider.Value; } }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
