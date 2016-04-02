using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using CB.Wpf.Controls.Inpl;


namespace CB.Wpf.Controls
{
    public class NumericControl : RangeBase
    {
        #region Fields
        private const string BUTTON_DOWN = "buttonDown";
        private const string BUTTON_UP = "buttonUp";
        private const double MAXIMAL_POINTS_PER_CHANGE = 7;
        private const double MINIMAL_POINTS_PER_CHANGE = 2;
        private const string PROGRESS_BAR = "progressBar";
        private const string TEXT_BOX = "textBox";

        public static readonly DependencyProperty BarBrushProperty = DependencyProperty.Register(
            nameof(BarBrush), typeof(Brush), typeof(NumericControl), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty NumericControlStyleProperty = DependencyProperty.Register(
            nameof(NumericControlStyle), typeof(NumericControlStyle), typeof(NumericControl),
            new PropertyMetadata(default(NumericControlStyle)));

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            nameof(StringFormat), typeof(string), typeof(NumericControl),
            new PropertyMetadata(default(string), OnStringFormatChanged));

        public static readonly RoutedEvent CommitValueChangedEvent = EventManager.RegisterRoutedEvent(
            "CommitValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>),
            typeof(NumericControl));

        private ButtonBase _buttonDown;
        private ButtonBase _buttonUp;
        private bool _dragging;
        private Point _mouseLocation;
        private Control _progressBar;
        private TextBoxBase _textBox;
        #endregion


