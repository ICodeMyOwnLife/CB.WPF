using System.Drawing;
using Hardcodet.Wpf.TaskbarNotification;


namespace CB.WPF.NotificationIcon
{
    public interface IShowBalloonTip
    {
        #region Abstract
        void ShowBalloonTip(string title, string message, BalloonIcon symbol, string soundSource = null);
        void ShowBalloonTip(string title, string message, Icon customIcon, bool largeIcon = false, string soundSource = null);
        #endregion
    }
}