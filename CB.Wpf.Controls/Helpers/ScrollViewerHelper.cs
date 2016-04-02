using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CB.Wpf.Controls.Helpers
{
    public class ScrollViewerHelper
    {
        #region Methods
        public static ScrollViewer GetScrollViewer(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return GetScrollViewerImpl(dependencyObject) as ScrollViewer;
        }
        #endregion


        #region Implementation
        private static DependencyObject GetScrollViewerImpl(DependencyObject dependencyObject)
        {
            if (dependencyObject is ScrollViewer)
            {
                return dependencyObject;
            }

            var childCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for (var i = 0; i < childCount; ++i)
            {
                var result = GetScrollViewerImpl(VisualTreeHelper.GetChild(dependencyObject, i));
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
        #endregion
    }
}