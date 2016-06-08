using CB.Model.Prism;
using TestFree.Models;


namespace TestFree.ViewModels
{
    public class TestPropertyEditViewModel: PrismViewModelBase
    {
        #region  Properties & Indexers
        public TestPropertyEditModel TestModel { get; } = new TestPropertyEditModel();
        #endregion
    }
}