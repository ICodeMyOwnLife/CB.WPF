using System.Windows;
using System.Windows.Media;


namespace CB.WPF.Resources.MahApps
{
    public class MahAppsButtonIcon: DependencyObject
    {
        #region Dependency Properties
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            nameof(Height), typeof(double), typeof(MahAppsButtonIcon), new PropertyMetadata(default(double)));

        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public static readonly DependencyProperty VisualProperty = DependencyProperty.Register(
            nameof(Visual), typeof(Visual), typeof(MahAppsButtonIcon), new PropertyMetadata(default(Visual)));

        public Visual Visual
        {
            get { return (Visual)GetValue(VisualProperty); }
            set { SetValue(VisualProperty, value); }
        }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            nameof(Width), typeof(double), typeof(MahAppsButtonIcon), new PropertyMetadata(default(double)));

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        #endregion
    }
}