        #region  Constructors & Destructor
        static NumericControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericControl),
                new FrameworkPropertyMetadata(typeof(NumericControl)));
        }
        #endregion


        #region Dependency Properties
        public Brush BarBrush
        {
            get { return (Brush)GetValue(BarBrushProperty); }
            set { SetValue(BarBrushProperty, value); }
        }

        public NumericControlStyle NumericControlStyle
        {
            get { return (NumericControlStyle)GetValue(NumericControlStyleProperty); }
            set { SetValue(NumericControlStyleProperty, value); }
        }

        public string StringFormat
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }
        #endregion


        #region Events
        public event RoutedPropertyChangedEventHandler<double> CommitValueChanged
        {
            add { AddHandler(CommitValueChangedEvent, value); }
            remove { RemoveHandler(CommitValueChangedEvent, value); }
        }

        public static readonly RoutedEvent MyEventEvent = EventManager.RegisterRoutedEvent(
            nameof(MyEvent), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(NumericControl));

        private double _oldValue;

        public event RoutedPropertyChangedEventHandler<int> MyEvent
        {
            add { AddHandler(MyEventEvent, value); }
            remove { RemoveHandler(MyEventEvent, value); }
        }

        protected virtual void OnMyEvent()
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<int>(1, 1));
        }


        #endregion


        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitializeProgressBar();
            InitializeTextBox();
            InitializeUpDownButtons();
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            HandleMouseDoubleClick(e);
            base.OnMouseDoubleClick(e);
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Focus();
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case Key.Up:
                    IncreaseSmall();
                    break;

                case Key.PageUp:
                    IncreaseLarge();
                    break;

                case Key.Down:
                    DecreaseSmall();
                    break;

                case Key.PageDown:
                    DecreaseLarge();
                    break;

                case Key.Home:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        SetToMinimum();
                    }
                    else
                    {
                        e.Handled = false;
                    }
                    break;

                case Key.End:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        SetToMaximum();
                    }
                    else
                    {
                        e.Handled = false;
                    }
                    break;

                default:
                    e.Handled = false;
                    break;
            }
            base.OnPreviewKeyDown(e);
        }
        #endregion


        #region Event Handlers
        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            DecreaseSmall();
            e.Handled = true;
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            IncreaseSmall();
            e.Handled = true;
        }

        private void progressBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragging = true;
            _oldValue = Value;
            _mouseLocation = e.GetPosition(_progressBar);
            _progressBar.CaptureMouse();
        }

        private void progressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_dragging) return;

            _progressBar.ReleaseMouseCapture();
            _dragging = false;
            var newValue = Value;
            if (Math.Abs(_oldValue - newValue) > double.Epsilon)
            {
                OnCommitValueChanged(_oldValue, newValue);
            }
        }

        private void progressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                ChangeValue(e.GetPosition(_progressBar));
            }
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NumericControlStyle != NumericControlStyle.None)
            {
                _textBox.IsHitTestVisible = false;
            }
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    UpdateValue();
                    break;
            }
        }
        #endregion


        #region Implementation
        private void ChangeValue(Point currentMouseLocation)
        {
            var pointChange = currentMouseLocation.X - _mouseLocation.X + _mouseLocation.Y - currentMouseLocation.Y;
            var oneValueChange = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)
                                     ? SmallChange : LargeChange;
            var pointsPerChange = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)
                                      ? MINIMAL_POINTS_PER_CHANGE : MAXIMAL_POINTS_PER_CHANGE;

            var valueChange = (int)(pointChange / pointsPerChange) * oneValueChange;
            if (Math.Abs(valueChange) <= double.Epsilon) return;
            var newValue = Value + valueChange;
            SetNewValue(newValue);
            _mouseLocation = currentMouseLocation;
        }

        private void DecreaseLarge() => SetNewValue(Value - LargeChange);

        private void DecreaseSmall() => SetNewValue(Value - SmallChange);

        private void FocusTextBox()
        {
            if (!_textBox.IsHitTestVisible)
            {
                _textBox.IsHitTestVisible = true;
                _textBox.Focus();
                _textBox.SelectAll();
            }
        }

        private void HandleMouseDoubleClick(MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    FocusTextBox();
                    break;
                case MouseButton.Middle:
                    break;
                case MouseButton.Right:
                    break;
                case MouseButton.XButton1:
                    break;
                case MouseButton.XButton2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void IncreaseLarge() => SetNewValue(Value + LargeChange);

        private void IncreaseSmall() => SetNewValue(Value + SmallChange);

        private void InitializeProgressBar()
        {
            _progressBar = GetTemplateChild(PROGRESS_BAR) as Control;
            if (_progressBar == null) return;

            _progressBar.MouseLeftButtonDown += progressBar_MouseLeftButtonDown;
            _progressBar.MouseMove += progressBar_MouseMove;
            _progressBar.MouseLeftButtonUp += progressBar_MouseLeftButtonUp;
        }

        private void InitializeTextBox()
        {
            _textBox = GetTemplateChild(TEXT_BOX) as TextBoxBase;
            if (_textBox == null) return;

            SetTextBinding();
            _textBox.LostFocus += textBox_LostFocus;
            _textBox.PreviewKeyDown += textBox_PreviewKeyDown;
        }

        private void InitializeUpDownButtons()
        {
            _buttonUp = GetTemplateChild(BUTTON_UP) as ButtonBase;
            if (_buttonUp != null) _buttonUp.Click += buttonUp_Click;

            _buttonDown = GetTemplateChild(BUTTON_DOWN) as ButtonBase;
            if (_buttonDown != null) _buttonDown.Click += buttonDown_Click;
        }

        protected virtual void OnCommitValueChanged(double oldValue, double newValue)
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, CommitValueChangedEvent));
        }

        private static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisElement = d as NumericControl;
            thisElement?.SetTextBinding();
        }

        private void SetNewValue(double newValue)
        {
            newValue = newValue < Minimum ? Minimum : newValue > Maximum ? Maximum : newValue;
            var oldValue = Value;
            if ((Math.Abs(newValue - oldValue) < double.Epsilon)) return;

            Value = newValue;
            if (!_dragging) OnCommitValueChanged(oldValue, newValue);
        }

        private void SetTextBinding()
        {
            if (_textBox == null) return;
            BindingOperations.ClearBinding(_textBox, TextBox.TextProperty);
            var textBinding = new Binding("Value")
            {
                ElementName = PROGRESS_BAR,
                StringFormat = StringFormat
            };
            _textBox.SetBinding(TextBox.TextProperty, textBinding);
        }

        private void SetToMaximum() => SetNewValue(Maximum);

        private void SetToMinimum() => SetNewValue(Minimum);

        private void UpdateValue()
        {
            var oldValue = Value;
            var textBindingExpression = BindingOperations.GetBindingExpression(_textBox, TextBox.TextProperty);
            textBindingExpression?.UpdateSource();
            var textBinding = BindingOperations.GetBinding(_textBox, TextBox.TextProperty);
            var newValue = Value;

            if(Math.Abs(newValue - oldValue) > double.Epsilon) OnCommitValueChanged(oldValue, newValue);
        }
        #endregion
    }

    /*[TemplatePart(Name = TEXT_BOX, Type = typeof(TextBoxBase)),
     TemplatePart(Name = PROGRESS_BAR, Type = typeof(Control)),
     TemplatePart(Name = BUTTON_UP, Type = typeof(ButtonBase)),
     TemplatePart(Name = BUTTON_DOWN, Type = typeof(ButtonBase))]
    public class NumericControl: RangeBase
    {
        #region Fields
        private const string BUTTON_DOWN = "buttonDown";
        private const string BUTTON_UP = "buttonUp";
        private const double MAXIMAL_POINTS_PER_CHANGE = 7;
        private const double MINIMAL_POINTS_PER_CHANGE = 2;
        private const string PROGRESS_BAR = "progressBar";
        private const string TEXT_BOX = "textBox";

        public static readonly DependencyProperty BarBrushProperty = DependencyProperty.Register(
            "BarBrush", typeof(Brush), typeof(NumericControl), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty NumericControlStyleProperty = DependencyProperty.Register(
            "NumericControlStyle", typeof(NumericControlStyle), typeof(NumericControl),
            new PropertyMetadata(default(NumericControlStyle)));

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            "StringFormat", typeof(string), typeof(NumericControl),
            new PropertyMetadata(default(string), OnStringFormatChanged));

        public static readonly RoutedEvent CommitValueChangedEvent = EventManager.RegisterRoutedEvent(
            "CommitValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>),
            typeof(NumericControl));

        private ButtonBase _buttonDown;
        private ButtonBase _buttonUp;
        private bool _dragging;
        private Point _mouseLocation;
        private Control _progressBar;
        private TextBoxBase _textBox;
        #endregion


        #region  Constructors & Destructor
        static NumericControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericControl),
                new FrameworkPropertyMetadata(typeof(NumericControl)));
        }
        #endregion


        #region Dependency Properties
        public Brush BarBrush
        {
            get { return (Brush)GetValue(BarBrushProperty); }
            set { SetValue(BarBrushProperty, value); }
        }

        public NumericControlStyle NumericControlStyle
        {
            get { return (NumericControlStyle)GetValue(NumericControlStyleProperty); }
            set { SetValue(NumericControlStyleProperty, value); }
        }

        public string StringFormat
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }
        #endregion


        #region Events
        public event RoutedPropertyChangedEventHandler<double> CommitValueChanged
        {
            add { AddHandler(CommitValueChangedEvent, value); }
            remove { RemoveHandler(CommitValueChangedEvent, value); }
        }
        #endregion


        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitializeProgressBar();
            InitializeTextBox();
            InitializeUpDownButtons();
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            HandleMouseDoubleClick(e);
            base.OnMouseDoubleClick(e);
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Focus();
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case Key.Up:
                    IncreaseSmall();
                    break;

                case Key.PageUp:
                    IncreaseLarge();
                    break;

                case Key.Down:
                    DecreaseSmall();
                    break;

                case Key.PageDown:
                    DecreaseLarge();
                    break;

                case Key.Home:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        SetToMinimum();
                    }
                    else
                    {
                        e.Handled = false;
                    }
                    break;

                case Key.End:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        SetToMaximum();
                    }
                    else
                    {
                        e.Handled = false;
                    }
                    break;

                default:
                    e.Handled = false;
                    break;
            }
            base.OnPreviewKeyDown(e);
        }
        #endregion


        #region Event Handlers
        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            DecreaseSmall();
            e.Handled = true;
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            IncreaseSmall();
            e.Handled = true;
        }

        private void progressBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragging = true;
            _mouseLocation = e.GetPosition(_progressBar);
            _progressBar.CaptureMouse();
        }

        private void progressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_dragging)
            {
                _dragging = false;
                _progressBar.ReleaseMouseCapture();
            }
        }

        private void progressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                ChangeValue(e.GetPosition(_progressBar));
            }
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NumericControlStyle != NumericControlStyle.None)
            {
                _textBox.IsHitTestVisible = false;
            }
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    UpdateValue();
                    break;
            }
        }
        #endregion


        #region Implementation
        private void ChangeValue(Point currentMouseLocation)
        {
            var pointChange = currentMouseLocation.X - _mouseLocation.X + _mouseLocation.Y - currentMouseLocation.Y;
            var oneValueChange = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)
                                     ? SmallChange : LargeChange;
            var pointsPerChange = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)
                                      ? MINIMAL_POINTS_PER_CHANGE : MAXIMAL_POINTS_PER_CHANGE;

            var valueChange = (int)(pointChange / pointsPerChange) * oneValueChange;
            if (Math.Abs(valueChange) <= double.Epsilon) return;
            var newValue = Value + valueChange;
            SetNewValue(newValue);
            _mouseLocation = currentMouseLocation;
        }

        private void DecreaseLarge() => SetNewValue(Value - LargeChange);

        private void DecreaseSmall() => SetNewValue(Value - SmallChange);

        private void FocusTextBox()
        {
            if (!_textBox.IsHitTestVisible)
            {
                _textBox.IsHitTestVisible = true;
                _textBox.Focus();
                _textBox.SelectAll();
            }
        }

        private void HandleMouseDoubleClick(MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    FocusTextBox();
                    break;
                case MouseButton.Middle:
                    break;
                case MouseButton.Right:
                    break;
                case MouseButton.XButton1:
                    break;
                case MouseButton.XButton2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void IncreaseLarge() => SetNewValue(Value + LargeChange);

        private void IncreaseSmall() => SetNewValue(Value + SmallChange);

        private void InitializeProgressBar()
        {
            _progressBar = GetTemplateChild(PROGRESS_BAR) as Control;
            if (_progressBar == null) return;

            _progressBar.MouseLeftButtonDown += progressBar_MouseLeftButtonDown;
            _progressBar.MouseMove += progressBar_MouseMove;
            _progressBar.MouseLeftButtonUp += progressBar_MouseLeftButtonUp;
        }

        private void InitializeTextBox()
        {
            _textBox = GetTemplateChild(TEXT_BOX) as TextBoxBase;
            if (_textBox == null) return;

            SetTextBinding();
            _textBox.LostFocus += textBox_LostFocus;
            _textBox.PreviewKeyDown += textBox_PreviewKeyDown;
        }

        private void InitializeUpDownButtons()
        {
            _buttonUp = GetTemplateChild(BUTTON_UP) as ButtonBase;
            if (_buttonUp != null) _buttonUp.Click += buttonUp_Click;

            _buttonDown = GetTemplateChild(BUTTON_DOWN) as ButtonBase;
            if (_buttonDown != null) _buttonDown.Click += buttonDown_Click;
        }

        protected virtual void OnCommitValueChanged(double oldValue, double newValue)
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, CommitValueChangedEvent));
        }

        private static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisElement = d as NumericControl;
            thisElement?.SetTextBinding();
        }

        private double SetNewValue(double newValue)
        {
            newValue = newValue < Minimum ? Minimum : newValue > Maximum ? Maximum : newValue;
            Value = newValue;
            return newValue;
        }

        private void SetTextBinding()
        {
            if (_textBox == null) return;
            BindingOperations.ClearBinding(_textBox, TextBox.TextProperty);
            var textBinding = new Binding("Value")
            {
                ElementName = PROGRESS_BAR,
                StringFormat = StringFormat
            };
            _textBox.SetBinding(TextBox.TextProperty, textBinding);
        }

        private void SetToMaximum() => SetNewValue(Maximum);

        private void SetToMinimum() => SetNewValue(Minimum);

        private void UpdateValue()
        {
            var textBindingExpression = BindingOperations.GetBindingExpression(_textBox, TextBox.TextProperty);
            textBindingExpression?.UpdateSource();
        }
        #endregion
    }*/
}