using System;
using System.Windows;
using System.Windows.Controls;


namespace CB.Wpf.Controls
{
    [TemplatePart(Name = LISTBOX, Type = typeof(ListBox))]
    public abstract class EnumListBoxControlBase: Control
    {
        #region Fields
        private const string LISTBOX = "listBox";
        protected ListBox _listBox;
        #endregion


        #region  Constructors & Destructor
        static EnumListBoxControlBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EnumListBoxControlBase),
                new FrameworkPropertyMetadata(typeof(EnumListBoxControlBase)));
        }
        #endregion


        #region Abstract
        protected abstract void InitilizeListBox();
        #endregion


        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _listBox = GetTemplateChild(LISTBOX) as ListBox;
            if (_listBox == null)
            {
                throw new Exception(LISTBOX);
            }
            InitilizeListBox();
        }
        #endregion
    }
}