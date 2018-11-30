using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyPhoto.Utilities
{
    /// <summary>
    /// The class that can transform control preview if it's putted into ScrollViewver
    /// with method is ExecuteTrasforWith(string transmethodname).
    /// The full supported method's list available with method GetMethods.
    /// Also this one class make control movable with pressed mouse weel.
    /// </summary>
    class ImgPreviewTransformer
    {
        private FrameworkElement _control;
        private double _Width;
        private double _Height;
        private bool _IsCenterFitted = true;
        private ScrollViewer _Scroll;
        private readonly Dictionary<string, Action> _TransformationCollection;
        private readonly Dictionary<string, object> _TransformationIconCollection;

        private double _Original_H_Offset = 1;
        private double _Original_V_Offset = 1;
        private Point _OriginalPoint;

        /// <summary>
        /// Create a new instance of the ImgPreviewTransformer class.
        /// </summary>
        /// <param name="element">The control is derived from FrameworkElement.</param>
        /// <param name="width">Original width of the element in pixels.</param>
        /// <param name="height">Original height of the element in pixels)</param>
        public ImgPreviewTransformer(FrameworkElement element, double width, double height)
        {
            if (element.Parent is ScrollViewer parent)
            {
                _control = element;
                _control.MouseDown += Element_MouseDown; ;
                _control.MouseUp += Element_MouseUp;
                _control.MouseMove += Element_MouseMove;
                _control.MouseWheel += _control_MouseWheel;
                _Scroll = parent;
                _Width = width;
                _Height = height;
                _TransformationCollection = new Dictionary<string, Action>{
                    { "Zoom-", ZoomSub },
                    { "Zoom+", ZoomAdd },
                    { "OriginalSize", OriginalSize },
                    { "FitToParent", FitToParent },
                    { "Rotate", Rotate }
                };
                _TransformationIconCollection = new Dictionary<string, object>
                {
                    { "Zoom-", "\uE1A4" },
                    { "Zoom+", "\uE12E" },
                    { "OriginalSize", "\uE1A3" },
                    { "FitToParent", "\uE91B" },
                    { "Rotate", "\uE14A" }
                };
            }
        }

        public void SetOriginalDimentions(double width, double height)
        {
            _Width = width;
            _Height = height;
        }

        private void _control_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                if(e.Delta > 0)
                    ZoomAdd();
                else
                    ZoomSub();
            }
        }

        private void Element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _control.ReleaseMouseCapture();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                _control.CaptureMouse();
                _OriginalPoint = e.GetPosition(_Scroll);
                _Original_V_Offset = _Scroll.VerticalOffset;
                _Original_H_Offset = _Scroll.HorizontalOffset;
                Mouse.OverrideCursor = Cursors.ScrollAll;
            }
        }

        private void Element_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed && _control.IsMouseCaptured)
            {
                var new_V_Offset = _Original_V_Offset + (_OriginalPoint.Y - e.GetPosition(_Scroll).Y);
                var new_H_Offset = _Original_H_Offset + (_OriginalPoint.X - e.GetPosition(_Scroll).X);
                _Scroll.ScrollToVerticalOffset(new_V_Offset);
                _Scroll.ScrollToHorizontalOffset(new_H_Offset);
            }
        }

        /// <summary>
        /// Get list of the available transformation method's names.
        /// </summary>
        /// <returns>List of the available transformation method's names.</returns>
        public List<string> GetMethods() => _TransformationCollection?.Select(dict => dict.Key).ToList();

        public object GetIconWithName(string methodname) => _TransformationIconCollection?[methodname]?? null;

        /// <summary>
        /// Apply a transformation with specified method's name.
        /// </summary>
        /// <param name="transmethodname">Predefined in this class method's name.</param>
        public void ExecuteTransformWith(string transmethodname)
        {
            if (String.IsNullOrEmpty(transmethodname)) return;

            if (_TransformationCollection?.ContainsKey(transmethodname) ?? false)
                _TransformationCollection?[transmethodname]();
        }

        private void ZoomAdd()
        {
            if (_control.Width > 0)
            {
                _control.Width = _control.Width * 1.1;
            }
            else
            {
                _control.Width = _control.ActualWidth;
                _control.Width = _control.Width * 1.1;
            }
            if (_control.Height > 0)
                _control.Height = _control.Height * 1.1;
            else
            {
                _control.Height = _control.ActualHeight;
                _control.Height = _control.Width * 1.1;
            }

            _Scroll.ScrollToHorizontalOffset(_Scroll.HorizontalOffset + (_Scroll.ScrollableWidth * 0.1));
            _Scroll.ScrollToVerticalOffset(_Scroll.VerticalOffset + (_Scroll.ScrollableHeight * 0.1));

            _IsCenterFitted = false;
        }

        private void ZoomSub()
        {
            if (_control.Width > 0)
                _control.Width = _control.Width / 1.1;
            else {
                _control.Width = _control.ActualWidth;
                _control.Width = _control.Width / 1.1;
            }
            if (_control.Height > 0)
                _control.Height = _control.Height / 1.1;
            else {
                _control.Height = _control.ActualHeight;
                _control.Height = _control.Width / 1.1;
            }

            _Scroll.ScrollToHorizontalOffset(_Scroll.HorizontalOffset - (_Scroll.ScrollableWidth * 0.1));
            _Scroll.ScrollToVerticalOffset(_Scroll.VerticalOffset - (_Scroll.ScrollableHeight * 0.1));

            _IsCenterFitted = false;
        }

        private void OriginalSize()
        {
            _control.Width = _Width;
            _control.Height = _Height;
            _IsCenterFitted = false;
        }

        private void FitToParent()
        {
            if (_control.Parent is ScrollViewer parent) {
                var transform = _control.LayoutTransform as RotateTransform;
                var curAngle = transform?.Angle;
                Thickness thickness = _control.Margin;
                thickness.Left = 0;
                thickness.Top = 0;
                thickness.Right = 0;
                thickness.Bottom = 0;
                _control.Margin = thickness;
                if ((curAngle / 90) % 2 > 0)
                    FitRotated(parent);
                else
                    FitNotRotated(parent);
            }
            _IsCenterFitted = true;
        }

        private void FitNotRotated(ScrollViewer parent)
        {
            double width = parent.ActualWidth - parent.Padding.Left - parent.Padding.Right;
            double aspectRatio = _Width / _Height;

            double height = width / aspectRatio;
            if (height >= (parent.ActualHeight - parent.Padding.Top - parent.Padding.Bottom)) {
                height = parent.ActualHeight - parent.Padding.Top - parent.Padding.Bottom;
                if (height < 0) height = 0;
                width = height * aspectRatio;
            }

            _control.Width = width;
            _control.Height = height;
        }

        private void FitRotated(ScrollViewer parent)
        {
            double width = parent.ActualHeight - parent.Padding.Top - parent.Padding.Bottom;
            double aspectRatio = _Width / _Height;

            double height = width / aspectRatio;
            if (height >= (parent.ActualWidth - parent.Padding.Left - parent.Padding.Right)) {
                height = parent.ActualWidth - parent.Padding.Left - parent.Padding.Right;
                if (height < 0) height = 0;
                width = height * aspectRatio;
            }

            _control.Width = width;
            _control.Height = height;
        }

        private void Rotate()
        {
            var transform = _control.LayoutTransform as RotateTransform;
            var curAngle = transform?.Angle ?? 0;

            //Swipe Width and Height
            var tempSize = _control.Width;
            _control.Width = _control.Height;
            _control.Height = tempSize;

            // Rotate around center of rectangle
            _control.LayoutTransform = new RotateTransform(curAngle + 90, 0.5, 0.5);

            if (_IsCenterFitted)
                FitToParent();
        }
    }
}
