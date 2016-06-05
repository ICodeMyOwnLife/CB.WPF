using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CB.Model.Prism;
using Microsoft.Practices.Prism.Commands;


namespace TestCB.WPF.Controls.ViewModels
{
    public class TestProgressButtonViewModel: PrismViewModelBase
    {
        #region Fields
        private int _taskNumber = 1;
        private readonly ObservableCollection<ProgressTaskViewModel> _tasks = new ObservableCollection<ProgressTaskViewModel>();
        #endregion


        #region  Constructors & Destructor
        public TestProgressButtonViewModel()
        {
            AddTaskCommand = new DelegateCommand(AddTask);
        }
        #endregion


        #region  Commands
        public ICommand AddTaskCommand { get; }
        #endregion


        #region  Properties & Indexers
        public IEnumerable<ProgressTaskViewModel> Tasks => _tasks;
        #endregion


        #region Methods
        public void AddTask()
            => _tasks.Add(new ProgressTaskViewModel { Name = $"Task {_taskNumber++}" });
        #endregion
    }
}