using System;
using System.Collections.ObjectModel;
using TestMahAppsResources.Models;
using TestMahAppsResources.ViewModels;


namespace TestMahAppsResources.DesignViewModels
{
    public class DesignTestStylesViewModel: TestStylesViewModel
    {
        #region  Constructors & Destructor
        public DesignTestStylesViewModel()
        {
            People = new ObservableCollection<Person>(new[]
            {
                new Person
                {
                    Name = "David",
                    DateOfBirth = new DateTime(1975, 4, 30),
                    IsMale = true,
                    IsMarried = true,
                    Religion = Religion.Catholicism,
                    Income = 5649.5m,
                    IntelligeceQuotient = 97
                },
                new Person
                {
                    Name = "Sarah",
                    DateOfBirth = new DateTime(1981, 11, 24),
                    IsMale = false,
                    IsMarried = false,
                    Religion = Religion.Anglicanism,
                    Income = 419.1m,
                    IntelligeceQuotient = 112
                },
                new Person
                {
                    Name = "Paul",
                    DateOfBirth = new DateTime(1968, 5, 7),
                    IsMale = true,
                    IsMarried = false,
                    Religion = Religion.Christianity,
                    Income = 2444.4m,
                    IntelligeceQuotient = 108
                },
                new Person
                {
                    Name = "Mary",
                    DateOfBirth = new DateTime(1977, 1, 17),
                    IsMale = false,
                    IsMarried = true,
                    Religion = Religion.Agnosticism,
                    Income = 604.7m,
                    IntelligeceQuotient = 81
                },
                new Person
                {
                    Name = "Jame",
                    DateOfBirth = new DateTime(1955, 8, 3),
                    IsMale = true,
                    IsMarried = true,
                    Religion = Religion.Atheism,
                    Income = 1239.9m,
                    IntelligeceQuotient = 84
                },
                new Person
                {
                    Name = "John",
                    DateOfBirth = new DateTime(1980, 3, 21),
                    IsMale = true,
                    IsMarried = true,
                    Religion = Religion.Buddhism,
                    Income = 412.3m,
                    IntelligeceQuotient = 77
                },
                new Person
                {
                    Name = "George",
                    DateOfBirth = new DateTime(1963, 10, 5),
                    IsMale = true,
                    IsMarried = false,
                    Religion = Religion.Hinduism,
                    Income = 3742.1m,
                    IntelligeceQuotient = 72
                },
                new Person
                {
                    Name = "Helen",
                    DateOfBirth = new DateTime(1985, 8, 14),
                    IsMale = false,
                    IsMarried = false,
                    Religion = Religion.None,
                    Income = 711.7m,
                    IntelligeceQuotient = 123
                },
                new Person
                {
                    Name = "Jennifer",
                    DateOfBirth = new DateTime(1973, 12, 17),
                    IsMale = false,
                    IsMarried = true,
                    Religion = Religion.Buddhism,
                    Income = 396.7m,
                    IntelligeceQuotient = 89
                },
            });
        }
        #endregion
    }
}