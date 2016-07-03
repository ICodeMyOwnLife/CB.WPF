using Hardcodet.Wpf.TaskbarNotification;


namespace CB.WPF.NotificationIcon
{
    public class SymbolBallonNotification: BalloonNotification, ISymbolBalloonNotify
    {
        #region  Properties & Indexers
        public BalloonIcon Symbol { get; set; }
        #endregion
    }
}