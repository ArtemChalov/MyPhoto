using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyPhoto
{
    /// <summary>
    /// Логика взаимодействия для EdgeMenu.xaml
    /// </summary>
    public partial class EdgeMenu : UserControl, INotifyPropertyChanged
    {
        private bool _IsMenuOpened = true;

        public EdgeMenu()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public bool IsMenuOpened
        {
            get { return _IsMenuOpened; }
            set { _IsMenuOpened = value; OnPropertyChanged(); }
        }

        public void CreateMenuItem(object icon, string header, ICommand command)
        {
            DockPanel dock = new DockPanel()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                LastChildFill = true,
            };

            dock.Children.Add(new ContentControl()
            {
                Content = icon,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Segoe MDL2 Assets"),
                FontSize = 16
            });

            dock.Children.Add(new TextBlock
            {
                Text = header,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontSize = 16,
                Margin = new System.Windows.Thickness(5, 0, 0, 0)
            });

            Button button = new Button()
            {
                Content = dock,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(5, 0, 5, 0),
                Padding = new System.Windows.Thickness(5)
            };

            button.Command = command;

            menuList.Children.Add(button);
        }

        public void CreateHSeparator(Color color)
        {
            menuList.Children.Add(new Border()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(color),
                Margin = new System.Windows.Thickness(5, 3, 5, 1),
                Height = 1
            });
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            IsMenuOpened = true;
        }

        private void Closemenubtn_Click(object sender, RoutedEventArgs e)
        {
            IsMenuOpened = false;
        }

        #region PropertyChanged interface

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            IsMenuOpened = false;
        }
    }
}
