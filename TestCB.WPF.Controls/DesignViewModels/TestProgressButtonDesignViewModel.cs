using TestCB.WPF.Controls.ViewModels;


namespace TestCB.WPF.Controls.DesignViewModels
{
    public class TestProgressButtonDesignViewModel: TestProgressButtonViewModel
    {
        #region  Constructors & Destructor
        public TestProgressButtonDesignViewModel()
        {
            for (var i = 0; i < 10; i++)
            {
                AddTask();
            }
        }
        #endregion
    }
}