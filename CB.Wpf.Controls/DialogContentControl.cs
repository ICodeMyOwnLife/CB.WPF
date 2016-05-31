using System.Windows;
using System.Windows.Controls;


namespace CB.Wpf.Controls
{
    public class DialogContentControl: ContentControl
    {
        #region  Constructors & Destructor
        static DialogContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogContentControl),
                new FrameworkPropertyMetadata(typeof(DialogContentControl)));
        }
        #endregion
    }
}