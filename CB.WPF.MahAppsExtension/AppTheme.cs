using System.Windows.Media;


namespace CB.WPF.MahAppsExtension
{
    public class AppTheme: MahAppsTheme
    {
        #region  Properties & Indexers
        public Brush Background { get; set; }
        public Brush BorderBrush { get; set; }
        #endregion


        #region Override
        public override void Apply()
            => MahAppsThemeManager.ChangeAppTheme(Name);
        #endregion
    }
}


//TODO: Bring to CB.WPF.ExtendedMahApps