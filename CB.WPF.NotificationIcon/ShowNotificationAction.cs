using System.Windows;
using CB.Prism.Interactivity;
using CB.Xaml.Interactivity;


namespace CB.WPF.NotificationIcon
{
    public class ShowNotificationAction: TargetTriggerAction<FrameworkElement, IShowBalloonTip>
    {
        #region Override
        protected override void Invoke(object parameter)
        {
            var notifier = this.GetTarget();
            var args = parameter as ContextRequestEventArgs;
            if (notifier == null || args == null) return;

            var symbolBalloonNotify = args.Context as ISymbolBalloonNotify;
            if (symbolBalloonNotify != null)
            {
                notifier.ShowBalloonTip(symbolBalloonNotify.Title, symbolBalloonNotify.Content.ToString(),
                    symbolBalloonNotify.Symbol, symbolBalloonNotify.SoundSource);
                return;
            }

            var customIconBalloonNotify = args.Context as ICustomIconBallonNotify;
            if (customIconBalloonNotify != null)
            {
                notifier.ShowBalloonTip(customIconBalloonNotify.Title, customIconBalloonNotify.Content.ToString(),
                    customIconBalloonNotify.CustomIcon, customIconBalloonNotify.LargeIcon, customIconBalloonNotify.SoundSource);
            }
        }
        #endregion
    }
}