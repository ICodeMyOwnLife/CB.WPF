using CB.Prism.Interactivity;


namespace CB.WPF.NotificationIcon
{
    public interface IBalloonNotify: INotifyContext
    {
        #region Abstract
        string SoundSource { get; set; }
        #endregion
    }
}