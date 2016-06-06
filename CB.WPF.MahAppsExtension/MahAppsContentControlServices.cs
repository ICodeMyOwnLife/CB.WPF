using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CB.WPF.MahAppsExtension
{
    public static class MahAppsContentControlServices
    {
        #region Dependency Properties
        public static readonly DependencyProperty IconVisualProperty = DependencyProperty.RegisterAttached(
            "IconVisual", typeof(Visual), typeof(MahAppsContentControlServices), new PropertyMetadata(default(Visual)));

        [Category("MahAppsContentControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        public static Visual GetIconVisual(DependencyObject d)
            => (Visual)d.GetValue(IconVisualProperty);

        [Category("MahAppsContentControlServices")]
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        public static void SetIconVisual(DependencyObject d, Visual value)
            => d.SetValue(IconVisualProperty, value);
        #endregion
    }
}