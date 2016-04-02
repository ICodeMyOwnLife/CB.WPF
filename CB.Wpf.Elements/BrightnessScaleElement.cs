using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using CB.Media.Brushes.Impl;
using CB.Wpf.Elements.Impl;


namespace CB.Wpf.Elements
{
    [ContentProperty("RootColor")]
    public class BrightnessScaleElement: ColorSelectorElement
    {
        #region Fields
        private const double THUMB_RADIUS = 7.0;
        private static readonly Color _black = Color.FromArgb(255, 0, 0, 0);

        private static readonly Color _transparent = Color.FromArgb(0, 0, 0, 0);

        private static readonly LinearGradientBrush _blackMask = new LinearGradientBrush(_transparent,
            _black, 90.0);

        private static readonly Pen _blackPen = new Pen(Brushes.Black, 1);

        private static readonly Color _white = Color.FromArgb(255, 255, 255, 255);
        private static readonly Pen _whitePen = new Pen(Brushes.White, 1);

        private bool _directSetRootColor;
        private double _offsetX;

        private double _offsetY;

        private readonly LinearGradientBrush _whiteMask = new LinearGradientBrush(_transparent, _white, new Point(1, 0),
            new Point(0, 0));
        #endregion


        #region  Constructors & Destructor
        static BrightnessScaleElement()
        {
            SelectedColorProperty.OverrideMetadata(typeof(BrightnessScaleElement),
                new PropertyMetadata(_transparent));
        }
        #endregion


        #region  Properties & Indexers
        public Color DefaultRootColor { get; set; } = Colors.Red;
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty RootColorProperty =
            DependencyProperty.Register("RootColor", typeof(Color), typeof(BrightnessScaleElement),
                new FrameworkPropertyMetadata(new Color(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRootColorChanged));

        public Color RootColor
        {
            get { return (Color)GetValue(RootColorProperty); }
            set { SetValue(RootColorProperty, value); }
        }
        #endregion


        #region Override
        protected override void DrawBackground(DrawingContext drawingContext)
        {
            var bounds = new Rect(0, 0, ActualWidth, ActualHeight);
            _whiteMask.GradientStops[0].Color = (Color)GetValue(RootColorProperty);

            drawingContext.DrawRectangle(_whiteMask, null, bounds);
            drawingContext.DrawRectangle(_blackMask, null, bounds);
        }

        protected override void DrawThumb(DrawingContext drawingContext)
        {
            var thumbPen = ColorHelper.CalculateBrightness(SelectedColor) > 0.5 ? _blackPen : _whitePen;
            drawingContext.DrawEllipse(new SolidColorBrush(SelectedColor), thumbPen, CreateThumbPoint(), THUMB_RADIUS,
                THUMB_RADIUS);
        }

        protected override void GetMouseOffset()
        {
            var mousePoint = Mouse.GetPosition(this);
            double offsetX = mousePoint.X / ActualWidth, offsetY = mousePoint.Y / ActualHeight;
            _offsetX = offsetX < 0.0 ? 0.0 : offsetX > 1.0 ? 1.0 : offsetX;
            _offsetY = offsetY < 0.0 ? 0.0 : offsetY > 1.0 ? 1.0 : offsetY;
        }

        protected override void SetMouseOffset()
        {
            double offsetX, offsetY;
            var rootColor = LinearBrushHelper.GetRootColor(SelectedColor, out offsetX, out offsetY);
            _directSetRootColor = true;
            RootColor = rootColor ?? DefaultRootColor;
            _directSetRootColor = false;
            _offsetX = offsetX;
            _offsetY = offsetY;
        }

        protected override void UpdateSelectedColor()
        {
            var selectedColor = LinearBrushHelper.GetBrightnessScaleColor(RootColor, 1 - _offsetX, _offsetY);
            _indirectSetSelectedColor = true;
            SetValue(SelectedColorProperty, selectedColor);
            _indirectSetSelectedColor = false;
        }
        #endregion


        #region Implementation
        private Point CreateThumbPoint() => new Point(_offsetX * ActualWidth, _offsetY * ActualHeight);

        private static void OnRootColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as BrightnessScaleElement;
            if (element == null) return;

            if (!element._directSetRootColor)
            {
                element.UpdateSelectedColor();
            }
        }
        #endregion
    }

