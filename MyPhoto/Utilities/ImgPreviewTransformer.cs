using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace MyPhoto.Utilities
{
    /// <summary>
    /// The class that can transform control inherited from FrameworkElement passed in constructor
    /// with method is ExecuteTrasforWith(string transmethodname);
    /// The full supported method's list available with method GetMethods.
    /// </summary>
    class ImgPreviewTransformer
    {
        private FrameworkElement _imagecontrol;
        private readonly double _Width;
        private readonly double _Height;
        private bool _IsCenterFitted = true;
        private Dictionary<string, Action> _TransformationCollection;

        /// <summary>
        /// Create a new instance of the ControlTransformer class.
        /// </summary>
        /// <param name="element">The control inherited from FrameworkElement to transform with specified method.</param>
        /// <param name="width">Original width of the control (to an Image is PixelWidth).</param>
        /// <param name="height">Original height of the control (to an Image is PixelHeight)</param>
        public ImgPreviewTransformer(FrameworkElement element, double width, double height)
        {
            _imagecontrol = element;
            _Width = width;
            _Height = height;
            _TransformationCollection = new Dictionary<string, Action>{
                { "Zoom+", ZoomAdd },
                { "Zoom-", ZoomSub },
                { "OriginalSize", OriginalSize },
                { "FitToParent", FitToParent },
                { "Rotate", Rotate }
            };
        }

        /// <summary>
        /// Get list of the available transformation method's names.
        /// </summary>
        /// <returns>List of the available transformation method's names.</returns>
        public List<string> GetMethods() => _TransformationCollection.Select(dict => dict.Key).ToList();

        /// <summary>
        /// Apply a transformation with specified method to a corresponding control.
        /// </summary>
        /// <param name="transmethodname">Predefined in this class method name.</param>
        public void ExecuteTrasforWith(string transmethodname)
        {
            if (String.IsNullOrEmpty(transmethodname)) return;
            if (_TransformationCollection.ContainsKey(transmethodname))
                _TransformationCollection[transmethodname]();
        }

        private void ZoomAdd()
        {
            if (_imagecontrol.Width > 0)
                _imagecontrol.Width = _imagecontrol.Width * 1.1;
            else {
                _imagecontrol.Width = _imagecontrol.ActualWidth;
                _imagecontrol.Width = _imagecontrol.Width * 1.1;
            }
            if (_imagecontrol.Height > 0)
                _imagecontrol.Height = _imagecontrol.Height * 1.1;
            else {
                _imagecontrol.Height = _imagecontrol.ActualHeight;
                _imagecontrol.Height = _imagecontrol.Width * 1.1;
            }

            _IsCenterFitted = false;
        }

        private void ZoomSub()
        {
            if (_imagecontrol.Width > 0)
                _imagecontrol.Width = _imagecontrol.Width / 1.1;
            else {
                _imagecontrol.Width = _imagecontrol.ActualWidth;
                _imagecontrol.Width = _imagecontrol.Width / 1.1;
            }
            if (_imagecontrol.Height > 0)
                _imagecontrol.Height = _imagecontrol.Height / 1.1;
            else {
                _imagecontrol.Height = _imagecontrol.ActualHeight;
                _imagecontrol.Height = _imagecontrol.Width / 1.1;
            }
           
            _IsCenterFitted = false;
        }

        private void OriginalSize()
        {
            _imagecontrol.Width = _Width;
            _imagecontrol.Height = _Height;
            _IsCenterFitted = false;
        }

        private void FitToParent()
        {
            if (_imagecontrol.Parent is FrameworkElement parent) {
                var transform = _imagecontrol.LayoutTransform as RotateTransform;
                var curAngle = transform?.Angle;
                Thickness thickness = _imagecontrol.Margin;
                thickness.Left = 0;
                thickness.Top = 0;
                thickness.Right = 0;
                thickness.Bottom = 0;
                _imagecontrol.Margin = thickness;
                if ((curAngle / 90) % 2 > 0)
                    FitRotated(parent);
                else
                    FitNotRotated(parent);
            }
            _IsCenterFitted = true;
        }

        private void FitNotRotated(FrameworkElement parent)
        {
            double width = parent.ActualWidth;
            double aspectRatio = _Width / _Height;

            double height = width / aspectRatio;
            if (height >= parent.ActualHeight) {
                height = parent.ActualHeight;
                if (height < 0) height = 0;
                width = height * aspectRatio;
            }

            _imagecontrol.Width = width;
            _imagecontrol.Height = height;
        }

        private void FitRotated(FrameworkElement parent)
        {
            double width = parent.ActualHeight;
            double aspectRatio = _Width / _Height;

            double height = width / aspectRatio;
            if (height >= parent.ActualWidth) {
                height = parent.ActualWidth;
                if (height < 0) height = 0;
                width = height * aspectRatio;
            }

            _imagecontrol.Width = width;
            _imagecontrol.Height = height;
        }

        private void Rotate()
        {
            var transform = _imagecontrol.LayoutTransform as RotateTransform;
            var curAngle = transform?.Angle ?? 0;

            //Swipe Width and Height
            var tempSize = _imagecontrol.Width;
            _imagecontrol.Width = _imagecontrol.Height;
            _imagecontrol.Height = tempSize;

            // Rotate around center of rectangle
            _imagecontrol.LayoutTransform = new RotateTransform(curAngle + 90, 0.5, 0.5);

            if (_IsCenterFitted)
                FitToParent();
        }
    }
}
