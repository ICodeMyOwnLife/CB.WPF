using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro;
using Microsoft.Practices.Prism.Commands;


namespace CB.WPF.MahAppsExtension
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
            => ChangeTheme(GetCurrentTheme().Item1, ThemeManager.GetAccent(accentName));

        /*{
            var currentApp = Application.Current;
            var theme = ThemeManager.DetectAppStyle(currentApp);
            var accent = ThemeManager.GetAccent(accentName);
            ThemeManager.ChangeAppStyle(currentApp, accent, theme.Item1);
        }*/

        public static void ChangeAppTheme(string appThemeName)
            => ChangeTheme(ThemeManager.GetAppTheme(appThemeName), GetCurrentTheme().Item2);

        /*{
            var currentApp = Application.Current;
            var theme = ThemeManager.DetectAppStyle(currentApp);
            var appTheme = ThemeManager.GetAppTheme(appThemeName);
            ThemeManager.ChangeAppStyle(currentApp, theme.Item2, appTheme);
        }*/

        public static string GetCurrentAccent()
            => GetCurrentTheme()?.Item2?.Name;

        public static string GetCurrentAppTheme()
            => GetCurrentTheme()?.Item1?.Name;
        #endregion


        #region Implementation
        private static void ChangeTheme(MahApps.Metro.AppTheme appTheme, MahApps.Metro.Accent accent)
            => ThemeManager.ChangeAppStyle(Application.Current, accent, appTheme);

        private static Tuple<MahApps.Metro.AppTheme, MahApps.Metro.Accent> GetCurrentTheme()
            => ThemeManager.DetectAppStyle(Application.Current);
        #endregion
    }
}


// TODO: Selected AppTheme and Accent: Done
// TODO: DataGridRowHeaderGripper background