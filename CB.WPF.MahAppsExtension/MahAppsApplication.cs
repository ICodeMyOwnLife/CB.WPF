using System.Windows;


namespace CB.WPF.MahAppsExtension
{
    public class MahAppsApplication: Application
    {
        #region Override
        protected override void OnStartup(StartupEventArgs e)
        {
            MahAppsThemeManager.ApplyConfig();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            MahAppsThemeManager.SaveConfig();
            base.OnExit(e);
        }
        #endregion
    }
}