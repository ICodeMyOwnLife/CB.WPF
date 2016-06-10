using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
        public static void ApplyConfig(MahAppsConfiguration mahAppsConfiguration)
            => ApplyConfig(mahAppsConfiguration.ConfigurationSection);

        public static void ApplyConfig()
            => ApplyConfig(new MahAppsConfiguration());

        public static void ChangeAccent(string accentName)
            => ChangeTheme(GetCurrentAppStyle().Item1, ThemeManager.GetAccent(accentName));

        public static void ChangeAppTheme(string appThemeName)
            => ChangeTheme(ThemeManager.GetAppTheme(appThemeName), GetCurrentAppStyle().Item2);

        public static string GetCurrentAccent()
            => GetCurrentAppStyle()?.Item2?.Name;

        public static string GetCurrentAppTheme()
            => GetCurrentAppStyle()?.Item1?.Name;

        public static void SaveConfig()
        {
            var config = new MahAppsConfiguration();
            var appStyle = GetCurrentAppStyle();
            var configurationSection = config.ConfigurationSection;
            configurationSection.Accent = appStyle.Item2.Name;
            configurationSection.AppTheme = appStyle.Item1.Name;
            configurationSection.CurrentConfiguration.Save(ConfigurationSaveMode.Modified);
        }
        #endregion


        #region Implementation
        private static void ApplyConfig(MahAppsConfigSection mahAppsConfigSection)
        {
            /*var appTheme = ThemeManager.GetAppTheme(mahAppsConfigSection.AppTheme);
            ThemeManager.AddAppTheme(appTheme.Name, appTheme.Resources.Source);*/
            ChangeTheme(ThemeManager.GetAppTheme(mahAppsConfigSection.AppTheme),
                ThemeManager.GetAccent(mahAppsConfigSection.Accent));
        }

        private static void ChangeTheme(MahApps.Metro.AppTheme appTheme, MahApps.Metro.Accent accent)
            => /*ThemeManager.*/ChangeAppStyle(Application.Current, accent, appTheme);

        private static Tuple<MahApps.Metro.AppTheme, MahApps.Metro.Accent> GetCurrentAppStyle()
            => ThemeManager.DetectAppStyle(Application.Current);
        #endregion


        #region Test
        public static void ChangeAppStyle(Application app, MahApps.Metro.Accent newAccent, MahApps.Metro.AppTheme newTheme)
        {
            if (app == null) throw new ArgumentNullException("app");

            var oldTheme = ThemeManager.DetectAppStyle(app);
            ChangeAppStyle(app.Resources, oldTheme, newAccent, newTheme);
        }

        private static void ChangeAppStyle(ResourceDictionary resources, Tuple<MahApps.Metro.AppTheme, MahApps.Metro.Accent> oldThemeInfo, MahApps.Metro.Accent newAccent, MahApps.Metro.AppTheme newTheme)
        {
            var themeChanged = false;
            if (oldThemeInfo != null)
            {
                var oldAccent = oldThemeInfo.Item2;
                if (oldAccent != null && oldAccent.Name != newAccent.Name)
                {
                    var key = oldAccent.Resources.Source.ToString().ToLower();
                    var oldAccentResource = resources.MergedDictionaries.Where(x => x.Source != null).FirstOrDefault(d => d.Source.ToString().ToLower() == key);
                    if (oldAccentResource != null)
                    {
                        resources.MergedDictionaries.Add(newAccent.Resources);
                        resources.MergedDictionaries.Remove(oldAccentResource);

                        themeChanged = true;
                    }
                }

                var oldTheme = oldThemeInfo.Item1;
                if (oldTheme != null && oldTheme != newTheme)
                {
                    var key = oldTheme.Resources.Source.ToString().ToLower();
                    var oldThemeResource = resources.MergedDictionaries.Where(x => x.Source != null).FirstOrDefault(d => d.Source.ToString().ToLower() == key);
                    if (oldThemeResource != null)
                    {
                        resources.MergedDictionaries.Add(newTheme.Resources);
                        resources.MergedDictionaries.Remove(oldThemeResource);

                        themeChanged = true;
                    }
                }
            }
            else
            {
                ChangeAppStyle(resources, newAccent, newTheme);

                themeChanged = true;
            }

            /*if (themeChanged)
            {
                OnThemeChanged(newAccent, newTheme);
            }*/
        }

        public static void ChangeAppStyle(ResourceDictionary resources, MahApps.Metro.Accent newAccent, MahApps.Metro.AppTheme newTheme)
        {
            if (resources == null) throw new ArgumentNullException("resources");
            if (newAccent == null) throw new ArgumentNullException("newAccent");
            if (newTheme == null) throw new ArgumentNullException("newTheme");

            ApplyResourceDictionary(newAccent.Resources, resources);
            ApplyResourceDictionary(newTheme.Resources, resources);
        }

        private static void ApplyResourceDictionary(ResourceDictionary newRd, ResourceDictionary oldRd)
        {
            oldRd.BeginInit();

            foreach (DictionaryEntry r in newRd)
            {
                if (oldRd.Contains(r.Key))
                    oldRd.Remove(r.Key);

                oldRd.Add(r.Key, r.Value);
            }

            oldRd.EndInit();
        }
        #endregion


        #region  Commands
        public static ICommand ChangeAccentCommand { get; } = new DelegateCommand<string>(ChangeAccent);
        public static ICommand ChangeAppThemeCommand { get; } = new DelegateCommand<string>(ChangeAppTheme);
        #endregion
    }
}


// TODO: ApplyConfig and SaveConfig: Debug ChangeAppStyle!!!
// TODO: DataGridRowHeaderGripper background