using System;
using CB.Model.Common;


namespace TestMahAppsResources.Models
{
    public class Person: BindableObject
    {
        #region Fields
        private string _avatarUrl;
        private DateTime _dateOfBirth;
        private decimal _income;
        private int _intelligeceQuotient;
        private bool _isMale;
        private bool _isMarried;
        private string _name;
        private Religion _religion;
        #endregion


        #region  Properties & Indexers
        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set { SetProperty(ref _avatarUrl, value); }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { SetProperty(ref _dateOfBirth, value); }
        }

        public decimal Income
        {
            get { return _income; }
            set { SetProperty(ref _income, value); }
        }

        public int IntelligeceQuotient
        {
            get { return _intelligeceQuotient; }
            set { SetProperty(ref _intelligeceQuotient, value); }
        }

        public bool IsMale
        {
            get { return _isMale; }
            set { SetProperty(ref _isMale, value); }
        }

        public bool IsMarried
        {
            get { return _isMarried; }
            set { SetProperty(ref _isMarried, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public Religion Religion
        {
            get { return _religion; }
            set { SetProperty(ref _religion, value); }
        }
        #endregion
    }
}