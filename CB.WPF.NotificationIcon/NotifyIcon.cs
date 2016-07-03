using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;


namespace CB.WPF.NotificationIcon
{
    public class NotifyIcon: TaskbarIcon, IShowBalloonTip
    {
        #region Dependency Properties
        public static readonly DependencyProperty CloseWhenParentWindowClosedProperty = DependencyProperty.Register(
            nameof(CloseWhenParentWindowClosed), typeof(bool), typeof(NotifyIcon),
            new PropertyMetadata(false, OnCloseWhenParentWindowClosedChanged));

        public bool CloseWhenParentWindowClosed
        {
            get { return (bool)GetValue(CloseWhenParentWindowClosedProperty); }
            set { SetValue(CloseWhenParentWindowClosedProperty, value); }
        }
        #endregion


        #region Methods
        public void ShowBalloonTip(string title, string message, BalloonIcon symbol, string soundSource = null)
        {
            if (!string.IsNullOrEmpty(soundSource)) SetSoundPlaying(soundSource);
            base.ShowBalloonTip(title, message, symbol);
        }

        public void ShowBalloonTip(string title, string message, Icon customIcon, bool largeIcon = false,
            string soundSource = null)
        {
            if (!string.IsNullOrEmpty(soundSource)) SetSoundPlaying(soundSource);
            base.ShowBalloonTip(title, message, customIcon, largeIcon);
        }
        #endregion


        #region Event Handlers
        private void ParentWindow_Closed(object sender, EventArgs e)
            => Visibility = Visibility.Hidden;
        #endregion


        #region Implementation
        private static MediaPlayer CreateMediaPlayer(string soundSource)
        {
            var mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += (sender, args) => mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Open(new Uri(soundSource, UriKind.RelativeOrAbsolute));
            return mediaPlayer;
        }

        private static void OnCloseWhenParentWindowClosedChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs args)
            => ((NotifyIcon)d).OnCloseWhenParentWindowClosedChanged((bool)args.OldValue, (bool)args.NewValue);

        // ReSharper disable once UnusedParameter.Local
        private void OnCloseWhenParentWindowClosedChanged(bool oldValue, bool newValue)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow == null) return;

            if (newValue) parentWindow.Closed += ParentWindow_Closed;
            else parentWindow.Closed -= ParentWindow_Closed;
        }

        private void SetSoundPlaying(string soundSource)
        {
            var mediaPlayer = CreateMediaPlayer(soundSource);

            RoutedEventHandler shownHandler = null;
            shownHandler = (sender, args) =>
            {
                mediaPlayer.Play();
                TrayBalloonTipShown -= shownHandler;
            };
            TrayBalloonTipShown += shownHandler;

            RoutedEventHandler closedHandler = null;
            closedHandler = (sender, args) =>
            {
                mediaPlayer.Close();
                TrayBalloonTipClicked -= closedHandler;
                TrayBalloonTipClosed -= closedHandler;
            };
            TrayBalloonTipClicked += closedHandler;
            TrayBalloonTipClosed += closedHandler;
        }
        #endregion
    }
}