using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;


namespace CB.WPF.Resources.MahApps
{
    public abstract class MahAppsTheme
    {
        #region  Constructors & Destructor
        protected MahAppsTheme()
        {
            ApplyCommand = new DelegateCommand(Apply);
        }
        #endregion


        #region Abstract
        public abstract void Apply();
        #endregion


        #region  Commands
        public ICommand ApplyCommand { get; }
        #endregion


        #region  Properties & Indexers
        public string Name { get; set; }
        #endregion
    }
}


// TODO: FlatSlider?