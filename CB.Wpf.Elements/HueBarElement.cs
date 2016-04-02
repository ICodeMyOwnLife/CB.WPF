using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CB.Media.Brushes.Impl;
using CB.Wpf.Elements.Impl;


namespace CB.Wpf.Elements
{
    public class HueBarElement: HueElementBase
    {
        #region Fields
        private double _offset;
        private readonly Pen _squarePen = new Pen(Brushes.Black, 1);
        private readonly SolidColorBrush _thumbBrush = Brushes.Black;
        #endregion


        #region  Constructors & Destructor
        static HueBarElement()
        {
            SelectedColorProperty.OverrideMetadata(typeof(HueBarElement),
                new PropertyMetadata(Color.FromArgb(0, 0, 0, 0)));
        }
        #endregion


        #region Override
        protected override Brush CreateBrush(GradientStopCollection gradientStops)
        {
            return new LinearGradientBrush(gradientStops, new Point(0.0, 0.0), new Point(0.0, 1.0));
        }

        protected override void DrawBackground(DrawingContext drawingContext)
            => drawingContext.DrawRectangle(_brush, null, new Rect(RenderSize));

        protected override void DrawThumb(DrawingContext drawingContext)
        {
            var unit = ActualWidth / 100;
            DrawLateralTriangles(drawingContext, unit);
            DrawCentralSquare(drawingContext, unit);
        }

        protected override void GetMouseOffset()
        {
            var mousePoint = Mouse.GetPosition(this);
            var offset = mousePoint.Y / ActualHeight;
            _offset = offset < 0.0 ? 0.0 : offset > 1.0 ? 1.0 : offset;
        }

        protected override void SetMouseOffset()
        {
            var offset = LinearBrushHelper.GetLinearOffset(SelectedColor, ColorStops);
            _offset = double.IsNaN(offset) ? 0.0 : offset;
        }

        protected override void UpdateSelectedColor()
        {
            _indirectSetSelectedColor = true;
            SelectedColor = LinearBrushHelper.GetLinearOffsetColor(_offset, ColorStops) ??
                            Color.FromArgb(0, 0, 0, 0);
            _indirectSetSelectedColor = false;
        }
        #endregion


        #region Implementation
        private void DrawCentralSquare(DrawingContext drawingContext, double unit)
        {
            var squareDimension = unit * 50;
            var squareLeft = (ActualWidth - squareDimension) / 2;
            var squareTop = _offset * ActualHeight - squareDimension / 2;

            var centerRectangle = new Rect(squareLeft, squareTop, squareDimension, squareDimension);
            drawingContext.DrawRectangle(new SolidColorBrush(SelectedColor), _squarePen, centerRectangle);
        }

        private void DrawLateralTriangles(DrawingContext drawingContext, double unit)
        {
            double triangleWidth = 40 * unit,
                   triangleHeight = triangleWidth / Math.Sqrt(3),
                   thumbY = _offset * ActualHeight,
                   triangleTop = thumbY - triangleHeight,
                   triangleBottom = thumbY + triangleHeight;

            var triangles = Geometry.Parse(
                $"M0,{triangleTop} L{triangleWidth},{thumbY} L0,{triangleBottom} z " +
                $"M{ActualWidth},{triangleTop} L{ActualWidth - triangleWidth},{thumbY} L{ActualWidth},{triangleBottom} z");

            drawingContext.DrawGeometry(_thumbBrush, null, triangles);
        }
        #endregion
    }

    /*[ContentProperty("ColorStops")]
    public class HueBarElement: ColorSelectorElement
    {
        #region Fields
        private readonly LinearGradientBrush _brush;
        private readonly Pen _squarePen = new Pen(Brushes.Black, 1);
        private double _thumb;
        private readonly SolidColorBrush _thumbBrush = Brushes.Black;
        #endregion


        #region  Constructors & Destructors
        static HueBarElement()
        {
            SelectedColorProperty.OverrideMetadata(typeof(HueBarElement),
                new PropertyMetadata(Color.FromArgb(0, 0, 0, 0)));
        }

        public HueBarElement()
        {
            _brush = new LinearGradientBrush
            {
                StartPoint = new Point(),
                EndPoint = new Point(0, 1.0),
            };
            _brush.GradientStops.Changed += OnColorStopsChanged;
        }
        #endregion


        #region  Properties & Indexers
        public GradientStopCollection ColorStops => _brush.GradientStops;
        #endregion


        #region Override
        protected override void DrawBackground(DrawingContext drawingContext)
        {
            var bounds = new Rect(0, 0, ActualWidth, ActualHeight);
            drawingContext.DrawRectangle(_brush, null, bounds);
        }

        protected override void DrawThumb(DrawingContext drawingContext)
        {
            var unit = ActualWidth / 100;
            DrawLateralTriangles(drawingContext, unit);
            DrawCentralSquare(drawingContext, unit);
        }

        protected override void GetMouseOffset()
        {
            var dpiPoint = Mouse.GetPosition(this);
            _thumb = dpiPoint.Y < 0.0 ? 0.0 : dpiPoint.Y > ActualHeight ? ActualHeight : dpiPoint.Y;
        }

        protected override void SetMouseOffset()
        {
            var offset = LinearBrushHelper.GetLinearOffset(SelectedColor, ColorStops);
            _thumb = double.IsNaN(offset) ? 0.0 : offset * ActualHeight;
        }

        protected override void UpdateSelectedColor()
        {
            _indirectSetSelectedColor = true;
            SelectedColor = LinearBrushHelper.GetLinearOffsetColor(_thumb / ActualHeight, ColorStops) ??
                            Color.FromArgb(0, 0, 0, 0);
            _indirectSetSelectedColor = false;
        }
        #endregion


        #region Event Handlers
        private void OnColorStopsChanged(object sender, EventArgs e)
        {
            InvalidateVisual();
        }
        #endregion


        #region Implementation
        private void DrawCentralSquare(DrawingContext drawingContext, double unit)
        {
            var squareDimension = unit * 50;
            var squareLeft = (ActualWidth - squareDimension) / 2;
            var squareTop = _thumb - squareDimension / 2;

            var centerRectangle = new Rect(squareLeft, squareTop, squareDimension, squareDimension);
            drawingContext.DrawRectangle(new SolidColorBrush(SelectedColor), _squarePen, centerRectangle);
        }

        private void DrawLateralTriangles(DrawingContext drawingContext, double unit)
        {
            var triangleWidth = 40 * unit;
            var triangleHeight = triangleWidth / Math.Sqrt(3);
            var triangleTop = _thumb - triangleHeight;
            var triangleBottom = _thumb + triangleHeight;

            var trianglesData = "M{0},{1} L{2},{3} L{4},{5} z M{6},{7} L{8},{9} L{10},{11} z";

            var triangles = Geometry.Parse(string.Format(trianglesData, 0, triangleTop,
                triangleWidth, _thumb, 0, triangleBottom, ActualWidth, triangleTop,
                ActualWidth - triangleWidth, _thumb, ActualWidth, triangleBottom));

            drawingContext.DrawGeometry(_thumbBrush, null, triangles);
        }
        #endregion
    }*/
}