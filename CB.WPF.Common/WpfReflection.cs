using System;
using System.Reflection;
using System.Windows;


namespace CB.WPF.Common
{
    public static class WpfReflection
    {
        #region Methods
        public static DependencyProperty GetDependencyProperty(this DependencyObject dependencyObject,
            string propertyName)
            => GetDependencyProperty(dependencyObject.GetType(), propertyName);

        public static DependencyProperty GetDependencyProperty(this Type elementType, string propertyName)
            => GetDependencyObjectField(elementType, propertyName)?.GetValue(null) as
               DependencyProperty;

        public static RoutedEvent GetRoutedEvent(this IReflect elementType, string eventName)
            => GetDependencyObjectField(elementType, eventName)?.GetValue(null) as RoutedEvent;

        public static RoutedEvent GetRoutedEvent(this UIElement element, string eventName)
            => GetRoutedEvent(element.GetType(), eventName);
        #endregion


        #region Implementation
        private static FieldInfo GetDependencyObjectField(IReflect elementType, string eventName)
            => elementType.GetField(eventName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
        #endregion
    }
}