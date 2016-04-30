using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CB.WPF.Controls.DataControls
{
    [ValueConversion(typeof(IdModelButtons), typeof(GridLength))]
    public class IdModelButtonsToGridLengthConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var star = new GridLength(1, GridUnitType.Star);

            if (!(value is IdModelButtons) || !(parameter is IdModelButtons)) return star;

            IdModelButtons allButtons = (IdModelButtons)value, thisButtons = (IdModelButtons)parameter;

            return allButtons.HasFlag(thisButtons) ? star : new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }
}