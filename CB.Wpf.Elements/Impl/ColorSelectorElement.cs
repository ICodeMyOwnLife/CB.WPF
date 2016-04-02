using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace CB.Wpf.Elements.Impl
{
    public abstract class ColorSelectorElement : FrameworkElement, ISelectColor
    {
        #region Fields
        protected bool _indirectSetSelectedColor;
        private bool _mouseDown;
        #endregion


        #region Abstract
        protected abstract void DrawBackground(DrawingContext drawingContext);

        protected abstract void DrawThumb(DrawingContext drawingContext);

        protected abstract void GetMouseOffset();

        protected abstract void SetMouseOffset();

        protected abstract void UpdateSelectedColor();
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor",
            typeof(Color), typeof(ColorSelectorElement),
            new PropertyMetadata(Color.FromArgb(0, 0, 0, 0), OnSelectedColorChanged));

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        #endregion


        #region Override
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            _mouseDown = true;
            CaptureMouse();
            SelectColorAtMousePosition();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_mouseDown)
            {
                SelectColorAtMousePosition();
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            _mouseDown = false;
            ReleaseMouseCapture();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DrawBackground(drawingContext);
            DrawThumb(drawingContext);
        }
        #endregion


        #region Implementation
        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as ColorSelectorElement;
            if (element == null) return;
            if (!element._indirectSetSelectedColor)
            {
                element.SetMouseOffset();
            }
            element.InvalidateVisual();
        }

        protected virtual void SelectColorAtMousePosition()
        {
            GetMouseOffset();
            UpdateSelectedColor();
        }
        #endregion
    }
}