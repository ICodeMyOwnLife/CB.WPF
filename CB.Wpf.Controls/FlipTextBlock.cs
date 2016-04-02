using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;


namespace CB.Wpf.Controls
{
    public class FlipTextBlock : TextBlock
    {
        #region Types
        public class ChangeTextEventArgs : RoutedEventArgs
        {
            #region Properties
            public string CurrentText { get; private set; }

            public string NextText { get; set; }
            #endregion


            #region Constructors
            public ChangeTextEventArgs(string currentText, string nextText)
            {
                CurrentText = currentText;
                NextText = nextText;
            }

            public ChangeTextEventArgs(RoutedEvent routedEvent, object source, string currentText, string nextText)
                : base(routedEvent, source)
            {
                CurrentText = currentText;
                NextText = nextText;
            }
            #endregion
        }


        public delegate void ChangeTextEventHandler(object sender, ChangeTextEventArgs e);
        #endregion


        #region Fields
        private static readonly Duration defaultDuration = TimeSpan.FromMilliseconds(125);
        private readonly ScaleTransform transform = new ScaleTransform();
        private readonly DispatcherTimer animationTimer;
        private DependencyProperty orientationProperty = ScaleTransform.ScaleYProperty;

        private readonly DoubleAnimation flipAnimation = new DoubleAnimation
        {
            Duration = TimeSpan.FromMilliseconds(125),
            From = 1,
            To = 0
        };

        private readonly DoubleAnimation reverseAnimation = new DoubleAnimation
        {
            Duration = TimeSpan.FromMilliseconds(125),
            From = 0,
            To = 1
        };
        #endregion


        #region Properties
        public Duration FlipDuration
        {
            get { return (Duration) GetValue(FlipDurationProperty); }
            set { SetValue(FlipDurationProperty, value); }
        }

        public static readonly DependencyProperty FlipDurationProperty = DependencyProperty.Register(
            "FlipDuration", typeof (Duration), typeof (FlipTextBlock),
            new FrameworkPropertyMetadata(defaultDuration, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                                                           FrameworkPropertyMetadataOptions.Inherits, OnDurationChanged),
            ValidateDuration);

        public TimeSpan FlipInterval
        {
            get { return (TimeSpan) GetValue(FlipIntervalProperty); }
            set { SetValue(FlipIntervalProperty, value); }
        }

        public static readonly DependencyProperty FlipIntervalProperty = DependencyProperty.Register(
            "FlipInterval", typeof (TimeSpan), typeof (FlipTextBlock),
            new FrameworkPropertyMetadata(TimeSpan.FromMilliseconds(1000),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                FrameworkPropertyMetadataOptions.Inherits, OnFlipIntervalChanged, CoerceAnimationInverval),
            ValidateFlipInterval);

        public Orientation FlipOrientation
        {
            get { return (Orientation) GetValue(FlipOrientationProperty); }
            set { SetValue(FlipOrientationProperty, value); }
        }

        public static readonly DependencyProperty FlipOrientationProperty =
            DependencyProperty.Register("FlipOrientation", typeof (Orientation), typeof (FlipTextBlock),
                new FrameworkPropertyMetadata(Orientation.Vertical,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                    FrameworkPropertyMetadataOptions.Inherits, OnFlipOrientationChanged));

        public string NextText
        {
            get { return (string) GetValue(NextTextProperty); }
            set { SetValue(NextTextProperty, value); }
        }

        public static readonly DependencyProperty NextTextProperty = DependencyProperty.Register(
            "NextText", typeof (string), typeof (FlipTextBlock), new PropertyMetadata(null));

        public Duration ReverseDuration
        {
            get { return (Duration) GetValue(ReverseDurationProperty); }
            set { SetValue(ReverseDurationProperty, value); }
        }

        public static readonly DependencyProperty ReverseDurationProperty = DependencyProperty.Register(
            "ReverseDuration", typeof (Duration), typeof (FlipTextBlock),
            new FrameworkPropertyMetadata(defaultDuration, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                                                           FrameworkPropertyMetadataOptions.Inherits, OnDurationChanged),
            ValidateDuration);

        public bool Started
        {
            get { return (bool) GetValue(StartedProperty); }
            set { SetValue(StartedProperty, value); }
        }

        public static readonly DependencyProperty StartedProperty = DependencyProperty.Register(
            "Started", typeof (bool), typeof (FlipTextBlock),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                                                FrameworkPropertyMetadataOptions.Inherits, OnStartedChanged));
        #endregion


