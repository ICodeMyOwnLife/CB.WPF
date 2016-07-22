using System.Windows;


namespace CB.WPF.MahAppsFileExplorerControls
{
    public class MahAppsFileControl: MahAppsFileSystemEntryControl
    {
        #region  Constructors & Destructor
        static MahAppsFileControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MahAppsFileControl),
                new FrameworkPropertyMetadata(typeof(MahAppsFileControl)));
        }
        #endregion
    }
}