using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro;
using Microsoft.Practices.Prism.Commands;


namespace CB.WPF.Resources.MahApps
{
    public class MahAppsThemeManager
    {
        #region Fields
        private static IEnumerable<Accent> _accents;
        private static IEnumerable<AppTheme> _appsThemes;
        #endregion


        #region  Commands
        public static ICommand ChangeAccentCommand { get; } = new DelegateCommand<string>(ChangeAccent);
        public static ICommand ChangeAppThemeCommand { get; } = new DelegateCommand<string>(ChangeAppTheme);
        #endregion


        #region  Properties & Indexers
        public static IEnumerable<Accent> Accents
            => _accents ?? (_accents = ThemeManager.Accents.Select(
                a => new Accent { Name = a.Name, AccentBrush = a.Resources["AccentColorBrush"] as Brush }));

        public static IEnumerable<AppTheme> AppsThemes
            => _appsThemes ?? (_appsThemes = ThemeManager.AppThemes.Select(
                t =>
                new AppTheme
                {
                    Name = t.Name,
                    Background = t.Resources["WhiteColorBrush"] as Brush,
                    BorderBrush = t.Resources["BlackColorBrush"] as Brush
                }));
        #endregion


        #region Methods
        public static void ChangeAccent(string accentName)
        {
            var currentApp = Application.Current;
            var theme = ThemeManager.DetectAppStyle(currentApp);
            var accent = ThemeManager.GetAccent(accentName);
            ThemeManager.ChangeAppStyle(currentApp, accent, theme.Item1);
        }

        public static void ChangeAppTheme(string appThemeName)
        {
            var currentApp = Application.Current;
            var theme = ThemeManager.DetectAppStyle(currentApp);
            var appTheme = ThemeManager.GetAppTheme(appThemeName);
            ThemeManager.ChangeAppStyle(currentApp, theme.Item2, appTheme);
        }
        #endregion
    }
}