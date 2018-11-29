using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyPhoto.Utilities
{
    class MenuItemFactory
    {
        public Button CreateMenuItem(object icon, string header, ICommand command)
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

            return button;
        }
    }
}
