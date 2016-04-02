using System;
using System.Windows;
using System.Windows.Media;
using CB.Media.Brushes;
using CB.Wpf.Elements.Impl;


namespace CB.Wpf.Elements
{
    public class ColorScaleBarElement: FrameworkElement
    {
        #region Fields
        private const int OPAQUE_SQUARE_DIMENSION = 3;

        /*private readonly GradientBrush _brush = new LinearGradientBrush(new GradientStopCollection
        {
            new GradientStop(Colors.Transparent, 0.0),
            new GradientStop(Colors.Transparent, 1.0)
        }, new Point(0, 0), new Point(1, 0));*/

        private readonly Brush _opaqueBrush = SquareBrush.Create(Colors.White, Colors.LightGray, OPAQUE_SQUARE_DIMENSION,
            OPAQUE_SQUARE_DIMENSION);
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty ScaleBrushProperty = DependencyProperty.Register(
            nameof(ScaleBrush), typeof(Brush), typeof(ColorScaleBarElement), new PropertyMetadata(default(Brush)));

        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }

        public static readonly DependencyProperty ScaleColorProperty = DependencyProperty.Register(
            nameof(ScaleColor), typeof(Color), typeof(ColorScaleBarElement),
            new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.AffectsRender));

        public Color ScaleColor
        {
            get { return (Color)GetValue(ScaleColorProperty); }
            set { SetValue(ScaleColorProperty, value); }
        }

        public static readonly DependencyProperty ScaleComponentProperty = DependencyProperty.Register(
            nameof(ScaleComponent), typeof(ColorComponent), typeof(ColorScaleBarElement),
            new FrameworkPropertyMetadata(ColorComponent.Alpha, FrameworkPropertyMetadataOptions.AffectsRender));

        /*private static void OnScaleComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as ColorScaleBarElement;
            if (element == null) return;

            element.UpdateBrush();
            element.InvalidateVisual();
        }*/

        public ColorComponent ScaleComponent
        {
            get { return (ColorComponent)GetValue(ScaleComponentProperty); }
            set { SetValue(ScaleComponentProperty, value); }
        }
        #endregion


        #region Override
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            UpdateBrush();
            DrawBackground(drawingContext);
        }
        #endregion


        #region Implementation
        private void DrawBackground(DrawingContext drawingContext)
        {
            var bounds = GetBounds();
            drawingContext.DrawRectangle(_opaqueBrush, null, bounds);
            drawingContext.DrawRectangle(ScaleBrush, null, bounds);
        }

        private Rect GetBounds() => new Rect(RenderSize);

        private ColorInterpolationMode GetColorInterpolationMode()
        {
            switch (ScaleComponent)
            {
                case ColorComponent.Alpha:
                case ColorComponent.Blue:
                case ColorComponent.Green:
                case ColorComponent.Red:
                    return ColorInterpolationMode.SRgbLinearInterpolation;

                case ColorComponent.ScA:
                case ColorComponent.ScB:
                case ColorComponent.ScG:
                case ColorComponent.ScR:
                    return ColorInterpolationMode.ScRgbLinearInterpolation;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Color GetColorWithComponent(byte componentValue)
        {
            var color = ScaleColor;
            SetColorComponent(ref color, componentValue);
            return color;
        }

        private void SetColorComponent(ref Color color, byte value)
        {
            switch (ScaleComponent)
            {
                case ColorComponent.Alpha:
                case ColorComponent.ScA:
                    color.A = value;
                    break;

                case ColorComponent.Blue:
                case ColorComponent.ScB:
                    color.B = value;
                    break;

                case ColorComponent.Green:
                case ColorComponent.ScG:
                    color.G = value;
                    break;

                case ColorComponent.Red:
                case ColorComponent.ScR:
                    color.R = value;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateBrush()
        {
            var brush = new LinearGradientBrush(GetColorWithComponent(byte.MinValue),
                GetColorWithComponent(byte.MaxValue), new Point(0, 0), new Point(1, 0))
            {
                ColorInterpolationMode = GetColorInterpolationMode()
            };
            ScaleBrush = brush;
            /*_brush.GradientStops[0].Color = GetColorWithComponent(byte.MinValue);
            _brush.GradientStops[1].Color = GetColorWithComponent(byte.MaxValue);*/
        }
        #endregion
    }
}