using System;
using System.Windows.Input;
using CB.Model.Common;
using CB.Model.Prism;
using Microsoft.Practices.Prism.Commands;


namespace CB.Wpf.UserControls
{
    public class FileTransferProgressViewModel: PrismViewModelBase
    {
        #region Fields
        private FileProgressState _state = FileProgressState.Running;
        #endregion


        #region  Constructors & Destructor
        public FileTransferProgressViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            PauseCommand = new DelegateCommand(Pause);
            ResumeCommand = new DelegateCommand(Resume);
        }
        #endregion


        #region  Properties & Indexers
        public bool CanCancel => CancelAction != null;
        public virtual Action CancelAction { get; set; }
        public ICommand CancelCommand { get; }
        public bool CanPause => PauseAction != null && State == FileProgressState.Running;
        public bool CanResume => ResumeAction != null && State == FileProgressState.Pausing;
        public virtual Action PauseAction { get; set; }
        public ICommand PauseCommand { get; }
        public virtual FileProgressReporter ProgressReporter { get; set; }
        public virtual Action ResumeAction { get; set; }
        public ICommand ResumeCommand { get; }

        public FileProgressState State
        {
            get { return _state; }
            private set
            {
                if (SetProperty(ref _state, value))
                {
                    NotifyPropertiesChanged(nameof(CanCancel), nameof(CanPause), nameof(CanResume));
                }
            }
        }
        #endregion


        #region Methods
        public void Cancel()
        {
            if (!CanCancel) return;

            CancelAction();
            State = FileProgressState.Canceled;
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