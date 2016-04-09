using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;


namespace CB.Wpf.Controls
{
    [TemplatePart(Name = POPUP, Type = typeof(Popup))]
    public class EnumComboBox: ComboBox
    {
        #region Fields
        private const string POPUP = "PART_Popup";
        protected Popup _popup;
        #endregion


        #region  Properties & Indexers
        public EnumListBoxControlBase EnumListBoxControl { get; set; }
        #endregion


        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popup = GetTemplateChild(POPUP) as Popup;
            if (_popup == null) throw new Exception(POPUP);

            _popup.Child = EnumListBoxControl;
        }
        #endregion
    }
}