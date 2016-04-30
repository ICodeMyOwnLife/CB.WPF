using System;


namespace CB.WPF.Controls.DataControls
{
    [Flags]
    public enum IdModelButtons
    {
        None = 0,
        Add = 1,
        Load = 2,
        Save = 4,
        Delete = 8,
        Copy = 16
    }
}