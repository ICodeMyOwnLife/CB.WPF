using System;
using CB.Model.Common;


namespace TestFree.Models
{
    public class TestPropertyEditModel: BindableObject
    {
        #region Fields
        private string _name = "Name";
        private int _year = DateTime.Today.Year;
        #endregion


        #region  Properties & Indexers
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public int Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
        }
        #endregion
    }
}