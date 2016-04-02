using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;


namespace CB.Wpf.Elements.Impl
{
    [ContentProperty("ColorStops")]
    public abstract class HueElementBase: ColorSelectorElement
    {
        protected Brush _brush;

        #region Dependency Properties
        public static readonly DependencyProperty ColorStopsProperty = DependencyProperty.Register(
            nameof(ColorStops), typeof(GradientStopCollection), typeof(HueElementBase),
            new FrameworkPropertyMetadata(default(GradientStopCollection), FrameworkPropertyMetadataOptions.AffectsRender, OnColorStopsChanged));

        private static void OnColorStopsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as HueElementBase;
            element?.OnColorStopsChanged((GradientStopCollection)e.OldValue, (GradientStopCollection)e.NewValue);
        }

        protected virtual void OnColorStopsChanged(GradientStopCollection oldValue, GradientStopCollection newValue)
        {
            _brush = CreateBrush(newValue);
        }

        protected abstract Brush CreateBrush(GradientStopCollection gradientStops);

        public GradientStopCollection ColorStops
        {
            get { return (GradientStopCollection)GetValue(ColorStopsProperty); }
            set { SetValue(ColorStopsProperty, value); }
        }
        #endregion
    }
}