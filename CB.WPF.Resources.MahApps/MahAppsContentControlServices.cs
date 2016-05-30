using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;


namespace CB.WPF.Resources.MahApps
{
    public class MahAppsContentControlServices
    {
        #region Dependency Properties
        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached(
            "Icon", typeof(MahAppsButtonIcon), typeof(MahAppsContentControlServices),
            new PropertyMetadata(default(MahAppsButtonIcon), OnIconChanged));

        [Category("MahAppsContentControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        public static MahAppsButtonIcon GetIcon(DependencyObject d)
            => (MahAppsButtonIcon)d.GetValue(IconProperty);

        [Category("MahAppsContentControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        public static void SetIcon(DependencyObject d, MahAppsButtonIcon value)
            => d.SetValue(IconProperty, value);
        #endregion


        #region Implementation
        private static void AddIcon(ContentControl button, MahAppsButtonIcon icon)
            => button.Content = CreateIcon(icon);

        private static object CreateIcon(MahAppsButtonIcon icon)
        {
            var rec = new Rectangle
            {
                Width = icon.Width,
                Height = icon.Height,
                OpacityMask = new VisualBrush { Stretch = Stretch.Fill, Visual = icon.Visual }
            };
            var fillBinding = new Binding("Foreground") { Source = rec.Parent };
            rec.SetBinding(Shape.FillProperty, fillBinding);
            return rec;
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as Button;
            if (element == null) return;

            var icon = e.NewValue as MahAppsButtonIcon;
            if (icon == null) RemoveIcon(element);
            else AddIcon(element, icon);
        }

        private static void RemoveIcon(ContentControl element)
            => element.Content = null;
        #endregion
    }
}