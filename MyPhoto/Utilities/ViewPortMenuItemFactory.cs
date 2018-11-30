using MVVM;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyPhoto.Utilities
{
    class ViewPortMenuItemFactory
    {
        public void CreateAllMenuItems(StackPanel panel, ImgPreviewTransformer transformer, Image image)
        {
            ICommand transformcmd = new DelegateCommand((obj) => transformer.ExecuteTransformWith(obj as string), (obj) => image.Source != null);

            foreach (var item in transformer.GetMethods())
            {
                panel.Children.Add(new Button
                {
                    Margin = new System.Windows.Thickness(0, 0, 5, 0),
                    Width = 32,
                    Height = 32,
                    Content = transformer.GetIconWithName(item),
                    Command = transformcmd,
                    CommandParameter = item
                });
            }
        }
    }
}
