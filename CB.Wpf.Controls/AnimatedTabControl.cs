using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace CB.Wpf.Controls
{
    public class AnimatedTabControl: TabControl
    {
        #region Fields
        public static readonly RoutedEvent SelectionChangingEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanging", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AnimatedTabControl));

        private DispatcherTimer _timer;
        #endregion


        #region  Constructors & Destructor
        static AnimatedTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedTabControl), new FrameworkPropertyMetadata(typeof(AnimatedTabControl)));
        }
        #endregion


        #region Events
        public event RoutedEventHandler SelectionChanging
        {
            add { AddHandler(SelectionChangingEvent, value); }
            remove { RemoveHandler(SelectionChangingEvent, value); }
        }
        #endregion


        #region Override
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(
                (Action)delegate
                {
                    RaiseSelectionChangingEvent();

                    StopTimer();

                    _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 500) };

                    _timer.Tick += (sender, args) =>
                    {
                        StopTimer();
                        base.OnSelectionChanged(e);
                    };
                    _timer.Start();
                });
        }
        #endregion


        #region Implementation

        // This method raises the Tap event
        private void RaiseSelectionChangingEvent()
        {
            var args = new RoutedEventArgs(SelectionChangingEvent);
            RaiseEvent(args);
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }
        #endregion
    }
}