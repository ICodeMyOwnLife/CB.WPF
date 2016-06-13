using System.Windows;


namespace TestMahAppsResources.Models
{
    public class MahAppsIcon
    {
        #region  Constructors & Destructor
        public MahAppsIcon(string name)
        {
            Name = name;
        }
        #endregion


        #region  Properties & Indexers
        public object Icon => string.IsNullOrEmpty(Name) ? null : Application.Current.FindResource(Name);
        public string Name { get; set; }
        #endregion
    }
}