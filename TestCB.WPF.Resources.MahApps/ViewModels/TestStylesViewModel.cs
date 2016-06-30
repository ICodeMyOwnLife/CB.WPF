using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using Microsoft.Practices.Prism.Commands;
using TestMahAppsResources.Models;
using ProgressState = TestMahAppsResources.Models.ProgressState;


namespace TestMahAppsResources.ViewModels
{
    public class TestStylesViewModel: PrismViewModelBase
    {
        #region Fields
        private const string ADD_CMD = "Add Person";
        private const string DEL_CMD = "Delete Person";
        private const string SAVE_CMD = "Save Person";
        private readonly string _backgroundFolder = Path.GetFullPath(@"Images\Background");
        private bool _canEdit;
        private ObservableCollection<Person> _people = new ObservableCollection<Person>();
        private double? _progress;
        protected readonly Random _random = new Random(DateTime.Now.Millisecond);
        private string _selectedBackground;
        private string _selectedCommand = ADD_CMD;
        private Person _selectedPerson;
        private ProgressState _state = ProgressState.Pending;
        #endregion


        #region  Constructors & Destructor
        public TestStylesViewModel()
        {
            AddNewPersonCommand = new DelegateCommand(AddNewPerson);
            BrowseAvatarCommand = new DelegateCommand(BrowseAvatar, () => CanEdit);
            DeleteAsynCommand = DelegateCommand.FromAsyncHandler(DeleteAsync, () => CanEdit);
            DoAsyncCommand = DelegateCommand.FromAsyncHandler(DoAsync, CanDo);
            SaveAsynCommand = DelegateCommand.FromAsyncHandler(SaveAsync, () => CanEdit);
            SelectBackgroundCommand = new DelegateCommand<string>(SelectBackground);

            BackgroundFiles = new[] { "" }.Concat(Directory.EnumerateFiles(_backgroundFolder));
            SelectedBackground = BackgroundFiles.FirstOrDefault();
        }
        #endregion


        #region  Commands
        public virtual ICommand AddNewPersonCommand { get; }
        public ICommand BrowseAvatarCommand { get; }
        public virtual ICommand DeleteAsynCommand { get; }
        public virtual ICommand DoAsyncCommand { get; }
        public virtual ICommand SaveAsynCommand { get; }

        public ICommand SelectBackgroundCommand { get; }
        #endregion


        #region  Properties & Indexers
        public IEnumerable<string> BackgroundFiles { get; }

        public bool CanEdit
        {
            get { return _canEdit; }
            protected set
            {
                if (SetProperty(ref _canEdit, value))
                {
                    RaiseCommandsCanExecuteChanged(SaveAsynCommand, DeleteAsynCommand, BrowseAvatarCommand,
                        DoAsyncCommand);
                }
            }
        }

        public IEnumerable<string> Commands { get; } = new[] { ADD_CMD, SAVE_CMD, DEL_CMD };

        /*public ConfirmationInteractionRequest<IConfirmation> FileRequest { get; } =
            new ConfirmationInteractionRequest<IConfirmation>();*/

        public CommonInteractionRequest FileRequest { get; } = new CommonInteractionRequest();

        public ObservableCollection<Person> People
        {
            get { return _people; }
            protected set
            {
                if (SetProperty(ref _people, value))
                {
                    SelectedPerson = value?.FirstOrDefault();
                }
            }
        }

        public double? Progress
        {
            get { return _progress; }
            private set { SetProperty(ref _progress, value); }
        }

        public string SelectedBackground
        {
            get { return _selectedBackground; }
            private set { SetProperty(ref _selectedBackground, value); }
        }

        public string SelectedCommand
        {
            get { return _selectedCommand; }
            set
            {
                if (SetProperty(ref _selectedCommand, value))
                {
                    RaiseCommandsCanExecuteChanged(DoAsyncCommand);
                }
            }
        }

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (SetProperty(ref _selectedPerson, value))
                {
                    CanEdit = SelectedPerson != null;
                }
            }
        }

        public ProgressState State
        {
            get { return _state; }
            private set { SetProperty(ref _state, value); }
        }
        #endregion


        #region Methods
        public virtual void AddNewPerson()
        {
            var p = new Person();
            People.Add(p);
            SelectedPerson = p;
        }

        public void BrowseAvatar()
        {
            if (!CanEdit) return;

            var openFileDialogInfo = new OpenFileDialogInfo
            {
                InitialDirectory = Path.Combine(Environment.CurrentDirectory, "Images")
            };
            FileRequest.Raise(openFileDialogInfo,
                _ => { if (openFileDialogInfo.Confirmed) SelectedPerson.AvatarUrl = openFileDialogInfo.FileName; });
        }

        public virtual async Task DeleteAsync()
        {
            if (!CanEdit) return;

            Progress = null;
            State = ProgressState.Running;
            await Task.Delay(_random.Next(500, 4000));
            People.Remove(SelectedPerson);
            SelectedPerson = People.FirstOrDefault();
            State = ProgressState.Complete;
        }

        public virtual async Task DoAsync()
        {
            switch (SelectedCommand)
            {
                case ADD_CMD:
                    AddNewPerson();
                    break;
                case SAVE_CMD:
                    await SaveAsync();
                    break;
                case DEL_CMD:
                    await DeleteAsync();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public virtual async Task SaveAsync()
        {
            if (!CanEdit) return;

            State = ProgressState.Running;
            Progress = 0;
            while (Progress < 1)
            {
                await Task.Delay(_random.Next(10, 100));
                var newProgress = Progress + _random.NextDouble() / 100;
                Progress = newProgress < 1 ? newProgress : 1;
            }
            State = ProgressState.Complete;
        }

        public void SelectBackground(string backgroundFile)
            => SelectedBackground = backgroundFile;
        #endregion


        #region Implementation
        private bool CanDo()
        {
            switch (SelectedCommand)
            {
                case ADD_CMD:
                    return true;
                case SAVE_CMD:
                case DEL_CMD:
                    return CanEdit;
                default:
                    return false;
            }
        }
        #endregion
    }
}


// TODO: Image View Request
// TODO: Add more properties to Person class
// TODO: Save Accent, Theme, Background Settings