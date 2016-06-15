using CB.Configuration.Common;


namespace CB.WPF.Common
{
    public class WindowConfiguration: ConfigurationBase<WindowConfigSection>
    {
        #region  Constructors & Destructor
        public WindowConfiguration(): this("windowConfig") { }
        public WindowConfiguration(string configSectionName): base(configSectionName) { }
        #endregion
    }
}