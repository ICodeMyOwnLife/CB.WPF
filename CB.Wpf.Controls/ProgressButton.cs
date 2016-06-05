using System.Windows;
using System.Windows.Controls;
using CB.Model.Common;


namespace CB.Wpf.Controls
{
    public class ProgressButton: Button
    {
        #region  Constructors & Destructor
        static ProgressButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressButton),
                new FrameworkPropertyMetadata(typeof(ProgressButton)));
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
            nameof(Progress), typeof(Progress), typeof(ProgressButton),
            new PropertyMetadata(Progress.NotRunningProgress));

        public Progress Progress
        {
            get { return (Progress)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            nameof(ProgressBarStyle), typeof(Style), typeof(ProgressButton), new PropertyMetadata(default(Style)));

        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }
        #endregion
    }
}