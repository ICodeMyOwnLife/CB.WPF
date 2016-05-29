using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CB.Model.Prism;
using Microsoft.Practices.Prism.Commands;
using TestMahAppsResources.Models;


namespace TestMahAppsResources.ViewModels
{
    public class TestStylesViewModel: PrismViewModelBase
    {
        #region Fields
        private const string ADD_CMD = "Add Person";
        private const string DEL_CMD = "Delete Person";
        private const string SAVE_CMD = "Save Person";
        private ObservableCollection<Person> _people = new ObservableCollection<Person>();
        private double? _progress;
        protected readonly Random _random = new Random(DateTime.Now.Millisecond);
        private string _selectedCommand = ADD_CMD;
        private Person _selectedPerson;
        private ProgressState _state = ProgressState.Pending;
        #endregion


        #region  Constructors & Destructor
        public TestStylesViewModel()
        {
            AddNewPersonCommand = new DelegateCommand(AddNewPerson);
            DeleteAsynCommand = DelegateCommand.FromAsyncHandler(DeleteAsync, CanEdit);
            DoAsyncCommand = DelegateCommand.FromAsyncHandler(DoAsync, CanDo);
            SaveAsynCommand = DelegateCommand.FromAsyncHandler(SaveAsync, CanEdit);
        }
        #endregion


        #region  Commands
        public virtual ICommand AddNewPersonCommand { get; }
        public virtual ICommand DeleteAsynCommand { get; }
        public virtual ICommand DoAsyncCommand { get; }
        public virtual ICommand SaveAsynCommand { get; }
        #endregion


        #region  Properties & Indexers
        public IEnumerable<string> Commands { get; } = new[] { ADD_CMD, SAVE_CMD, DEL_CMD };

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

        public string SelectedCommand
        {
            get { return _selectedCommand; }
            set { SetProperty(ref _selectedCommand, value); }
        }

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set { SetProperty(ref _selectedPerson, value); }
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

        public virtual async Task DeleteAsync()
        {
            if (!CanEdit()) return;

            Progress = null;
            State = ProgressState.Running;
            await Task.Delay(_random.Next(2500, 4500));
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
            if (!CanEdit()) return;

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
                    return CanEdit();
                default:
                    return false;
            }
        }

        private bool CanEdit() => SelectedPerson != null;
        #endregion
    }
}