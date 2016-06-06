using System.Reflection;
using System.Windows;
using System.Windows.Threading;


namespace CB_WPF_Test
{
    public class TestApplication: Application
    {
        #region Fields
        private Assembly _assembly;
        #endregion


        #region  Constructors & Destructor
        public TestApplication(Assembly assembly)
        {
            InitializeComponents(assembly);
        }

        public TestApplication()
        {
            InitializeComponents(GetType().Assembly);
        }
        #endregion


        #region Override
        protected override void OnStartup(StartupEventArgs e)
            => new TestWindow { Assembly = _assembly }.Show();
        #endregion


        #region Event Handlers
        private static void TestApplication_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
            => MessageBox.Show(e.Exception.ToString());
        #endregion


        #region Implementation
        private void InitializeComponents(Assembly assembly)
        {
            _assembly = assembly;
            DispatcherUnhandledException += TestApplication_DispatcherUnhandledException;
        }
        #endregion
    }
}