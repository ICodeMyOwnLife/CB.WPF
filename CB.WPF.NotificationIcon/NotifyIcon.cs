using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using CB.Prism.Interactivity;
using Hardcodet.Wpf.TaskbarNotification;
using BalloonIcon = CB.Prism.Interactivity.BalloonIcon;


namespace CB.WPF.NotificationIcon
{
    public class NotifyIcon: TaskbarIcon, IShowBalloonTip
    {
        #region Fields
        public static readonly DependencyProperty CloseWhenParentWindowClosedProperty = DependencyProperty.Register(
            nameof(CloseWhenParentWindowClosed), typeof(bool), typeof(NotifyIcon),
            new PropertyMetadata(false, OnCloseWhenParentWindowClosedChanged));

        private static MediaPlayer _mediaPlayer;
        private static Action _reshowAction;
        private bool _loop;
        private bool _looping;
        private string _soundSource;
        #endregion


        #region  Constructors & Destructor
        public NotifyIcon()
        {
            TrayBalloonTipClicked += NotifyIcon_TrayBalloonTipClicked;
            TrayBalloonTipClosed += NotifyIcon_TrayBalloonTipClosed;
            TrayBalloonTipShown += NotifyIcon_TrayBalloonTipShown;
        }
        #endregion


        #region Dependency Properties
        public bool CloseWhenParentWindowClosed
        {
            get { return (bool)GetValue(CloseWhenParentWindowClosedProperty); }
            set { SetValue(CloseWhenParentWindowClosedProperty, value); }
        }
        #endregion


        #region Events
        public event EventHandler BalloonTipClosed;
        #endregion


        #region Methods
        public void ShowBalloonTip(string title, string message, BalloonIcon symbol, string soundSource = null)
            => ShowBalloonTip(title, message, symbol, soundSource, false);

        /*{
            if (!string.IsNullOrEmpty(soundSource)) SetSoundPlaying(soundSource);
            _soundSource = soundSource;
            ShowBalloonTip(title, message, MapBalloonIcon(symbol));
        }*/

        public void ShowBalloonTip(string title, string message, Icon customIcon, bool largeIcon = false,
            string soundSource = null)
            => ShowBalloonTip(title, message, customIcon, largeIcon, soundSource, false);

        /*{
            if (!string.IsNullOrEmpty(soundSource)) SetSoundPlaying(soundSource);
            _soundSource = soundSource;
            base.ShowBalloonTip(title, message, customIcon, largeIcon);
        }*/

        public void ShowBalloonTip(string title, string message, BalloonIcon symbol, string soundSource, bool loop)
            => ShowBalloonTip(title, message, symbol, soundSource, loop, false);

        public void ShowBalloonTip(string title, string message, Icon customIcon, bool largeIcon, string soundSource,
            bool loop)
            => ShowBalloonTip(title, message, customIcon, largeIcon, soundSource, loop, false);

        public void ShowBalloonTip(string title, string message, BalloonIcon symbol, string soundSource, bool loop,
            bool looping)
        {
            _loop = loop;
            _looping = looping;
            _soundSource = soundSource;

            if (loop)
            {
                _reshowAction = () => ShowBalloonTip(title, message, symbol, soundSource, true, true);
            }

            base.ShowBalloonTip(title, message, MapBalloonIcon(symbol));
        }
        #endregion


        #region Event Handlers
        private void NotifyIcon_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
            => OnBalloonTipClosed();

        private void NotifyIcon_TrayBalloonTipClosed(object sender, RoutedEventArgs e)
        {
            if (_loop) _reshowAction();
            else OnBalloonTipClosed();
        }

        private void NotifyIcon_TrayBalloonTipShown(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_soundSource) || _looping) return;

            if (_mediaPlayer == null)
            {
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.MediaEnded += (s, args) => _mediaPlayer.Position = TimeSpan.Zero;
            }

            _mediaPlayer.Open(new Uri(_soundSource, UriKind.RelativeOrAbsolute));
            _mediaPlayer.Play();
        }

        private void ParentWindow_Closed(object sender, EventArgs e) => Visibility = Visibility.Hidden;
        #endregion


        #region Implementation
        /*private static void InitializeMediaPlayer(string soundSource)
        {
            if (_mediaPlayer == null)
            {
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.MediaEnded += (sender, args) => _mediaPlayer.Position = TimeSpan.Zero;
            }
            _mediaPlayer.Open(new Uri(soundSource, UriKind.RelativeOrAbsolute));
        }*/

        private Hardcodet.Wpf.TaskbarNotification.BalloonIcon MapBalloonIcon(BalloonIcon symbol)
        {
            switch (symbol)
            {
                case BalloonIcon.None:
                    return Hardcodet.Wpf.TaskbarNotification.BalloonIcon.None;
                case BalloonIcon.Error:
                    return Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Error;
                case BalloonIcon.Info:
                    return Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info;
                case BalloonIcon.Warning:
                    return Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Warning;
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        protected virtual void OnBalloonTipClosed()
        {
            _mediaPlayer?.Stop();
            BalloonTipClosed?.Invoke(this, EventArgs.Empty);
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

        private void ShowBalloonTip(string title, string message, Icon customIcon, bool largeIcon, string soundSource,
            bool loop, bool looping)
        {
            _loop = loop;
            _looping = looping;
            _soundSource = soundSource;

            if (loop)
            {
                _reshowAction = () => ShowBalloonTip(title, message, customIcon, largeIcon, soundSource, true, true);
            }

            base.ShowBalloonTip(title, message, customIcon, largeIcon);
        }
        #endregion


        /*private void SetSoundPlaying(string soundSource)
        {
            InitializeMediaPlayer(soundSource);

            RoutedEventHandler shownHandler = null;
            shownHandler = (sender, args) =>
            {
                _mediaPlayer.Play();
                TrayBalloonTipShown -= shownHandler;
            };
            TrayBalloonTipShown += shownHandler;

            RoutedEventHandler closedHandler = null;
            closedHandler = (sender, args) =>
            {
                _mediaPlayer.Stop();
                TrayBalloonTipClicked -= closedHandler;
                TrayBalloonTipClosed -= closedHandler;
            };
            TrayBalloonTipClicked += closedHandler;
            TrayBalloonTipClosed += closedHandler;
        }*/
    }
}