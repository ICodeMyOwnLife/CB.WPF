using System;
using System.Windows;


namespace TestEnumListBox
{
    public partial class MainWindow
    {
        #region Fields
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        #endregion


        #region  Constructors & Destructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion


        #region Event Handlers
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var alphabets = new[]
            {
                Alphabet.None,
                Alphabet.B,
                Alphabet.E,
                Alphabet.H,
                Alphabet.H | Alphabet.B,
                Alphabet.G | Alphabet.C,
                Alphabet.F | Alphabet.D,
                Alphabet.A | Alphabet.F,
                Alphabet.B | Alphabet.G,
                Alphabet.C | Alphabet.H,
                Alphabet.C | Alphabet.E | Alphabet.G,
                Alphabet.B | Alphabet.D | Alphabet.F | Alphabet.H
            };
            enumListBox.SelectedValue = alphabets[_random.Next(alphabets.Length)];
        }
        #endregion
    }
}