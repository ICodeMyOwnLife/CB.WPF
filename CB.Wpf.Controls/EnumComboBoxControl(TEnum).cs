using System;
using System.Windows;
using System.Windows.Data;


namespace CB.Wpf.Controls
{
    public class EnumComboBoxControl<TEnum>: EnumComboBoxControlBase
        where TEnum: struct, IComparable, IConvertible, IFormattable
    {
        #region Dependency Properties
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
            nameof(SelectedValue), typeof(TEnum), typeof(EnumComboBoxControl<TEnum>),
            new PropertyMetadata(default(TEnum)));

        public TEnum SelectedValue
        {
            get { return (TEnum)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        #endregion


        #region Override
        protected override void InitializeEnumComboBox()
        {
            var enumListBoxControl = new EnumListBoxControl<TEnum>();
            _enumComboBox.EnumListBoxControl = enumListBoxControl;
            var binding = new Binding(nameof(SelectedValue))
            {
                Source = this,
                Mode = BindingMode.TwoWay
            };
            enumListBoxControl.SetBinding(EnumListBoxControl<TEnum>.SelectedValueProperty, binding);
        }
        #endregion
    }
}