    /*[ContentProperty("RootColor")]
    public class BrightnessScaleElement: ColorSelectorElement
    {
        #region Fields
        private const double THUMB_RADIUS = 7.0;
        private static readonly Color _black = Color.FromArgb(255, 0, 0, 0);

        private static readonly Color _transparent = Color.FromArgb(0, 0, 0, 0);

        private static readonly LinearGradientBrush _blackMask = new LinearGradientBrush(_transparent,
            _black, 90.0);

        private static readonly Pen _blackPen = new Pen(Brushes.Black, 1);

        private static readonly Color _white = Color.FromArgb(255, 255, 255, 255);
        private static readonly Pen _whitePen = new Pen(Brushes.White, 1);

        private bool _directSetRootColor;
        private Point _thumbPoint;

        private readonly LinearGradientBrush _whiteMask = new LinearGradientBrush(_transparent, _white, new Point(1, 0),
            new Point(0, 0));
        #endregion


        #region  Constructors & Destructors
        static BrightnessScaleElement()
        {
            SelectedColorProperty.OverrideMetadata(typeof(BrightnessScaleElement),
                new PropertyMetadata(_transparent));
        }
        #endregion


        #region  Properties & Indexers
        public Color DefaultRootColor { get; set; } = Colors.Red;
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty RootColorProperty =
            DependencyProperty.Register("RootColor", typeof(Color), typeof(BrightnessScaleElement),
                new FrameworkPropertyMetadata(new Color(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRootColorChanged));

        public Color RootColor
        {
            get { return (Color)GetValue(RootColorProperty); }
            set { SetValue(RootColorProperty, value); }
        }
        #endregion


        #region Override
        protected override void DrawBackground(DrawingContext drawingContext)
        {
            var bounds = new Rect(0, 0, ActualWidth, ActualHeight);
            _whiteMask.GradientStops[0].Color = (Color)GetValue(RootColorProperty);

            drawingContext.DrawRectangle(_whiteMask, null, bounds);
            drawingContext.DrawRectangle(_blackMask, null, bounds);
        }

        protected override void DrawThumb(DrawingContext drawingContext)
        {
            var thumbPen = ColorHelper.CalculateBrightness(SelectedColor) > 0.5 ? _blackPen : _whitePen;
            drawingContext.DrawEllipse(new SolidColorBrush(SelectedColor), thumbPen, _thumbPoint, THUMB_RADIUS,
                THUMB_RADIUS);
        }

        protected override void GetMouseOffset()
        {
            var dpiPoint = Mouse.GetPosition(this);
            _thumbPoint.X = dpiPoint.X < 0.0 ? 0.0 : dpiPoint.X > ActualWidth ? ActualWidth : dpiPoint.X;
            _thumbPoint.Y = dpiPoint.Y < 0.0 ? 0.0 : dpiPoint.Y > ActualHeight ? ActualHeight : dpiPoint.Y;
        }

        protected override void SetMouseOffset()
        {
            double offsetX, offsetY;
            var rootColor = LinearBrushHelper.GetRootColor(SelectedColor, out offsetX, out offsetY);
            _directSetRootColor = true;
            RootColor = rootColor ?? DefaultRootColor;
            _directSetRootColor = false;
            _thumbPoint.X = offsetX * ActualWidth;
            _thumbPoint.Y = offsetY * ActualHeight;
        }

        protected override void UpdateSelectedColor()
        {
            var offsetX = 1 - _thumbPoint.X / ActualWidth;
            var offsetY = _thumbPoint.Y / ActualHeight;
            var selectedColor = LinearBrushHelper.GetBrightnessScaleColor(RootColor, offsetX, offsetY);
            _indirectSetSelectedColor = true;
            SetValue(SelectedColorProperty, selectedColor);
            _indirectSetSelectedColor = false;
        }

        protected override void UpdateThumbPosition()
        {
            
        }
        #endregion


        #region Implementation
        private static void OnRootColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as BrightnessScaleElement;
            if (element == null) return;

            if (!element._directSetRootColor)
            {
                element.UpdateSelectedColor();
            }
        }
        #endregion
    }*/
}