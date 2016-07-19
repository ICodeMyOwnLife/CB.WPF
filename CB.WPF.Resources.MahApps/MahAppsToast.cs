using System;
using System.Windows;
using System.Windows.Threading;
using CB.WPF.MahAppsResources.Windows;


namespace CB.WPF.MahAppsResources
{
    public static class MahAppsToast
    {
        #region Fields
        private static readonly DispatcherTimer _timer = new DispatcherTimer();
        private static MahAppsToastWindow _window;
        #endregion


        #region Events
        public static event EventHandler<string> CommandClicked;
        #endregion


        #region Methods
        public static void Show(object content, string iconSource, TimeSpan? duration = null, params string[] commands)
        {
            if (_window == null)
            {
                _window = new MahAppsToastWindow();
                _window.IsVisibleChanged += Window_IsVisibleChanged;
                _window.CommandClicked += (sender, command) => OnCommandClicked(command);
            }

            _window.Content = content;
            _window.IconSource = iconSource;
            _window.Commands = commands;
            _window.Show();

            if (duration == null) return;

            _timer.Interval = duration.Value;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        #endregion


        #region Event Handlers
        private static void Timer_Tick(object sender, EventArgs e)
        {
            _window.Hide();
        }

        private static void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(!(bool)e.NewValue) _timer.Stop();
        }
        #endregion


        #region Implementation
        private static void OnCommandClicked(string command)
        {
            CommandClicked?.Invoke(null, command);
        }
        #endregion
    }
}