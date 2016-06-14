using System;
using System.Globalization;


namespace CB.WPF.MahAppsExtension
{
    public class ChosenAppThemeConverter: ChosenMahAppsThemeConverter
    {
        #region Override
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => StringComparer.InvariantCultureIgnoreCase.Equals((value as AppTheme)?.Name,
                MahAppsThemeManager.GetCurrentAppTheme());
        #endregion
    }
}