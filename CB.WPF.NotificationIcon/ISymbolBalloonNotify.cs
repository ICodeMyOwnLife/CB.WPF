using Hardcodet.Wpf.TaskbarNotification;


namespace CB.WPF.NotificationIcon
{
    public interface ISymbolBalloonNotify: IBalloonNotify
    {
        #region Abstract
        BalloonIcon Symbol { get; set; }
        #endregion
    }
}