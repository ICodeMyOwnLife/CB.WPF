using System;
using System.Globalization;
using System.Windows.Data;


namespace CB.WPF.MahAppsExtension
{
    [ValueConversion(typeof(Accent), typeof(bool))]
    public class ChosenAccentConverter: ChosenMahAppsThemeConverter
    {
        #region Override
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => StringComparer.InvariantCultureIgnoreCase.Equals((value as Accent)?.Name, MahAppsThemeManager.GetCurrentAccent());
        #endregion
    }
}