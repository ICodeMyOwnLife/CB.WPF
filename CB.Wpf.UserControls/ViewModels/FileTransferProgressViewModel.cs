using System;
using System.Windows.Input;
using CB.Model.Common;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using Microsoft.Practices.Prism.Commands;
using Prism.Interactivity.InteractionRequest;


namespace CB.Wpf.UserControls
{
    public class FileTransferProgressViewModel: PrismViewModelBase, IConfirmContext, IConfirmation
    {
        #region Fields
        private FileProgressState _state = FileProgressState.Running;
        #endregion


        #region  Constructors & Destructor
        public FileTransferProgressViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel, () => CanCancel);
            PauseCommand = new DelegateCommand(Pause, () => CanPause);
            ResumeCommand = new DelegateCommand(Resume, () => CanResume);
        }
        #endregion


        #region  Commands
        public ICommand CancelCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ResumeCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanCancel => CancelAction != null && State != FileProgressState.Canceled;
        public virtual Action CancelAction { get; set; }
        public bool CanPause => PauseAction != null && State == FileProgressState.Running;
        public bool CanResume => ResumeAction != null && State == FileProgressState.Pausing;
        public bool Confirmed { get; set; }
        public object Content { get; set; }
        public virtual Action PauseAction { get; set; }
        public virtual FileProgressReporter ProgressReporter { get; set; }
        public virtual Action ResumeAction { get; set; }

        public FileProgressState State
        {
            get { return _state; }
            private set
            {
                if (SetProperty(ref _state, value))
                {
                    NotifyPropertiesChanged(nameof(CanCancel), nameof(CanPause), nameof(CanResume));
                    RaiseCommandsCanExecuteChanged(CancelCommand, PauseCommand, ResumeCommand);
                }
            }
        }

        public string Title { get; set; }
        public WindowRequest WindowRequest { get; } = new WindowRequest();
        #endregion


        #region Methods
        public void Cancel()
        {
            if (!CanCancel) return;

            State = FileProgressState.Canceled;
            CancelAction();
            WindowRequest.Raise(WindowRequestAction.Close);
        }

        public void Pause()
        {
            if (!CanPause) return;

            PauseAction();
            State = FileProgressState.Pausing;
        }

        public void Resume()
        {
            if (!CanResume) return;

            ResumeAction();
            State = FileProgressState.Running;
        }
        #endregion
    }
}


// TODO: Xaml namespace: cb.xaml??