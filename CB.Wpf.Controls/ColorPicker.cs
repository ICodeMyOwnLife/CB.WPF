using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CB.Wpf.Controls.Inpl;
using CB.Wpf.Elements.Impl;


namespace CB.Wpf.Controls
{
    [TemplatePart(Name = BRIGHTNESS_SCALE_ELEMENT, Type = typeof(FrameworkElement)),
     TemplatePart(Name = HUEBAR_ELEMENT, Type = typeof(FrameworkElement)),
     TemplatePart(Name = RECTANGLE_PREVIOUS_COLOR, Type = typeof(Rectangle)),
     TemplatePart(Name = RECTANGLE_SELECTED_COLOR, Type = typeof(Rectangle))]
    public class ColorPicker: Control, ISelectColor, IPickColor
    {
        #region Fields
        private const string BRIGHTNESS_SCALE_ELEMENT = "brightnessScaleElement";
        private const string HUEBAR_ELEMENT = "hueBarElement";
        private const string RECTANGLE_PREVIOUS_COLOR = "rectanglePreviousColor";
        private const string RECTANGLE_SELECTED_COLOR = "rectangleSelectedColor";
        private bool _argbChanged;
        private FrameworkElement _brightnessScaleElement;
        private FrameworkElement _huebarElement;
        private Rectangle _rectanglePreviousColor;
        private Rectangle _rectangleSelectedColor;
        private bool _rgbChanged;

        private bool _scArgbChanged;
        private bool _selectedColorChanged;
        #endregion


