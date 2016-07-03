using System;
using System.Drawing;
using CB.Prism.Interactivity;
using Hardcodet.Wpf.TaskbarNotification;


namespace CB.WPF.NotificationIcon
{
    public class BalloonNotifyRequestProvider: RequestProviderBase
    {
        #region Methods
        public virtual void Notify(string title, string message, Icon customIcon,
            bool largeIcon = false, string soundSource = null, Action<ICustomIconBallonNotify> callback = null)
            => Request.Raise(
                new CustomIconBalloonNotification
                {
                    Title = title,
                    Content = message,
                    CustomIcon = customIcon,
                    LargeIcon = largeIcon,
                    SoundSource = soundSource
                }, callback);

        public virtual void Notify(string title, string message, BalloonIcon symbol = BalloonIcon.None,
            string soundSource = null, Action<ISymbolBalloonNotify> callback = null)
            => Request.Raise(new SymbolBallonNotification
            {
                Title = title,
                Content = message,
                Symbol = symbol,
                SoundSource = soundSource
            },
                callback);

        public virtual void NotifyError(string title, string message, string soundSource = null,
            Action<ISymbolBalloonNotify> callback = null)
            => Notify(title, message, BalloonIcon.Error, soundSource, callback);

        public virtual void NotifyErrorOnUiThread(string title, string message, string soundSource = null,
            Action<ISymbolBalloonNotify> callback = null)
            => RunOnUiThread(() => NotifyError(title, message, soundSource, callback));

        public virtual void NotifyInfo(string title, string message, string soundSource = null,
            Action<ISymbolBalloonNotify> callback = null)
            => Notify(title, message, BalloonIcon.Info, soundSource, callback);

        public virtual void NotifyInfoOnUiThread(string title, string message, string soundSource = null,
            Action<ISymbolBalloonNotify> callback = null)
            => RunOnUiThread(() => NotifyInfo(title, message, soundSource, callback));

        public virtual void NotifyOnUiThread(string title, string message, Icon customIcon, bool largeIcon = false,
            string soundSource = null,
            Action<ICustomIconBallonNotify> callback = null)
            => RunOnUiThread(() => Notify(title, message, customIcon, largeIcon, soundSource, callback));

        public virtual void NotifyOnUiThread(string title, string message, BalloonIcon symbol = BalloonIcon.None,
            string soundSource = null, Action<ISymbolBalloonNotify> callback = null)
            => RunOnUiThread(() => Notify(title, message, symbol, soundSource, callback));

        public virtual void NotifyWarning(string title, string message, string soundSource = null,
            Action<ISymbolBalloonNotify> callback = null)
            => Notify(title, message, BalloonIcon.Warning, soundSource, callback);

        public virtual void NotifyWarningOnUiThread(string title, string message, string soundSource = null,
            Action<ISymbolBalloonNotify> callback = null)
            => RunOnUiThread(() => NotifyWarning(title, message, soundSource, callback));
        #endregion
    }
}