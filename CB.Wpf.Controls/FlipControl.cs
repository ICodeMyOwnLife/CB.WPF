using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;


namespace CB.Wpf.Controls
{
    public class FlipControl: ContentControl
    {
        #region Fields
        private static readonly Duration _defaultDuration = TimeSpan.FromMilliseconds(125);

        public static readonly DependencyProperty CornerRadiusProperty =
            Border.CornerRadiusProperty.AddOwner(typeof(FlipControl));

        public static readonly DependencyProperty FlipDurationProperty = DependencyProperty.Register(
            "FlipDuration", typeof(Duration), typeof(FlipControl),
            new FrameworkPropertyMetadata(_defaultDuration, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                                                            FrameworkPropertyMetadataOptions.Inherits, OnDurationChanged),
            ValidateDuration);

        public static readonly DependencyProperty FlipIntervalProperty = DependencyProperty.Register(
            "FlipInterval", typeof(TimeSpan), typeof(FlipControl),
            new FrameworkPropertyMetadata(TimeSpan.FromMilliseconds(1000),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                FrameworkPropertyMetadataOptions.Inherits, OnFlipIntervalChanged, CoerceAnimationInverval),
            ValidateFlipInterval);

        public static readonly DependencyProperty FlipOrientationProperty =
            DependencyProperty.Register("FlipOrientation", typeof(Orientation), typeof(FlipControl),
                new FrameworkPropertyMetadata(Orientation.Vertical,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                    FrameworkPropertyMetadataOptions.Inherits, OnFlipOrientationChanged));

        public static readonly DependencyProperty NextContentProperty = DependencyProperty.Register(
            "NextContent", typeof(object), typeof(FlipControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ReverseDurationProperty = DependencyProperty.Register(
            "ReverseDuration", typeof(Duration), typeof(FlipControl),
            new FrameworkPropertyMetadata(_defaultDuration, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                                                            FrameworkPropertyMetadataOptions.Inherits, OnDurationChanged),
            ValidateDuration);

        public static readonly DependencyProperty StartedProperty = DependencyProperty.Register(
            "Started", typeof(bool), typeof(FlipControl),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                                                FrameworkPropertyMetadataOptions.Inherits, OnStartedChanged));

        public static readonly RoutedEvent ChangeContentEvent = EventManager.RegisterRoutedEvent("ChangeContent",
            RoutingStrategy.Bubble, typeof(ChangeContentEventHandler), typeof(FlipControl));

        public static readonly RoutedEvent FlipStartEvent = EventManager.RegisterRoutedEvent("FlipStart",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FlipControl));

        private readonly DispatcherTimer _animationTimer;

        private readonly DoubleAnimation _flipAnimation = new DoubleAnimation
        {
            Duration = TimeSpan.FromMilliseconds(125),
            From = 1,
            To = 0
        };

        private DependencyProperty _orientationProperty = ScaleTransform.ScaleYProperty;

        private readonly DoubleAnimation _reverseAnimation = new DoubleAnimation
        {
            Duration = TimeSpan.FromMilliseconds(125),
            From = 0,
            To = 1
        };

        private readonly ScaleTransform _transform = new ScaleTransform();
        #endregion


        #region Dependency Properties
        private static object CoerceAnimationInverval(DependencyObject d, object baseValue)
        {
            var fc = d as FlipControl;
            if (fc == null) throw new InvalidOperationException();

            var totalDuration = fc._flipAnimation.Duration.TimeSpan + fc._reverseAnimation.Duration.TimeSpan;
            var value = (TimeSpan)baseValue;
            return value > totalDuration ? value : totalDuration;
        }

        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fc = d as FlipControl;
            if (fc == null) return;

            var animationInterval = (TimeSpan)fc.GetValue(FlipIntervalProperty);
            var flipDuration = (Duration)fc.GetValue(FlipDurationProperty);
            var reverseDuration = (Duration)fc.GetValue(ReverseDurationProperty);
            var totalDuration = flipDuration.TimeSpan + reverseDuration.TimeSpan;
            if (totalDuration > animationInterval)
            {
                fc.SetValue(FlipIntervalProperty, totalDuration);
            }
        }

