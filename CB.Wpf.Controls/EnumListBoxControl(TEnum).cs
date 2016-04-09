using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;


namespace CB.Wpf.Controls
{
    public class EnumListBoxControl<TEnum>: EnumListBoxControlBase where TEnum: struct, IComparable, IConvertible, IFormattable
    {
        #region Fields
        private bool _listBoxSelectionChanged;

        private bool _selectedValueChanged;
        #endregion


        #region  Constructors & Destructor
        static EnumListBoxControl()
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum || !enumType.GetCustomAttributes<FlagsAttribute>().Any())
                throw new InvalidOperationException();
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
            nameof(SelectedValue), typeof(TEnum), typeof(EnumListBoxControl<TEnum>),
            new PropertyMetadata(default(TEnum), OnSelectedValueChanged));

        public TEnum SelectedValue
        {
            get { return (TEnum)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        #endregion


        #region Override
        protected override void InitilizeListBox()
        {
            _listBox.ItemsSource = Enum.GetValues(typeof(TEnum));
            _listBox.SelectionChanged += ListBox_SelectionChanged;
        }
        #endregion


        #region Event Handlers
        protected void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_listBoxSelectionChanged)
            {
                _listBoxSelectionChanged = true;
                if (!_selectedValueChanged)
                {
                    SetSelectedValueFromSelectedItems();
                }
            }
            _listBoxSelectionChanged = false;
        }
        #endregion


        #region Implementation
        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EnumListBoxControl<TEnum>)d).OnSelectedValueChanged((TEnum)e.OldValue, (TEnum)e.NewValue);
        }

        protected virtual void OnSelectedValueChanged(TEnum oldValue, TEnum newValue)
        {
            if (!_selectedValueChanged)
            {
                _selectedValueChanged = true;
                if (!_listBoxSelectionChanged)
                {
                    SetSelectedItemsFromSelectedValue(newValue);
                }
            }
            _selectedValueChanged = false;
        }

        private void SetSelectedItemsFromSelectedValue(TEnum newValue)
        {
            _listBox.SelectedItems.Clear();
            dynamic newEnumValue = newValue;
            var zeroValue = default(TEnum);
            var enumItems = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

            if (Equals(newValue, zeroValue))
            {
                foreach (var item in enumItems.Where(item => Equals(zeroValue, item)))
                {
                    _listBox.SelectedItems.Add(item);
                }
            }
            else
            {
                foreach (var item in enumItems.Where(item => !Equals(zeroValue, item) && newEnumValue.HasFlag(item)))
                {
                    _listBox.SelectedItems.Add(item);
                }
            }
        }

        private void SetSelectedValueFromSelectedItems()
        {
            dynamic value = default(TEnum);
            foreach (TEnum item in _listBox.SelectedItems)
            {
                value |= item;
            }
            SelectedValue = value;
        }
        #endregion
    }
}