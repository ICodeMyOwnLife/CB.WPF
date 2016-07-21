using System.Windows;
using CB.IO.Common;
using CB.Xaml.Common;


namespace CB.WPF.MahAppsFileExplorer.Helpers
{
    public class ListEntryDataTemplateSelector: ExtendedDataTemplateSelector
    {
        #region Override
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var directory = item as IDirectory;
            if (directory != null) return FindDataTemplate("ListDirectoryTemplate", container);
            var file = item as IFile;
            if (file != null) return FindDataTemplate("ListFileTemplate", container);
            return null;
        }
        #endregion
    }
}