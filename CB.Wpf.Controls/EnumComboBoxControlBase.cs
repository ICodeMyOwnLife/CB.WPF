using System;
using System.Windows;
using System.Windows.Controls;


namespace CB.Wpf.Controls
{
    [TemplatePart]
    public abstract class EnumComboBoxControlBase: Control
    {
        #region Fields
        private const string ENUM_COMBOBOX = "enumComboBox";
        protected EnumComboBox _enumComboBox;
        #endregion


        #region  Constructors & Destructor
        static EnumComboBoxControlBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EnumComboBoxControlBase),
                new FrameworkPropertyMetadata(typeof(EnumComboBoxControlBase)));
        }
        #endregion


        #region Abstract
        protected abstract void InitializeEnumComboBox();
        #endregion


        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _enumComboBox = GetTemplateChild(ENUM_COMBOBOX) as EnumComboBox;
            if (_enumComboBox == null)
            {
                throw new Exception(ENUM_COMBOBOX);
            }

            InitializeEnumComboBox();
        }
        #endregion
    }
}