        private static void OnFlipIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fc = d as FlipControl;
            if (fc != null) fc._animationTimer.Interval = (TimeSpan)(e.NewValue);
        }

        private static void OnFlipOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fc = d as FlipControl;
            if (fc == null) throw new ArgumentNullException(nameof(fc));

            switch ((Orientation)e.NewValue)
            {
                case Orientation.Horizontal:
                    fc._orientationProperty = ScaleTransform.ScaleXProperty;
                    break;

                case Orientation.Vertical:
                    fc._orientationProperty = ScaleTransform.ScaleYProperty;
                    break;
            }
        }

        private static void OnStartedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fc = d as FlipControl;
            if (fc != null) fc._animationTimer.IsEnabled = (bool)e.NewValue;
        }
        #endregion


        #region Events
        public event EventHandler AnimationTimerTick
        {
            add { _animationTimer.Tick += value; }
            remove { _animationTimer.Tick -= value; }
        }

        public event ChangeContentEventHandler ChangeContent
        {
            add { AddHandler(ChangeContentEvent, value); }
            remove { RemoveHandler(ChangeContentEvent, value); }
        }

        public event EventHandler FlipComplete
        {
            add { _flipAnimation.Completed += value; }
            remove { _flipAnimation.Completed -= value; }
        }

        public event RoutedEventHandler FlipStart
        {
            add { AddHandler(FlipStartEvent, value); }
            remove { RemoveHandler(FlipStartEvent, value); }
        }

        public event EventHandler ReverseComplete
        {
            add { _reverseAnimation.Completed += value; }
            remove { _reverseAnimation.Completed -= value; }
        }
        #endregion


        #region Methods
        public void Animate()
        {
            OnFlipStart();
            _transform.BeginAnimation(_orientationProperty, _flipAnimation);
        }

        public void StartFlip()
        {
            SetValue(StartedProperty, true);

            //animationTimer.Start();
        }

        public void StopFlip()
        {
            SetValue(StartedProperty, false);

            //animationTimer.Stop();
        }
        #endregion


        #region Event Handlers
        private void flipAnimation_Completed(object sender, EventArgs e)
        {
            _transform.BeginAnimation(_orientationProperty, _reverseAnimation);
            Content = OnChangeContent(GetValue(ContentProperty));
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Animate();
        }
        #endregion


        #region Implementation
        protected virtual object OnChangeContent(object currentContent) // UNDONE
        {
            var nextContent = GetValue(NextContentProperty) ?? GetValue(ContentProperty);
            var e = new ChangeContentEventArgs(ChangeContentEvent, this, currentContent, nextContent);
            RaiseEvent(e);
            return e.NextContent;
        }

        protected virtual void OnFlipStart()
        {
            var e = new RoutedEventArgs(FlipStartEvent, this);
            RaiseEvent(e);
        }

        private static bool ValidateDuration(object value)
        {
            var duration = (Duration)value;
            return duration.HasTimeSpan && duration.TimeSpan >= TimeSpan.Zero;
        }

        private static bool ValidateFlipInterval(object value)
        {
            return (TimeSpan)value > TimeSpan.Zero;
        }
        #endregion


        #region Types
        public class ChangeContentEventArgs: RoutedEventArgs
        {
            #region Properties
            public object CurrentContent { get; private set; }

            public object NextContent { get; set; }
            #endregion


            #region Constructors
            public ChangeContentEventArgs(object currentContent, object nextContent)
            {
                CurrentContent = currentContent;
                NextContent = nextContent;
            }

            public ChangeContentEventArgs(RoutedEvent routedEvent, object source, object currentContent,
                object nextContent)
                : base(routedEvent, source)
            {
                CurrentContent = currentContent;
                NextContent = nextContent;
            }
            #endregion
        }

        public delegate void ChangeContentEventHandler(object sender, ChangeContentEventArgs e);
        #endregion


        #region Properties
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public Duration FlipDuration
        {
            get { return (Duration)GetValue(FlipDurationProperty); }
            set { SetValue(FlipDurationProperty, value); }
        }

        public TimeSpan FlipInterval
        {
            get { return (TimeSpan)GetValue(FlipIntervalProperty); }
            set { SetValue(FlipIntervalProperty, value); }
        }

        public Orientation FlipOrientation
        {
            get { return (Orientation)GetValue(FlipOrientationProperty); }
            set { SetValue(FlipOrientationProperty, value); }
        }

        public object NextContent
        {
            get { return GetValue(NextContentProperty); }
            set { SetValue(NextContentProperty, value); }
        }

        public Duration ReverseDuration
        {
            get { return (Duration)GetValue(ReverseDurationProperty); }
            set { SetValue(ReverseDurationProperty, value); }
        }

        public bool Started
        {
            get { return (bool)GetValue(StartedProperty); }
            set { SetValue(StartedProperty, value); }
        }
        #endregion


        #region Constructors
        static FlipControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipControl),
                new FrameworkPropertyMetadata(typeof(FlipControl)));
        }

        public FlipControl()
        {
            RenderTransform = _transform;
            RenderTransformOrigin = new Point(0.5, 0.5);
            _animationTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(1000), DispatcherPriority.Normal, timer_Tick,
                Dispatcher);

            var flipDurationBinding = new Binding("FlipDuration")
            {
                Mode = BindingMode.TwoWay,
                Source = this
            };
            BindingOperations.SetBinding(_flipAnimation, Timeline.DurationProperty, flipDurationBinding);

            var reverseDurationBinding = new Binding("ReverseDuration")
            {
                Mode = BindingMode.TwoWay,
                Source = this
            };
            BindingOperations.SetBinding(_reverseAnimation, Timeline.DurationProperty, reverseDurationBinding);

            _flipAnimation.Completed += flipAnimation_Completed;
        }
        #endregion
    }
}