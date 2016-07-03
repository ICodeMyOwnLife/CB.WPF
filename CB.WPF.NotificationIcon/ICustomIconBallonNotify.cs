using System.Drawing;
using CB.Prism.Interactivity;


namespace CB.WPF.NotificationIcon
{
    public interface ICustomIconBallonNotify: IBalloonNotify
    {
        #region Abstract
        Icon CustomIcon { get; set; }
        bool LargeIcon { get; set; }
        #endregion
    }
}