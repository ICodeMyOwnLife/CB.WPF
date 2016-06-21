using System.Configuration;
using System.Windows;


namespace CB.WPF.Common
{
    public class WindowConfigSection: ConfigurationSection
    {
        #region Fields
        protected const string HEIGHT_ATTR = "height";
        protected const string HIDE_ON_START_ATTR = "hideOnStart";
        protected const string WIDTH_ATTR = "width";
        protected const string X_ATTR = "x";
        protected const string Y_ATTR = "y";
        #endregion


        #region  Properties & Indexers
        [ConfigurationProperty(HEIGHT_ATTR, DefaultValue = double.NaN, IsRequired = false)]
        public double Height
        {
            get { return (double)this[HEIGHT_ATTR]; }
            set { this[HEIGHT_ATTR] = value; }
        }

        [ConfigurationProperty(HIDE_ON_START_ATTR, DefaultValue = false, IsRequired = false)]
        public bool HideOnStart
        {
            get { return (bool)this[HIDE_ON_START_ATTR]; }
            set { this[HIDE_ON_START_ATTR] = value; }
        }

        [ConfigurationProperty(WIDTH_ATTR, DefaultValue = double.NaN, IsRequired = false)]
        public double Width
        {
            get { return (double)this[WIDTH_ATTR]; }
            set { this[WIDTH_ATTR] = value; }
        }

        [ConfigurationProperty(X_ATTR, DefaultValue = double.NaN, IsRequired = false)]
        public double X
        {
            get { return (double)this[X_ATTR]; }
            set { this[X_ATTR] = value; }
        }

        [ConfigurationProperty(Y_ATTR, DefaultValue = double.NaN, IsRequired = false)]
        public double Y
        {
            get { return (double)this[Y_ATTR]; }
            set { this[Y_ATTR] = value; }
        }
        #endregion


        #region Methods
        public void Apply(Window window)
        {
            if (window == null) return;

            if (!double.IsNaN(Height)) window.Height = Height;
            if (!double.IsNaN(Width)) window.Width = Width;
            if (!double.IsNaN(X)) window.Left = X;
            if (!double.IsNaN(Y)) window.Top = Y;
            if (HideOnStart) window.Hide();
        }
        #endregion
    }
}