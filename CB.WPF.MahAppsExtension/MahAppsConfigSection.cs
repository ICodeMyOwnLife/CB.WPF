using System.Configuration;


namespace CB.WPF.MahAppsExtension
{
    public class MahAppsConfigSection: ConfigurationSection
    {
        #region Fields
        protected const string ACCENT_ATTR = "accent";
        protected const string APP_THEME_ATTR = "appTheme";
        #endregion


        #region  Properties & Indexers
        [ConfigurationProperty(ACCENT_ATTR, DefaultValue = MahAppsDefaults.ACCENT, IsRequired = false)]
        public virtual string Accent
        {
            get { return (string)this[ACCENT_ATTR]; }
            set { this[ACCENT_ATTR] = value; }
        }

        [ConfigurationProperty(APP_THEME_ATTR, DefaultValue = MahAppsDefaults.APP_THEME, IsRequired = false)]
        public virtual string AppTheme
        {
            get { return (string)this[APP_THEME_ATTR]; }
            set { this[APP_THEME_ATTR] = value; }
        }
        #endregion
    }
}