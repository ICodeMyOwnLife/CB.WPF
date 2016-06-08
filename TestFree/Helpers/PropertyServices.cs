using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace TestFree.Helpers
{

    public class PropertyEditControl: ContentControl
    {
        public PropertyEditControl()
        {
            
        }
    }

    public class PropertyServices
    {
        #region Dependency Properties
        public static readonly DependencyProperty PropertyBindingProperty = DependencyProperty.RegisterAttached(
            "PropertyBinding", typeof(string), typeof(PropertyServices),
            new PropertyMetadata(default(string), OnPropertyBindingChanged));

        [Category("PropertyServices")]
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        public static string GetPropertyBinding(DependencyObject d)
            => (string)d.GetValue(PropertyBindingProperty);

        [Category("PropertyServices")]
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        public static void SetPropertyBinding(DependencyObject d, string value)
            => d.SetValue(PropertyBindingProperty, value);
        #endregion


        #region Implementation
        private static void OnPropertyBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as ContentControl;

            var dataContext = element?.DataContext;
            if (dataContext == null) return;

            element.Content = null;
            var propName = e.NewValue as string;
            if(string.IsNullOrEmpty(propName)) return;

            var propInfo = dataContext.GetType().GetProperty(propName);
            if (propInfo == null) return;


            var propertyType = propInfo.PropertyType;
            if (propertyType == typeof(string))
            {
                
            }
            else if (propertyType == typeof(int))
            {
                
            }
        }
        #endregion
    }
}