using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Dialogs.Windows
{
    /// <summary>
    /// Логика взаимодействия для ResizeDialog.xaml
    /// </summary>
    public partial class ResizeDialogResult: Window, INotifyPropertyChanged
    {
        private int _PixelWidth;
        private int _PixelHeight;
        private bool _KeepProportions;
        private float ImageProportion;
        private int _ErrorCount = 0;

        private IReadOnlyDictionary<int, string> dict;

        /// <summary>
        /// Initializes a new instance of the ResizeDialog class.
        /// </summary>
        /// <param name="oldPixelWidth">Original width of the image.</param>
        /// <param name="oldPixelHeight">Original height of the image.</param>
        /// <param name="descripdict">The dictionary where keys are Interpolation enums 
        /// and values are desired descriptions of an interpolation method.</param>
        /// <param name="defselect">The default interpolation method</param>
        public ResizeDialogResult(int oldPixelWidth, int oldPixelHeight, IReadOnlyDictionary<int, string> descripdict, int defselect = 0)
        {
            InitializeComponent();
            dict = descripdict;
            KeepProportions = false;
            _PixelWidth = oldPixelWidth;
            _PixelHeight = oldPixelHeight;
            ImageProportion = (float)PixelWidth / (float)PixelHeight;
            methodCombo.ItemsSource = dict.Select(d => d.Value).ToList();
            methodCombo.SelectedItem = dict.FirstOrDefault(d => d.Key == defselect).Value;
            _KeepProportions = true;
            OnPropertyChanged("KeepProportions");
        }

        /// <summary>
        /// Desired new width
        /// </summary>
        public int PixelWidth
        {
            get { return _PixelWidth; }
            set
            {
                _PixelWidth = value;
                if (KeepProportions) {
                    _PixelHeight = (int)((float)value / ImageProportion + 0.5);
                    OnPropertyChanged("PixelHeight");
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Desired new height
        /// </summary>
        public int PixelHeight
        {
            get { return _PixelHeight; }
            set
            {
                _PixelHeight = value;
                if (KeepProportions) {
                    _PixelWidth = (int)((float)value * ImageProportion + 0.5);
                    OnPropertyChanged("PixelWidth");
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The Flag that save an image proportions if has value true
        /// </summary>
        public bool KeepProportions
        {
            get { return _KeepProportions; }
            set
            {
                _KeepProportions = value;
                if (value == true) {
                    if (PixelHeight > PixelWidth) {
                        _PixelWidth = (int)((float)PixelHeight * ImageProportion + 0.5);
                        OnPropertyChanged("PixelWidth");
                    }
                    else {
                        _PixelHeight = (int)((float)PixelWidth / ImageProportion + 0.5);
                        OnPropertyChanged("PixelHeight");
                    }
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Access to the selected interpolation method
        /// </summary>
        public int SelectedMethod { get; private set; }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            bool parseRes = false;
            int height = 0;
            parseRes = Int32.TryParse(wTextBox.Text, out int width);
            if(parseRes)
                parseRes = Int32.TryParse(hTextBox.Text, out height);
            PixelWidth = width;
            PixelHeight = height;
            SelectedMethod = dict.FirstOrDefault(k => k.Value == (string)methodCombo.SelectedItem).Key;

            this.DialogResult = parseRes;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Rise the property changed event
        /// </summary>
        /// <param name="propertyName">The optional propertyName parameter 
        /// causes the property name of the caller to be substituted as an argument.</param>
        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TextBox_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            switch (e.Action) {
                case ValidationErrorEventAction.Added:
                    _ErrorCount++;
                    break;
                case ValidationErrorEventAction.Removed:
                    _ErrorCount--;
                    break;
                default: break;
            }

            if (_ErrorCount == 0) ApplyBtn.IsEnabled = true;
            else ApplyBtn.IsEnabled = false;
        }
    }
}
