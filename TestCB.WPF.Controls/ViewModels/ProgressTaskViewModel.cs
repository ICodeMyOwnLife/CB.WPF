using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CB.Model.Common;
using CB.Model.Prism;
using Microsoft.Practices.Prism.Commands;


namespace TestCB.WPF.Controls.ViewModels
{


    public class ProgressTaskViewModel: PrismViewModelBase
    {
        #region Fields
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);
        private bool _canExecute = true;
        private string _name;
        private Progress _progress;
        #endregion


        #region  Constructors & Destructor
        public ProgressTaskViewModel()
        {
            ExecuteCommand = DelegateCommand.FromAsyncHandler(Execute, () => CanExecute);
        }
        #endregion


        #region  Commands
        public ICommand ExecuteCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanExecute
        {
            get { return _canExecute; }
            private set
            {
                if (SetProperty(ref _canExecute, value))
                {
                    RaiseCommandsCanExecuteChanged(ExecuteCommand);
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public Progress Progress
        {
            get { return _progress; }
            set
            {
                if (_progress.IsRunning != value.IsRunning)
                    CanExecute = !value.IsRunning;

                SetProperty(ref _progress, value);
            }
        }
        #endregion


        #region Methods
        public async Task Execute()
        {
            if (!CanExecute) return;

            if (IsIndeterminate())
            {
                Progress = Progress.IndeterminateProgress;
                await Task.Delay(_random.Next(1000, 5000));
            }
            else
            {
                Progress = Progress.FromValue(0);
                while (Progress.Value < 1)
                {
                    await Task.Delay(_random.Next(50, 150));
                    var increase = _random.NextDouble() / 50;
                    Progress = Progress.FromValue(Progress.Value + increase > 1 ? 1 : Progress.Value + increase);
                }
            }
            Progress = Progress.NotRunningProgress;
        }
        #endregion


        #region Implementation
        private static bool IsIndeterminate()
            => _random.Next() % 5 == 0;
        #endregion
    }
}


// TODO: Implement ProgressTaskViewModelBase Pause, Resume, Cancel methods & commands
// TODO: ProgressButton not behave well when applying template -> Reference MetroAccentButton style