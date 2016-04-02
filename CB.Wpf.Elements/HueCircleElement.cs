using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CB.Media.Brushes;
using CB.Media.Brushes.Impl;
using CB.Wpf.Elements.Impl;
using static System.Double;
using static System.Math;


namespace CB.Wpf.Elements
{
    public class HueCircleElement: HueElementBase
    {
        #region Fields
        private const double THUMB_RADIUS = 7.0;
        private double _angularOffset;
        private RadialGradientBrush _radialBrush;
        private double _radialOffset;
        private readonly Pen _thumbPen = new Pen(Brushes.Black, 1.0);
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty RadialColorStopsProperty = DependencyProperty.Register(
            nameof(RadialColorStops), typeof(GradientStopCollection), typeof(HueCircleElement),
            new FrameworkPropertyMetadata(default(GradientStopCollection),
                FrameworkPropertyMetadataOptions.AffectsRender, OnRadialColorStopsChanged));

        public GradientStopCollection RadialColorStops
        {
            get { return (GradientStopCollection)GetValue(RadialColorStopsProperty); }
            set { SetValue(RadialColorStopsProperty, value); }
        }
        #endregion


        #region Override
        protected override Brush CreateBrush(GradientStopCollection gradientStops)
        {
            return new CircularBrushCreator(gradientStops).Create();
        }

        protected override void DrawBackground(DrawingContext drawingContext)
        {
            var centerPoint = GetCenterPoint();
            double radiusX = ActualWidth / 2, radiusY = ActualHeight / 2;
            drawingContext.DrawEllipse(_brush, null, centerPoint, radiusX, radiusY);
            drawingContext.DrawEllipse(_radialBrush, null, centerPoint, radiusX, radiusY);
        }

        protected override void DrawThumb(DrawingContext drawingContext)
            => drawingContext.DrawEllipse(new SolidColorBrush(SelectedColor), _thumbPen, CreateThumbPoint(),
                THUMB_RADIUS, THUMB_RADIUS);

        protected override void GetMouseOffset()
        {
            var mousePoint = Mouse.GetPosition(this);
            var helper = new CircleCoordinateHelper(ActualWidth, ActualHeight);
            _angularOffset = helper.GetAngularOffset(mousePoint.X, mousePoint.Y);
            _radialOffset = helper.GetRadialOffset(mousePoint.X, mousePoint.Y);
        }

        protected override void SetMouseOffset() { }

        protected override void UpdateSelectedColor()
        {
            SelectedColor = LinearBrushHelper.GetLinearOffsetColor(_angularOffset, ColorStops) ?? Colors.White;
        }
        #endregion


        #region Implementation
        private double CalculateRadius()
        {
            return ActualWidth / 2; //UNDONE
        }

        private Point CreateThumbPoint()
        {
            var radius = CalculateRadius();
            if (Abs(_radialOffset) < Epsilon || IsNaN(_angularOffset)) return new Point(radius, radius);
            var rad = _angularOffset * 2 * PI;
            double deltaX = radius * Sin(rad), deltaY = -radius * Cos(rad);
            return new Point(_radialOffset * (radius + deltaX), _radialOffset * (radius + deltaY));
        }

        private Point GetCenterPoint() => new Point(ActualWidth / 2, ActualHeight / 2);

        private static void OnRadialColorStopsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as HueCircleElement;
            element?.OnRadialColorStopsChanged((GradientStopCollection)e.OldValue, (GradientStopCollection)e.NewValue);
        }

        protected virtual void OnRadialColorStopsChanged(GradientStopCollection oldValue,
            GradientStopCollection newValue)
        {
            _radialBrush = new RadialGradientBrush(newValue);
        }
        #endregion
    }
}