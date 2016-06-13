using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using CB.Model.Prism;
using TestMahAppsResources.Models;


namespace TestMahAppsResources.ViewModels
{
    public class TestMahAppsIconViewModel: PrismViewModelBase
    {
        #region Fields
        private string _filter;
        private MahAppsIcon _selectedIcon;
        #endregion


        #region  Constructors & Destructor
        public TestMahAppsIconViewModel()
        {
            var rd = new ResourceDictionary
            {
                Source = new Uri(@"Resources\IconsNonShared.xaml", UriKind.RelativeOrAbsolute)
            };
            IconsView = new ListCollectionView(rd.Keys.OfType<string>().Select(k => new MahAppsIcon(k)).ToList());
        }
        #endregion


        #region  Properties & Indexers
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (!SetProperty(ref _filter, value)) return;

                IconsView.Filter = string.IsNullOrEmpty(value)
                                       ? (Predicate<object>)null
                                       : (o => ((MahAppsIcon)o).Name.IndexOf(value,
                                           StringComparison.InvariantCultureIgnoreCase) >= 0);
            }
        }

        public ListCollectionView IconsView { get; }

        public MahAppsIcon SelectedIcon
        {
            get { return _selectedIcon; }
            set { SetProperty(ref _selectedIcon, value); }
        }
        #endregion
    }
}