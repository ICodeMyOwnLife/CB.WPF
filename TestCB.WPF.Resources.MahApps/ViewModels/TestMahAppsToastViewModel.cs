using System;
using System.Windows.Input;
using CB.Model.Prism;
using CB.WPF.MahAppsResources;
using Microsoft.Practices.Prism.Commands;


namespace TestMahAppsResources.ViewModels
{
    public class TestMahAppsToastViewModel: PrismViewModelBase
    {
        #region Fields
        private string _commands;
        private string _content;
        private TimeSpan? _duration;
        private string _iconSource;
        #endregion


        #region  Constructors & Destructor
        public TestMahAppsToastViewModel()
        {
            ShowCommand = new DelegateCommand(Show);
        }
        #endregion


        #region  Commands
        public ICommand ShowCommand { get; }
        #endregion


        #region  Properties & Indexers
        public string Commands
        {
            get { return _commands; }
            set { SetProperty(ref _commands, value); }
        }

        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        public TimeSpan? Duration
        {
            get { return _duration; }
            set { SetProperty(ref _duration, value); }
        }

        public string IconSource
        {
            get { return _iconSource; }
            set { SetProperty(ref _iconSource, value); }
        }
        #endregion


        #region Methods
        public void Show()
            => MahAppsToast.Show(Content, IconSource, Duration, Commands?.Split(','));
        #endregion
    }
}