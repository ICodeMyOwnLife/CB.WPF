using System;
using System.Windows;


namespace CB.WPF.MahAppsResources.Windows
{
    public partial class MahAppsToastWindow
    {
        #region  Constructors & Destructor
        public MahAppsToastWindow()
        {
            InitializeComponent();
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty CommandsProperty = DependencyProperty.Register(
            nameof(Commands), typeof(string[]), typeof(MahAppsToastWindow), new PropertyMetadata(default(string[])));

        public string[] Commands
        {
            get { return (string[])GetValue(CommandsProperty); }
            set { SetValue(CommandsProperty, value); }
        }

        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register(
            nameof(IconSource), typeof(string), typeof(MahAppsToastWindow), new PropertyMetadata(default(string)));

        public string IconSource
        {
            get { return (string)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }
        #endregion


        #region Events
        public event EventHandler<string> CommandClicked;
        #endregion


        #region Override
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            Left = SystemParameters.PrimaryScreenWidth - ActualWidth;
            Top = SystemParameters.WorkArea.Height - ActualHeight;
        }
        #endregion


        #region Event Handlers
        private void CommandButton_Click(object sender, RoutedEventArgs e)
        {
            var command = (e.OriginalSource as FrameworkElement)?.DataContext as string;
            if (command != null) OnCommandClicked(command);
        }
        #endregion


        #region Implementation
        protected virtual void OnCommandClicked(string command)
            => CommandClicked?.Invoke(this, command);
        #endregion
    }
}