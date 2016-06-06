using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace CB.WPF.MahAppsExtension
{
    public static class MahAppsBackgroundServices
    {
        #region Dependency Properties
        public static readonly DependencyProperty BackgroundPathProperty = DependencyProperty.RegisterAttached(
            "BackgroundPath", typeof(string), typeof(MahAppsBackgroundServices),
            new PropertyMetadata(default(string), OnBackgroundPathChanged));

        [Category("MahAppsBackgroundServices")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static string GetBackgroundPath(DependencyObject d)
            => (string)d.GetValue(BackgroundPathProperty);

        [Category("MahAppsBackgroundServices")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetBackgroundPath(DependencyObject d, string value)
            => d.SetValue(BackgroundPathProperty, value);

        public static readonly DependencyProperty BackgroundPropertyProperty = DependencyProperty.RegisterAttached(
            "BackgroundProperty", typeof(DependencyProperty), typeof(MahAppsBackgroundServices),
            new PropertyMetadata(Control.BackgroundProperty, OnBackgroundPropertyChanged));

        [Category("MahAppsBackgroundServices")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static DependencyProperty GetBackgroundProperty(DependencyObject d)
            => (DependencyProperty)d.GetValue(BackgroundPropertyProperty);

        [Category("MahAppsBackgroundServices")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetBackgroundProperty(DependencyObject d, DependencyProperty value)
            => d.SetValue(BackgroundPropertyProperty, value);
        #endregion


        #region Implementation
        private static void OnBackgroundPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => SetBackground(d as FrameworkElement, GetBackgroundProperty(d), e.NewValue as string);

        private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => SetBackground(d as FrameworkElement, e.NewValue as DependencyProperty, GetBackgroundPath(d));

        private static void SetBackground(FrameworkElement element, DependencyProperty backgroundProperty,
            string backgroundPath)
        {
            if (element == null || backgroundProperty == null) return;

            if (string.IsNullOrEmpty(backgroundPath))
            {
                element.SetResourceReference(backgroundProperty, "WhiteColorBrush");
            }
            else
            {
                var imageBrush = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(backgroundPath, UriKind.RelativeOrAbsolute)),
                    Stretch = Stretch.UniformToFill
                };
                element.SetValue(backgroundProperty, imageBrush);
            }
        }
        #endregion
    }
}