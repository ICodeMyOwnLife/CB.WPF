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


        #region Dependency Properties
        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(
            nameof(Buttons), typeof(IdModelButtons), typeof(IdModelControl),
            new PropertyMetadata(IdModelButtons.Delete | IdModelButtons.Load | IdModelButtons.Add | IdModelButtons.Save));

        public IdModelButtons Buttons
        {
            get { return (IdModelButtons)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }
        #endregion
    }
}

// TODO: Show/Hide Button use code-behind, disable button if hidden
// TODO: Use Icon on Buttons