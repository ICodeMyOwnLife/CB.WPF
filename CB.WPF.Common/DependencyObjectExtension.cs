using System;
using System.Windows;
using System.Windows.Media;


namespace CB.WPF.Common
{
    public static class DependencyObjectExtension
    {
        #region Dependency Properties
        public static T GetChild<T>(this DependencyObject obj) where T: DependencyObject
        {
            var t = obj as T;
            if (t != null) return t;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var result = GetChild<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        public static DependencyObject GetChild(this DependencyObject obj, Type childType)
        {
            var t = obj.GetType();
            if (t == childType || t.IsSubclassOf(childType)) return obj;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var result = GetChild(child, childType);
                if (result != null) return result;
            }

            return null;
        }

        public static Window GetWindow(this DependencyObject obj)
            => obj as Window ?? Window.GetWindow(obj);
        #endregion
    }
}