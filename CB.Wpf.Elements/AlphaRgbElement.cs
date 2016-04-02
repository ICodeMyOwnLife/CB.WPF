using System.Windows;
using System.Windows.Media;


namespace CB.Wpf.Elements
{
    public class AlphaRgbElement: FrameworkElement
    {
        #region Dependency Properties
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            nameof(Alpha), typeof(byte), typeof(AlphaRgbElement), new PropertyMetadata(default(byte), OnAlphaChanged));

        public byte Alpha
        {
            get { return (byte)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color), typeof(Color), typeof(AlphaRgbElement),
            new FrameworkPropertyMetadata(default(Color),
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnColorChanged));

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty RgbProperty = DependencyProperty.Register(
            nameof(Rgb), typeof(Color), typeof(AlphaRgbElement), new PropertyMetadata(default(Color), OnRgbChanged));

        public Color Rgb
        {
            get { return (Color)GetValue(RgbProperty); }
            set { SetValue(RgbProperty, value); }
        }
        #endregion


        #region Override
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DrawBackground(drawingContext);
        }
        #endregion


        #region Implementation
        private void DrawBackground(DrawingContext drawingContext)
            => drawingContext.DrawRectangle(new SolidColorBrush(Color), null, new Rect(0, 0, ActualWidth, ActualHeight));

        private static void OnAlphaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as AlphaRgbElement;
            element?.UpdateColor((byte)e.NewValue);
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as AlphaRgbElement;
            element?.UpdateAlphaRgb((Color)e.NewValue);
        }

        private static void OnRgbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as AlphaRgbElement;
            element?.UpdateColor((Color)e.NewValue);
        }

        private void UpdateAlpha(Color color) => SetValue(AlphaProperty, color.A);

        private void UpdateAlphaRgb(Color color)
        {
            UpdateAlpha(color);
            UpdateRgb(color);
        }

        private void UpdateColor(byte alpha) => UpdateColor(Rgb, alpha);

        private void UpdateColor(Color rgb, byte alpha)
            => SetValue(ColorProperty, Color.FromArgb(alpha, rgb.R, rgb.G, rgb.B));

        private void UpdateColor(Color rgb) => UpdateColor(rgb, Alpha);

        private void UpdateRgb(Color color)
            => SetValue(RgbProperty, Color.FromArgb(byte.MaxValue, color.R, color.G, color.B));
        #endregion
    }
}