        #region Constructors
        static FlipTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (FlipTextBlock),
                new FrameworkPropertyMetadata(typeof (FlipTextBlock)));
        }

        public FlipTextBlock()
        {
            RenderTransform = transform;
            RenderTransformOrigin = new Point(0.5, 0.5);
            animationTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(1000), DispatcherPriority.Normal, timer_Tick,
                Dispatcher);

            var flipDurationBinding = new Binding("FlipDuration")
            {
                Mode = BindingMode.TwoWay,
                Source = this
            };
            BindingOperations.SetBinding(flipAnimation, Timeline.DurationProperty, flipDurationBinding);

            var reverseDurationBinding = new Binding("ReverseDuration")
            {
                Mode = BindingMode.TwoWay,
                Source = this
            };
            BindingOperations.SetBinding(reverseAnimation, Timeline.DurationProperty, reverseDurationBinding);

            flipAnimation.Completed += flipAnimation_Completed;
        }
        #endregion


        #region Events
        public event EventHandler AnimationTimerTick
        {
            add { animationTimer.Tick += value; }
            remove { animationTimer.Tick -= value; }
        }

        public event ChangeTextEventHandler ChangeText
        {
            add { AddHandler(ChangeTextEvent, value); }
            remove { RemoveHandler(ChangeTextEvent, value); }
        }

        public static readonly RoutedEvent ChangeTextEvent = EventManager.RegisterRoutedEvent("ChangeText",
            RoutingStrategy.Bubble, typeof (ChangeTextEventHandler), typeof (FlipTextBlock));

        public event EventHandler FlipComplete
        {
            add { flipAnimation.Completed += value; }
            remove { flipAnimation.Completed -= value; }
        }

        public event RoutedEventHandler FlipStart
        {
            add { AddHandler(FlipStartEvent, value); }
            remove { RemoveHandler(FlipStartEvent, value); }
        }

        public static readonly RoutedEvent FlipStartEvent = EventManager.RegisterRoutedEvent("FlipStart",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (FlipTextBlock));

        public event EventHandler ReverseComplete
        {
            add { reverseAnimation.Completed += value; }
            remove { reverseAnimation.Completed -= value; }
        }
        #endregion


        #region Methods
        public void Animate()
        {
            OnFlipStart();
            transform.BeginAnimation(orientationProperty, flipAnimation);
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
            transform.BeginAnimation(orientationProperty, reverseAnimation);
            Text = OnChangeText(GetValue(TextProperty) as string);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Animate();
        }
        #endregion


        #region Implementation
        private static object CoerceAnimationInverval(DependencyObject d, object baseValue)
        {
            var ftb = d as FlipTextBlock;
            var totalDuration = ftb.flipAnimation.Duration.TimeSpan + ftb.reverseAnimation.Duration.TimeSpan;
            var value = (TimeSpan) baseValue;
            return value > totalDuration ? value : totalDuration;
        }

        protected virtual string OnChangeText(string currentText)
        {
            var nextText = GetValue(NextTextProperty) as string ?? GetValue(TextProperty) as string;
            var e = new ChangeTextEventArgs(ChangeTextEvent, this, currentText, nextText);
            RaiseEvent(e);
            return e.NextText;
        }

        private static void OnFlipIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ftb = d as FlipTextBlock;
            ftb.animationTimer.Interval = (TimeSpan) (e.NewValue);
        }

        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ftb = d as FlipTextBlock;
            var animationInterval = (TimeSpan) ftb.GetValue(FlipIntervalProperty);
            var flipDuration = (Duration) ftb.GetValue(FlipDurationProperty);
            var reverseDuration = (Duration) ftb.GetValue(ReverseDurationProperty);
            var totalDuration = flipDuration.TimeSpan + reverseDuration.TimeSpan;
            if (totalDuration > animationInterval)
            {
                ftb.SetValue(FlipIntervalProperty, totalDuration);
            }
        }

        private static void OnFlipOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ftb = d as FlipTextBlock;
            switch ((Orientation) e.NewValue)
            {
                case Orientation.Horizontal:
                    ftb.orientationProperty = ScaleTransform.ScaleXProperty;
                    break;
                case Orientation.Vertical:
                    ftb.orientationProperty = ScaleTransform.ScaleYProperty;
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnFlipStart()
        {
            var e = new RoutedEventArgs(FlipStartEvent, this);
            RaiseEvent(e);
        }

        private static void OnStartedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ftb = d as FlipTextBlock;
            ftb.animationTimer.IsEnabled = (bool) e.NewValue;
        }

        private static bool ValidateDuration(object value)
        {
            var duration = (Duration) value;
            return duration.HasTimeSpan && duration.TimeSpan >= TimeSpan.Zero;
        }

        private static bool ValidateFlipInterval(object value)
        {
            return (TimeSpan) value > TimeSpan.Zero;
        }
        #endregion
    }
}