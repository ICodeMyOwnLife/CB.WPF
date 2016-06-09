using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CB.WPF.MahAppsExtension
{
    public abstract class ChosenMahAppsThemeConverter: IValueConverter
    {
        #region Abstract
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        #endregion


        #region Methods
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;
        #endregion
    }
}