using System;
using System.Windows;
using System.Windows.Media;
using CB.Wpf.Elements.Impl;


namespace CB.Wpf.Controls
{
    public class ColorScaleControl: NumericControl
    {
        #region  Constructors & Destructor
        static ColorScaleControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorScaleControl),
                new FrameworkPropertyMetadata(typeof(ColorScaleControl)));
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty ScaleBrushProperty = DependencyProperty.Register(
            nameof(ScaleBrush), typeof(Brush), typeof(ColorScaleControl), new PropertyMetadata(default(Brush)));

        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }

        public static readonly DependencyProperty ScaleColorProperty = DependencyProperty.Register(
            nameof(ScaleColor), typeof(Color), typeof(ColorScaleControl),
            new PropertyMetadata(Color.FromRgb(255, 255, 255)));

        public Color ScaleColor
        {
            get { return (Color)GetValue(ScaleColorProperty); }
            set { SetValue(ScaleColorProperty, value); }
        }

        public static readonly DependencyProperty ScaleComponentProperty = DependencyProperty.Register(
            nameof(ScaleComponent), typeof(ColorComponent), typeof(ColorScaleControl),
            new PropertyMetadata(ColorComponent.Alpha, OnScaleComponentChanged));

        public ColorComponent ScaleComponent
        {
            get { return (ColorComponent)GetValue(ScaleComponentProperty); }
            set { SetValue(ScaleComponentProperty, value); }
        }
        #endregion


        #region Implementation
        private static void OnScaleComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as ColorScaleControl;
            element?.UpdateRange();
        }

        private void UpdateRange()
        {
            switch (ScaleComponent)
            {
                case ColorComponent.Alpha:
                case ColorComponent.Blue:
                case ColorComponent.Green:
                case ColorComponent.Red:
                    Maximum = 255;
                    Minimum = 0;
                    break;

                case ColorComponent.ScA:
                case ColorComponent.ScB:
                case ColorComponent.ScG:
                case ColorComponent.ScR:
                    Maximum = 1.0;
                    Minimum = 0.0;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}