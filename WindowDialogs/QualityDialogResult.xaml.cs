using System.Windows;

namespace Dialogs.Windows
{
    /// <summary>
    /// Логика взаимодействия для QualityDialogResult.xaml
    /// </summary>
    public partial class QualityDialogResult : Window
    {
        /// <summary>
        /// Initializes a new instance of the QualityDialogResult class.
        /// </summary>
        public QualityDialogResult()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The property that keep a slider value 
        /// </summary>
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
