using CB.Configuration.Common;


namespace CB.WPF.MahAppsExtension
{
    public class MahAppsConfiguration: ConfigurationBase<MahAppsConfigSection>
    {
        #region  Constructors & Destructor
        public MahAppsConfiguration(): this("mahAppsConfig") { }

        public MahAppsConfiguration(string configSectionName): base(configSectionName) { }
        #endregion
    }
}