        #region  Constructors & Destructor
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker),
                new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            nameof(Alpha), typeof(byte), typeof(ColorPicker), new PropertyMetadata(byte.MaxValue, OnArgbChanged));

        public byte Alpha
        {
            get { return (byte)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(
            nameof(Blue), typeof(byte), typeof(ColorPicker), new PropertyMetadata(default(byte), OnArgbChanged));

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(
            nameof(Green), typeof(byte), typeof(ColorPicker), new PropertyMetadata(default(byte), OnArgbChanged));

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public static readonly DependencyProperty OpaqueColorProperty = DependencyProperty.Register(
            nameof(OpaqueColor), typeof(Color), typeof(ColorPicker),
            new PropertyMetadata(default(Color), OnOpaqueColorChanged));

        public Color OpaqueColor
        {
            get { return (Color)GetValue(OpaqueColorProperty); }
            set { SetValue(OpaqueColorProperty, value); }
        }

        /*public static readonly DependencyPropertyKey PickedColorPropertyKey =
            DependencyProperty.RegisterReadOnly("PickedColor", typeof(Color), typeof(ColorPicker),
                new PropertyMetadata(default(Color), OnPickedColorChanged));

        private static void OnPickedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static readonly DependencyProperty PickedColorProperty =
            PickedColorPropertyKey.DependencyProperty;

        public Color PickedColor => (Color)GetValue(PickedColorProperty);*/

        public static readonly DependencyProperty PickedColorProperty = DependencyProperty.Register(
            nameof(PickedColor), typeof(Color), typeof(ColorPicker), new PropertyMetadata(default(Color)));

        public Color PickedColor
        {
            get { return (Color)GetValue(PickedColorProperty); }
            set { SetValue(PickedColorProperty, value); }
        }

        public static readonly DependencyPropertyKey PreviousColorPropertyKey =
            DependencyProperty.RegisterReadOnly("PreviousColor", typeof(Color), typeof(ColorPicker),
                new PropertyMetadata(default(Color)));

        public static readonly DependencyProperty PreviousColorProperty =
            PreviousColorPropertyKey.DependencyProperty;

        public Color PreviousColor => (Color)GetValue(PreviousColorProperty);

        public static readonly DependencyProperty RedProperty = DependencyProperty.Register(
            nameof(Red), typeof(byte), typeof(ColorPicker), new PropertyMetadata(byte.MaxValue, OnArgbChanged));

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public static readonly DependencyProperty ScAProperty = DependencyProperty.Register(
            nameof(ScA), typeof(float), typeof(ColorPicker), new PropertyMetadata(1.0f, OnScArgbChanged));

        public float ScA
        {
            get { return (float)GetValue(ScAProperty); }
            set { SetValue(ScAProperty, value); }
        }

        public static readonly DependencyProperty ScBProperty = DependencyProperty.Register(
            nameof(ScB), typeof(float), typeof(ColorPicker), new PropertyMetadata(default(float), OnScArgbChanged));

        public float ScB
        {
            get { return (float)GetValue(ScBProperty); }
            set { SetValue(ScBProperty, value); }
        }

        public static readonly DependencyProperty ScGProperty = DependencyProperty.Register(
            nameof(ScG), typeof(float), typeof(ColorPicker), new PropertyMetadata(default(float), OnScArgbChanged));

        public float ScG
        {
            get { return (float)GetValue(ScGProperty); }
            set { SetValue(ScGProperty, value); }
        }

        public static readonly DependencyProperty ScRProperty = DependencyProperty.Register(
            nameof(ScR), typeof(float), typeof(ColorPicker), new PropertyMetadata(1.0f, OnScArgbChanged));

        public float ScR
        {
            get { return (float)GetValue(ScRProperty); }
            set { SetValue(ScRProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            nameof(SelectedColor), typeof(Color), typeof(ColorPicker),
            new PropertyMetadata(Colors.Red, OnColorChanged));

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        #endregion


        #region Methods
        public void UpdatePickedColor()
        {
            SetValue(PickedColorProperty, SelectedColor);
        }
        #endregion


        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeBrightnessScaleElement();
            InitializeHueBarElement();
            InitializePreviousColorElement();
            InitializeSelectedColorElement();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            SavePreviousColor();
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }
        #endregion


        #region Event Handlers
        private void BrightnessScaleElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdatePickedColor();
        }

        private void HuebarElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdatePickedColor();
        }

        private void RectanglePreviousColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = e.Source as Rectangle;
            var fill = rectangle?.Fill as SolidColorBrush;
            if (fill != null)
            {
                SelectColor(fill.Color);
                UpdatePickedColor();
            }
        }

        private void RectangleSelectedColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdatePickedColor();
        }
        #endregion


        #region Implementation
        private static void DebugWriteLine(DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(
                $"{nameof(ColorPicker)}.{e.Property}Changed: {{OldValue: {e.OldValue}, NewValue: {e.NewValue}}}");
        }

        private void InitializeBrightnessScaleElement()
        {
            _brightnessScaleElement = GetTemplateChild(BRIGHTNESS_SCALE_ELEMENT) as FrameworkElement;
            if (_brightnessScaleElement != null)
            {
                _brightnessScaleElement.MouseLeftButtonUp += BrightnessScaleElement_MouseLeftButtonUp;
            }
        }

        private void InitializeHueBarElement()
        {
            _huebarElement = GetTemplateChild(HUEBAR_ELEMENT) as FrameworkElement;
            if (_huebarElement != null)
            {
                _huebarElement.MouseLeftButtonUp += HuebarElement_MouseLeftButtonUp;
            }
        }

        private void InitializePreviousColorElement()
        {
            _rectanglePreviousColor = GetTemplateChild(RECTANGLE_PREVIOUS_COLOR) as Rectangle;
            if (_rectanglePreviousColor != null)
                _rectanglePreviousColor.MouseLeftButtonDown += RectanglePreviousColor_MouseLeftButtonDown;
        }

        private void InitializeSelectedColorElement()
        {
            _rectangleSelectedColor = GetTemplateChild(RECTANGLE_SELECTED_COLOR) as Rectangle;
            if (_rectangleSelectedColor != null)
                _rectangleSelectedColor.MouseLeftButtonDown += RectangleSelectedColor_MouseLeftButtonDown;
        }

        private static void OnArgbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugWriteLine(e);
            var element = d as ColorPicker;
            element?.UpdateColor(UpdateOptions.Argb);
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugWriteLine(e);
            var element = d as ColorPicker;
            element?.UpdateComponents();
        }

        private static void OnOpaqueColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugWriteLine(e);
            var element = d as ColorPicker;
            element?.UpdateColor(UpdateOptions.Rgb);
        }

        private static void OnScArgbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugWriteLine(e);
            var element = d as ColorPicker;
            element?.UpdateColor(UpdateOptions.ScArgb);
        }

        private void SavePreviousColor()
        {
            SetValue(PreviousColorPropertyKey, PickedColor);
        }

        private void SelectColor(Color color)
        {
            SetValue(SelectedColorProperty, color);
        }

        private void UpdateArgb(Color color)
        {
            Alpha = color.A;
            Red = color.R;
            Green = color.G;
            Blue = color.B;
        }

        private void UpdateColor(UpdateOptions updateOptions)
        {
            if (_selectedColorChanged) return;

            switch (updateOptions)
            {
                case UpdateOptions.Argb:
                    UpdateColorFromArgb();
                    break;
                case UpdateOptions.Rgb:
                    UpdateColorFromRgb();
                    break;
                case UpdateOptions.ScArgb:
                    UpdateColorFromScArgb();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateOptions), updateOptions, null);
            }
        }

        private void UpdateColorFromArgb()
        {
            _argbChanged = true;
            SelectedColor = Color.FromArgb(Alpha, Red, Green, Blue);
            _argbChanged = false;
        }

        private void UpdateColorFromRgb()
        {
            _rgbChanged = true;
            var rgb = OpaqueColor;
            SelectedColor = Color.FromArgb(Alpha, rgb.R, rgb.G, rgb.B);
            _rgbChanged = false;
        }

        private void UpdateColorFromScArgb()
        {
            _scArgbChanged = true;
            SelectedColor = Color.FromScRgb(ScA, ScR, ScG, ScB);
            _scArgbChanged = false;
        }

        private void UpdateComponents()
        {
            var color = SelectedColor;
            _selectedColorChanged = true;
            if (!_argbChanged)
            {
                UpdateArgb(color);
            }
            if (!_rgbChanged)
            {
                UpdateOpaqueColor(color);
            }
            if (!_scArgbChanged)
            {
                UpdateScArgb(color);
            }
            _selectedColorChanged = false;
        }

        private void UpdateOpaqueColor(Color color)
        {
            OpaqueColor = Color.FromRgb(color.R, color.G, color.B);
        }

        private void UpdateScArgb(Color color)
        {
            ScA = color.ScA;
            ScR = color.ScR;
            ScG = color.ScG;
            ScB = color.ScB;
        }
        #endregion
    }

    internal enum UpdateOptions
    {
        Argb,
        Rgb,
        ScArgb
    }

    /*[TemplatePart(Name = BRIGHTNESS_SCALE_ELEMENT, Type = typeof(FrameworkElement)),
     TemplatePart(Name = HUEBAR_ELEMENT, Type = typeof(FrameworkElement)),
     TemplatePart(Name = RECTANGLE_PREVIOUS_COLOR, Type = typeof(Rectangle)),
     TemplatePart(Name = RECTANGLE_SELECTED_COLOR, Type = typeof(AlphaRgbElement))]
    public class ColorPicker: Control
    {
        #region Fields
        private const string BRIGHTNESS_SCALE_ELEMENT = "brightnessScaleElement";
        private const string HUEBAR_ELEMENT = "hueBarElement";
        private const string RECTANGLE_PREVIOUS_COLOR = "rectanglePreviousColor";
        private const string RECTANGLE_SELECTED_COLOR = "rectangleSelectedColor";
        private FrameworkElement _brightnessScaleElement;
        private FrameworkElement _huebarElement;
        private Rectangle _rectanglePreviousColor;
        private AlphaRgbElement _rectangleSelectedColor;
        #endregion


        #region  Constructors & Destructors
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker),
                new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            nameof(Alpha), typeof(byte), typeof(ColorPicker), new PropertyMetadata(byte.MaxValue));

        public byte Alpha
        {
            get { return (byte)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        public static readonly DependencyPropertyKey PickedColorPropertyKey =
            DependencyProperty.RegisterReadOnly("PickedColor", typeof(Color), typeof(ColorPicker),
                new PropertyMetadata(default(Color)));

        public static readonly DependencyProperty PickedColorProperty =
            PickedColorPropertyKey.DependencyProperty;

        public Color PickedColor => (Color)GetValue(PickedColorProperty);

        public static readonly DependencyPropertyKey PreviousColorPropertyKey =
            DependencyProperty.RegisterReadOnly("PreviousColor", typeof(Color), typeof(ColorPicker),
                new PropertyMetadata(default(Color)));

        public static readonly DependencyProperty PreviousColorProperty =
            PreviousColorPropertyKey.DependencyProperty;

        public Color PreviousColor => (Color)GetValue(PreviousColorProperty);

        public static readonly DependencyProperty SelectedColorProperty =
            ColorSelectorElement.SelectedColorProperty.AddOwner(typeof(ColorPicker));

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        #endregion


        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeBrightnessScaleElement();
            InitializeHueBarElement();
            InitializePreviousColorElement();
            InitializeSelectedColorElement();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            SavePreviousColor();
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }
        #endregion


        #region Event Handlers
        private void BrightnessScaleElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdatePickedColor();
        }

        private void HuebarElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdatePickedColor();
        }

        private void RectanglePreviousColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = e.Source as Rectangle;
            var fill = rectangle?.Fill as SolidColorBrush;
            if (fill != null)
            {
                SelectColor(fill.Color);
                UpdatePickedColor();
            }
        }
        #endregion


        #region Implementation
        private void InitializeBrightnessScaleElement()
        {
            _brightnessScaleElement = GetTemplateChild(BRIGHTNESS_SCALE_ELEMENT) as FrameworkElement;
            if (_brightnessScaleElement != null)
            {
                _brightnessScaleElement.MouseLeftButtonUp += BrightnessScaleElement_MouseLeftButtonUp;
            }
        }

        private void InitializeHueBarElement()
        {
            _huebarElement = GetTemplateChild(HUEBAR_ELEMENT) as FrameworkElement;
            if (_huebarElement != null)
            {
                _huebarElement.MouseLeftButtonUp += HuebarElement_MouseLeftButtonUp;
            }
        }

        private void InitializePreviousColorElement()
        {
            _rectanglePreviousColor = GetTemplateChild(RECTANGLE_PREVIOUS_COLOR) as Rectangle;
            if (_rectanglePreviousColor != null)
                _rectanglePreviousColor.MouseLeftButtonDown += RectanglePreviousColor_MouseLeftButtonDown;
        }

        private void InitializeSelectedColorElement()
        {
            _rectangleSelectedColor = GetTemplateChild(RECTANGLE_SELECTED_COLOR) as AlphaRgbElement;
            if (_rectangleSelectedColor != null)
                _rectangleSelectedColor.MouseLeftButtonDown += RectangleSelectedColor_MouseLeftButtonDown; ;
        }

        private void RectangleSelectedColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdatePickedColor();
        }

        private void SavePreviousColor()
        {
            SetValue(PreviousColorPropertyKey, PickedColor);
        }

        private void SelectColor(Color color)
        {
            SetValue(SelectedColorProperty, color);
        }

        private void UpdatePickedColor()
        {
            SetValue(PickedColorPropertyKey, SelectedColor);
        }
        #endregion
    }*/
}