using System.Windows;


namespace CB.WPF.MahAppsFileExplorerControls
{
    public class MahAppsFolderControl: MahAppsFileSystemEntryControl
    {
        #region  Constructors & Destructor
        static MahAppsFolderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MahAppsFolderControl),
                new FrameworkPropertyMetadata(typeof(MahAppsFolderControl)));
        }
        #endregion
    }
}