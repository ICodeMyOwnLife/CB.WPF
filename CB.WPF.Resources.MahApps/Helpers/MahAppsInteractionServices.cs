using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using CB.Prism.Interactivity;
using CB.WPF.MahAppsResources.Windows;
using Prism.Interactivity.InteractionRequest;
using TriggerBase = System.Windows.Interactivity.TriggerBase;


namespace CB.WPF.MahAppsResources
{
    public class MahAppsInteractionServices
    {
        #region Dependency Properties
        private static void RegisterRequestManager(DependencyObject obj, RequestManager requestManager)
        {
            var triggers = Interaction.GetTriggers(obj);
            triggers.Add(CreateConfirmTrigger(requestManager));
            triggers.Add(CreateNotifyTrigger(requestManager));
            triggers.Add(CreateFileTrigger(requestManager));
            triggers.Add(new WindowRequestTrigger { SourceObject = requestManager.WindowRequestProvider.Request });
        }

        public static readonly DependencyProperty RequestManagerProperty = DependencyProperty.RegisterAttached(
            "RequestManager", typeof(RequestManager), typeof(MahAppsInteractionServices),
            new PropertyMetadata(default(RequestManager), OnRequestManagerChanged));

        [Category("MahAppsWindowsServices")]
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static RequestManager GetRequestManager(DependencyObject d)
            => (RequestManager)d.GetValue(RequestManagerProperty);

        [Category("MahAppsWindowsServices")]
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static void SetRequestManager(DependencyObject d, RequestManager value)
            => d.SetValue(RequestManagerProperty, value);

        private static void UnRegisterRequestManager(DependencyObject obj, RequestManager requestManager)
        {
            if (requestManager == null) return;

            var triggers = Interaction.GetTriggers(obj);
            var removedTriggers = triggers.OfType<EventTriggerBase>().Where(
                eventTrigger => eventTrigger.SourceObject == requestManager.ConfirmRequestProvider.Request ||
                                eventTrigger.SourceObject == requestManager.FileRequestProvider.Request ||
                                eventTrigger.SourceObject == requestManager.NotifyRequestProvider.Request ||
                                eventTrigger.SourceObject == requestManager.WindowRequestProvider.Request).ToArray();

            foreach (var eventTrigger in removedTriggers) triggers.Remove(eventTrigger);
        }
        #endregion


        #region Implementation
        private static TriggerBase CreateConfirmTrigger(RequestManager requestManager)
        {
            var confirmTrigger = new InteractionRequestTrigger
            {
                SourceObject = requestManager.ConfirmRequestProvider.Request
            };
            confirmTrigger.Actions.Add(CreateWindowTriggerAction(typeof(MahAppsConfirmWindow)));
            return confirmTrigger;
        }

        private static TriggerBase CreateFileTrigger(RequestManager requestManager)
        {
            var fileTrigger = new InteractionRequestTrigger
            {
                SourceObject = requestManager.FileRequestProvider.Request
            };
            fileTrigger.Actions.Add(CreateWindowTriggerAction(null));
            return fileTrigger;
        }

        private static TriggerBase CreateNotifyTrigger(RequestManager requestManager)
        {
            var notifyTrigger = new InteractionRequestTrigger
            {
                SourceObject = requestManager.NotifyRequestProvider.Request
            };
            notifyTrigger.Actions.Add(CreateWindowTriggerAction(typeof(MahAppsNotifyWindow)));
            return notifyTrigger;
        }

        private static WindowTriggerAction CreateWindowTriggerAction(Type windowType)
        {
            return new WindowTriggerAction
            {
                CenterOverAssociatedObject = true,
                IsModal = true,
                WindowType = windowType
            };
        }

        private static void OnRequestManagerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UnRegisterRequestManager(d, (RequestManager)e.OldValue);
            RegisterRequestManager(d, (RequestManager)e.NewValue);
        }
        #endregion
    }
}