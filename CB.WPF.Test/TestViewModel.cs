using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using CB.Model.Prism;
using MahApps.Metro.Controls;
using Microsoft.Practices.Prism.Commands;


namespace CB_WPF_Test
{
    public class TestViewModel: PrismViewModelBase
    {
        #region Fields
        private Assembly _assembly;
        private IEnumerable<Type> _windowTypes;
        #endregion


        #region  Commands
        public ICommand ShowDialogWindowCommand { get; } = new DelegateCommand<Type>(ShowDialogWindow);
        public ICommand ShowWindowCommand { get; } = new DelegateCommand<Type>(ShowWindow);
        #endregion


        #region  Properties & Indexers
        public static NotificationRequestProvider NotificationProvider { get; } = new NotificationRequestProvider();

        public Assembly Assembly
        {
            get { return _assembly; }
            set
            {
                if (SetProperty(ref _assembly, value))
                {
                    WindowTypes = _assembly?.GetTypes().Where(IsWindowType);
                }
            }
        }

        public IEnumerable<Type> WindowTypes
        {
            get { return _windowTypes; }
            set { SetProperty(ref _windowTypes, value); }
        }
        #endregion


        #region Methods
        public static bool IsWindowType(Type windowType)
            => windowType != null && (windowType == typeof(Window) || windowType.IsSubclassOf(typeof(Window)));

        public static void ShowDialogWindow(Type windowType)
        {
            var window = CreateWindow(windowType);
            if (window != null)
            {
                NotificationProvider.Notify("Dialog Result", window.ShowDialog());
            }
        }

        public static void ShowWindow(Type windowType)
            => CreateWindow(windowType)?.Show();
        #endregion


        #region Implementation
        private static Window CreateWindow(Type windowType)
        {
            var window = Activator.CreateInstance(windowType);
            return window as MetroWindow ?? window as Window;
        }
        #endregion
    }
}