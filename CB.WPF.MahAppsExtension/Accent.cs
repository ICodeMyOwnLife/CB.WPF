using System.Windows.Media;


namespace CB.WPF.MahAppsExtension
{
    public class Accent: MahAppsTheme
    {
        #region  Properties & Indexers
        public Brush AccentBrush { get; set; }
        #endregion


        #region Override
        public override void Apply()
            => MahAppsThemeManager.ChangeAccent(Name);
        #endregion
    }
}