using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using CB.Wpf.Controls.Helpers;
using CB.Wpf.Controls.Inpl;


namespace CB.Wpf.Controls
{
    [ContentProperty("Items"), DefaultEvent("ItemClick"), DefaultProperty("Items")]
    public class ListPopup: Popup, IListPopup
    {
        #region Fields
        private static readonly Brush HIGHLIGHT_BRUSH = Brushes.Khaki;
        private static readonly Brush HIGHTLIGHT_TEXT_BRUSH = Brushes.Maroon;

        public static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent(
            "ItemClick", RoutingStrategy.Bubble, typeof(ItemClickRoutedEventHandler), typeof(ListPopup));

        protected ListBox _listBox;

        protected ScrollViewer _scrollViewer;
        #endregion


        #region  Constructors & Destructors
        static ListPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListPopup),
                new FrameworkPropertyMetadata(typeof(ListPopup)));
            ChildProperty.OverrideMetadata(typeof(ListPopup),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));
        }

        public ListPopup()
        {
            InitializeComponent();
        }
        #endregion


        #region  Properties & Indexers
        public new double ActualHeight => _listBox.ActualHeight;

        public new double ActualWidth => _listBox.ActualWidth;

        public Thickness BorderThickness
        {
            get { return (Thickness)_listBox.GetValue(Control.BorderThicknessProperty); }
            set { _listBox.SetValue(Control.BorderThicknessProperty, value); }
        }

        public ItemCollection Items => _listBox.Items;

        public Point Location
        {
            get { return PlacementRectangle.Location; }
            set { PlacementRectangle = new Rect(value, new Size()); }
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty BackgroundProperty =
            Control.BackgroundProperty.AddOwner(typeof(ListPopup));

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty =
            Control.BorderBrushProperty.AddOwner(typeof(ListPopup));

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty =
            Control.BorderThicknessProperty.AddOwner(typeof(ListPopup));

        public static readonly DependencyProperty FontFamilyProperty =
            Control.FontFamilyProperty.AddOwner(typeof(ListPopup));

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty =
            Control.FontSizeProperty.AddOwner(typeof(ListPopup));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontStretchProperty =
            Control.FontStretchProperty.AddOwner(typeof(ListPopup));

        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public static readonly DependencyProperty FontStyleProperty =
            Control.FontStyleProperty.AddOwner(typeof(ListPopup));

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public static readonly DependencyProperty FontWeightProperty =
            Control.FontWeightProperty.AddOwner(typeof(ListPopup));

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty =
            Control.ForegroundProperty.AddOwner(typeof(ListPopup));

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            ItemsControl.ItemsSourceProperty.AddOwner(typeof(ListPopup));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            Selector.SelectedIndexProperty.AddOwner(typeof(ListPopup));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            Selector.SelectedItemProperty.AddOwner(typeof(ListPopup));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        #endregion


        #region Events
        public event ItemClickRoutedEventHandler ItemClick
        {
            add { AddHandler(ItemClickEvent, value); }
            remove { RemoveHandler(ItemClickEvent, value); }
        }
        #endregion


        #region Methods
        public void MoveDownOneItem()
        {
            MoveTo(_listBox.SelectedIndex + 1, true);
        }

        public void MoveDownOnePage()
        {
            MoveTo(_listBox.SelectedIndex + (int)_scrollViewer.ViewportHeight);
        }

        public void MoveToEnd()
        {
            MoveTo(int.MaxValue);
        }

        public void MoveToHome()
        {
            MoveTo(0);
        }

        public void MoveUpOneItem()
        {
            MoveTo(_listBox.SelectedIndex - 1, true);
        }

        public void MoveUpOnePage()
        {
            MoveTo(_listBox.SelectedIndex - (int)_scrollViewer.ViewportHeight);
        }
        #endregion


        #region Override


        #region Overridden
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            InitilizeScrollViewer();
        }
        #endregion


        #endregion


        #region Event Handlers
        private void ListBoxItems_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (item != null)
            {
                OnItemClick(item.Content);
            }
        }
        #endregion


        #region Implementation
        private void ApplyListBoxItemStyle(ResourceDictionary resourceDictionary, IEnumerable<SetterBase> setters,
            IEnumerable<TriggerBase> triggers)
        {
            var listBoxItemType = typeof(ListBoxItem);

            var listBoxItemStyle = new Style(listBoxItemType)
            {
                Resources = resourceDictionary
            };

            foreach (var setter in setters)
            {
                listBoxItemStyle.Setters.Add(setter);
            }

            foreach (var trigger in triggers)
            {
                listBoxItemStyle.Triggers.Add(trigger);
            }

            _listBox.Resources[listBoxItemType] = listBoxItemStyle;
        }

        private KeyValuePair<DependencyProperty, BindingBase> CreateListBoxBinding(DependencyProperty dependencyProperty)
        {
            var binding = new Binding(dependencyProperty.Name)
            {
                Source = this
            };
            return new KeyValuePair<DependencyProperty, BindingBase>(dependencyProperty, binding);
        }

        private static int GetActualIndex(int index, int itemCount, bool turnAround)
        {
            if (turnAround)
            {
                return index >= itemCount
                           ? 0
                           : index < 0
                                 ? itemCount - 1
                                 : index;
            }

            return index >= itemCount
                       ? itemCount - 1
                       : index < 0
                             ? 0
                             : index;
        }

        private void InitializeComponent()
        {
            _listBox = new ListBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            SetListBoxBindings();
            SetListBoxBrushKeys();
            SetListBoxItemStyle();
            SetScrollBarStyle();
            Child = _listBox;
        }

        private void InitilizeScrollViewer()
        {
            if (_scrollViewer == null)
            {
                _scrollViewer = ScrollViewerHelper.GetScrollViewer(_listBox);
                Debug.Assert(_scrollViewer != null, "scrollViewer != null");
            }
        }

        private void MoveTo(int index, bool turnAround = false)
        {
            if (_listBox.Items == null || _listBox.Items.Count == 0)
            {
                return;
            }

            var actualIndex = GetActualIndex(index, _listBox.Items.Count, turnAround);
            var item = _listBox.Items[actualIndex];
            _listBox.SelectedIndex = actualIndex;
            _listBox.ScrollIntoView(item);
        }

        protected virtual void OnItemClick(object clickedContent)
        {
            RaiseEvent(new ItemClickEventArgs(ItemClickEvent, this, clickedContent));
        }

        protected virtual void OnSettingListBoxBindings(IDictionary<DependencyProperty, BindingBase> bindings)
        {
            bindings.Add(CreateListBoxBinding(OpacityProperty));
            bindings.Add(CreateListBoxBinding(Control.BackgroundProperty));
            bindings.Add(CreateListBoxBinding(Control.BorderBrushProperty));
            bindings.Add(CreateListBoxBinding(Control.BorderThicknessProperty));
            bindings.Add(CreateListBoxBinding(Control.ForegroundProperty));
            bindings.Add(CreateListBoxBinding(Control.FontSizeProperty));
            bindings.Add(CreateListBoxBinding(Control.FontStretchProperty));
            bindings.Add(CreateListBoxBinding(Control.FontStyleProperty));
            bindings.Add(CreateListBoxBinding(Control.FontWeightProperty));
            bindings.Add(CreateListBoxBinding(ItemsControl.ItemsSourceProperty));
            bindings.Add(CreateListBoxBinding(Selector.SelectedIndexProperty));
            bindings.Add(CreateListBoxBinding(Selector.SelectedItemProperty));
        }

        protected virtual void OnSettingListBoxBrushKeys(IDictionary<ResourceKey, Brush> brushKeys)
        {
            brushKeys[SystemColors.HighlightBrushKey] = HIGHLIGHT_BRUSH;
            brushKeys[SystemColors.InactiveSelectionHighlightBrushKey] = HIGHLIGHT_BRUSH;
            brushKeys[SystemColors.HighlightTextBrushKey] = HIGHTLIGHT_TEXT_BRUSH;
            brushKeys[SystemColors.InactiveSelectionHighlightTextBrushKey] = HIGHTLIGHT_TEXT_BRUSH;
        }

        protected virtual void OnSettingListBoxItemStyle(ResourceDictionary resourceDictionary,
            IList<SetterBase> setters, IList<TriggerBase> triggers)
        {
            var previewMouseDownEventSetter = new EventSetter(PreviewMouseDownEvent,
                new MouseButtonEventHandler(ListBoxItems_PreviewMouseDown));
            setters.Add(previewMouseDownEventSetter);
        }

        private void SetListBoxBindings()
        {
            var bindings = new Dictionary<DependencyProperty, BindingBase>();
            OnSettingListBoxBindings(bindings);
            foreach (var binding in bindings)
            {
                _listBox.SetBinding(binding.Key, binding.Value);
            }
        }

        private void SetListBoxBrushKeys()
        {
            var brushKeys = new Dictionary<ResourceKey, Brush>();
            OnSettingListBoxBrushKeys(brushKeys);
            foreach (var brushKey in brushKeys)
            {
                _listBox.Resources[brushKey.Key] = brushKey.Value;
            }
        }

        private void SetListBoxItemStyle()
        {
            var resourceDictionary = new ResourceDictionary();
            var setters = new List<SetterBase>();
            var triggers = new List<TriggerBase>();

            OnSettingListBoxItemStyle(resourceDictionary, setters, triggers);
            ApplyListBoxItemStyle(resourceDictionary, setters, triggers);
        }

        private void SetScrollBarStyle()
        {
            var resUri = new Uri(@"/CB.Wpf.Controls;Component/Themes/SimpleScrollBar.xaml", UriKind.RelativeOrAbsolute);
            var resDict = new ResourceDictionary
            {
                Source = resUri
            };
            var simpleScrollBarStyle = resDict["SimpleScrollBarStyle"] as Style;
            _listBox.Resources.Add(typeof(ScrollBar), simpleScrollBarStyle);
        }
        #endregion
    }
}