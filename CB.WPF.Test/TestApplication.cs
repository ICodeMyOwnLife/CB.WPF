using System.Reflection;
using System.Windows;


namespace CB_WPF_Test
{
    public class TestApplication: Application
    {
        #region Fields
        private readonly Assembly _assembly;
        #endregion


        #region  Constructors & Destructor
        public TestApplication(Assembly assembly)
        {
            _assembly = assembly;
        }

        public TestApplication()
        {
            _assembly = GetType().Assembly;
        }
        #endregion


        #region Override
        protected override void OnStartup(StartupEventArgs e)
            => new TestWindow { Assembly = _assembly }.Show();
        #endregion
    }
}