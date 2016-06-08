using System.Windows;
using System.Windows.Controls;


namespace TestFree.Helpers
{
    public class PropertyEditControlTemplateSelector: DataTemplateSelector

    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var propType = GetPropertyType(item);
            if (propType == null) return null;

            var containerElement = (FrameworkElement)container;
            if (propType == typeof(string))
            {
                return containerElement.FindResource("StringEditTemplate") as DataTemplate;
            }
            if (propType == typeof(int))
            {
                return containerElement.FindResource("IntEditTemplate") as DataTemplate;
            }
            return null;
        }
    }
}