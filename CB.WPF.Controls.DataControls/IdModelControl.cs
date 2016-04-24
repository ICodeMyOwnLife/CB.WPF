using System.Windows;
using System.Windows.Controls;


namespace CB.WPF.Controls.DataControls
{
    public class IdModelControl: ContentControl
    {
        #region  Constructors & Destructor
        static IdModelControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IdModelControl),
                new FrameworkPropertyMetadata(typeof(IdModelControl)));
        }
        #endregion
    }
}