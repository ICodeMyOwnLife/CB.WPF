using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace TestMahAppsResources.Helpers
{
    [ValueConversion(typeof(string), typeof(Brush))]
    public class FilePathToBrushConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            return string.IsNullOrEmpty(path)
                       ? (Brush)Application.Current.FindResource("WhiteColorBrush")
                       : new ImageBrush
                       {
                           ImageSource = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)),
                           Stretch = Stretch.UniformToFill
                       };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;
        #endregion
    }
}