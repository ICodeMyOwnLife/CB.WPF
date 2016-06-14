using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;


namespace CB.WPF.MahAppsExtension
{
    public class MetroWindowServices
    {
        #region Dependency Properties
        public static readonly DependencyProperty PressCloseToHideProperty = DependencyProperty.RegisterAttached(
            "PressCloseToHide", typeof(bool), typeof(MetroWindowServices),
            new PropertyMetadata(default(bool), OnPressCloseToHideChanged));

        [Category("MetroWindowServices")]
        [AttachedPropertyBrowsableForType(typeof(MetroWindow))]
        public static bool GetPressCloseToHide(DependencyObject d)
            => (bool)d.GetValue(PressCloseToHideProperty);

        [Category("MetroWindowServices")]
        [AttachedPropertyBrowsableForType(typeof(MetroWindow))]
        public static void SetPressCloseToHide(DependencyObject d, bool value)
            => d.SetValue(PressCloseToHideProperty, value);
        #endregion


        #region Event Handlers
        private static void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var metroWindow = (MetroWindow)sender;
            if (metroWindow.WindowButtonCommands != null)
                metroWindow.WindowButtonCommands.ClosingWindow += WindowButtonCommands_ClosingWindow;
        }

        private static void WindowButtonCommands_ClosingWindow(object sender, ClosingWindowEventHandlerArgs args)
        {
            args.Cancelled = true;
            ((WindowButtonCommands)sender).ParentWindow?.Hide();
        }
        #endregion


        #region Implementation
        private static void OnPressCloseToHideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var metroWindow = d as MetroWindow;
            if (metroWindow == null) return;

            var pressCloseToHide = (bool)e.NewValue;
            if (metroWindow.IsLoaded)
            {
                if (pressCloseToHide)
                {
                    metroWindow.WindowButtonCommands.ClosingWindow += WindowButtonCommands_ClosingWindow;
                }
                else
                {
                    metroWindow.WindowButtonCommands.ClosingWindow -= WindowButtonCommands_ClosingWindow;
                }
            }
            else
            {
                if (pressCloseToHide)
                {
                    metroWindow.Loaded += MetroWindow_Loaded;
                }
                else
                {
                    metroWindow.Loaded -= MetroWindow_Loaded;
                }
            }
        }
        #endregion
    }
}