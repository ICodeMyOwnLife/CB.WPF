using System.Reflection;
using System.Windows;


namespace CB_WPF_Test
{
    public partial class TestWindow

    {
        #region Fields
        private readonly TestViewModel _testViewModel = new TestViewModel();
        #endregion


        #region  Constructors & Destructor
        public TestWindow()
        {
            InitializeComponent();
            DataContext = _testViewModel;
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty AssemblyProperty = DependencyProperty.Register(
            nameof(Assembly), typeof(Assembly), typeof(TestWindow),
            new PropertyMetadata(default(Assembly), OnAssemblyChanged));

        public Assembly Assembly
        {
            get { return (Assembly)GetValue(AssemblyProperty); }
            set { SetValue(AssemblyProperty, value); }
        }
        #endregion


        #region Implementation
        private static void OnAssemblyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ((TestWindow)d).OnAssemblyChanged((Assembly)e.OldValue, (Assembly)e.NewValue);

        protected virtual void OnAssemblyChanged(Assembly oldValue, Assembly newValue)
            => _testViewModel.Assembly = newValue;
        #endregion
    }
}