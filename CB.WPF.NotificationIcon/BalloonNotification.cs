using CB.Prism.Interactivity;


namespace CB.WPF.NotificationIcon
{
    public class BalloonNotification: NotifyContext, IBalloonNotify

    {
        #region  Properties & Indexers
        public string SoundSource { get; set; }
        #endregion
    }
}