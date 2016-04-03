using System;


namespace TestEnumListBox
{
    [Flags]
    public enum Alphabet
    {
        None = 0,
        A = 1,
        B = 2,
        C = 4,
        D = 8,
        E = 16,
        F = 32,
        G = 64,
        H = 128
    }
}