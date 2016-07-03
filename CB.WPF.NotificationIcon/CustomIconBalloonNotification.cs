using System.Drawing;


namespace CB.WPF.NotificationIcon
{
    public class CustomIconBalloonNotification: BalloonNotification, ICustomIconBallonNotify
    {
        #region  Properties & Indexers
        public Icon CustomIcon { get; set; }
        public bool LargeIcon { get; set; }
        #endregion
    }
}