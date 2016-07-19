using System.Windows.Input;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using Prism.Commands;


namespace TestCB.Wpf.NotificationIcon.ViewModels
{
    public class TestLoopViewModel: PrismViewModelBase
    {
        #region Fields
        private bool _loop;
        private string _message;
        private string _soundSource;
        private BalloonIcon _symbol;
        private string _title;
        #endregion


        #region  Constructors & Destructor
        public TestLoopViewModel()
        {
            NotifyCommand = new DelegateCommand(Notify);
        }
        #endregion


        #region  Commands
        public ICommand NotifyCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool Loop
        {
            get { return _loop; }
            set {   SetProperty(ref _loop, value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public RequestManager RequestManager { get; } = new RequestManager();

        public string SoundSource
        {
            get { return _soundSource; }
            set { SetProperty(ref _soundSource, value); }
        }

        public BalloonIcon Symbol
        {
            get { return _symbol; }
            set { SetProperty(ref _symbol, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion


        #region Methods
        public void Notify()
            => RequestManager.BalloonNotifyRequestProvider.Notify(Title, Message, Symbol, SoundSource, Loop);
        #endregion
    }
}