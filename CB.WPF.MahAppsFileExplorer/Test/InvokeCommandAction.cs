using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Prism.Interactivity;


namespace CB.WPF.MahAppsFileExplorer.Test
{
    public class InvokeCommandAction: TriggerAction<UIElement>
    {
        #region Fields
        private ExecutableCommandBehavior _commandBehavior;
        #endregion


        #region Dependency Properties
        /// <summary>
        ///     Dependency property identifying if the associated element should automaticlaly be enabled or disabled based on the
        ///     result of the Command's CanExecute
        /// </summary>
        public static readonly DependencyProperty AutoEnableProperty =
            DependencyProperty.Register("AutoEnable", typeof(bool), typeof(InvokeCommandAction),
                new PropertyMetadata(true, (d, e) => ((InvokeCommandAction)d).OnAllowDisableChanged((bool)e.NewValue)));

        /// <summary>
        ///     Gets or sets whther or not the associated element will automatically be enabled or disabled based on the result of
        ///     the commands CanExecute
        /// </summary>
        public bool AutoEnable
        {
            get { return (bool)GetValue(AutoEnableProperty); }
            set { SetValue(AutoEnableProperty, value); }
        }

        /// <summary>
        ///     Dependency property identifying the command to execute when invoked.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction),
                new PropertyMetadata(null, (d, e) => ((InvokeCommandAction)d).OnCommandChanged((ICommand)e.NewValue)));

        /// <summary>
        ///     Gets or sets the command to execute when invoked.
        /// </summary>
        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        ///     Dependency property identifying the command parameter to supply on command execution.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction),
                new PropertyMetadata(null, (d, e) => ((InvokeCommandAction)d).OnCommandParameterChanged(e.NewValue)));

        /// <summary>
        ///     Gets or sets the command parameter to supply on command execution.
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        ///     Dependency property identifying the TriggerParameterPath to be parsed to identify the child property of the trigger
        ///     parameter to be used as the command parameter.
        /// </summary>
        public static readonly DependencyProperty TriggerParameterPathProperty =
            DependencyProperty.Register("TriggerParameterPath", typeof(string), typeof(InvokeCommandAction),
                new PropertyMetadata(null, (d, e) => { }));

        /// <summary>
        ///     Gets or sets the TriggerParameterPath value.
        /// </summary>
        public string TriggerParameterPath
        {
            get { return GetValue(TriggerParameterPathProperty) as string; }
            set { SetValue(TriggerParameterPathProperty, value); }
        }
        #endregion


        #region Methods
        /// <summary>
        ///     Public wrapper of the Invoke method.
        /// </summary>
        public void InvokeAction(object parameter)
        {
            Invoke(parameter);
        }
        #endregion


        #region Override
        /// <summary>
        ///     Executes the command
        /// </summary>
        /// <param name="parameter">
        ///     This parameter is passed to the command; the CommandParameter specified in the
        ///     CommandParameterProperty is used for command invocation if not null.
        /// </param>
        /*protected override void Invoke(object parameter)
        {
            if (!string.IsNullOrEmpty(TriggerParameterPath))
            {
                //Walk the ParameterPath for nested properties.
                var propertyPathParts = TriggerParameterPath.Split('.');
                var propertyValue = parameter;
                foreach (var propertyPathPart in propertyPathParts)
                {
                    var propInfo = propertyValue.GetType().GetTypeInfo().GetProperty(propertyPathPart);
                    propertyValue = propInfo.GetValue(propertyValue);
                }
                parameter = propertyValue;
            }

            var behavior = GetOrCreateBehavior();
            behavior?.ExecuteCommand(parameter);
        }*/

        protected override void Invoke(object parameter)
        {
            if (!string.IsNullOrEmpty(this.TriggerParameterPath))
            {
                var strArray = TriggerParameterPath.Split('.');
                var obj = strArray.Aggregate(parameter, (current, name) => current.GetType().GetTypeInfo().GetProperty(name).GetValue(current));
                parameter = obj;
            }

            var orCreateBehavior = GetOrCreateBehavior();
            orCreateBehavior?.ExecuteCommand(parameter);
        }

        /// <summary>
        ///     This method is called after the behavior is attached.
        ///     It updates the command behavior's Command and CommandParameter properties if necessary.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            // In case this action is attached to a target object after the Command and/or CommandParameter properties are set,
            // the command behavior would be created without a value for these properties.
            // To cover this scenario, the Command and CommandParameter properties of the behavior are updated here.
            var behavior = GetOrCreateBehavior();

            behavior.AutoEnable = AutoEnable;

            if (behavior.Command != Command)
                behavior.Command = Command;

            if (behavior.CommandParameter != CommandParameter)
                behavior.CommandParameter = CommandParameter;
        }

        /// <summary>
        ///     Sets the Command and CommandParameter properties to null.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            Command = null;
            CommandParameter = null;

            _commandBehavior = null;
        }
        #endregion


        #region Implementation
        private ExecutableCommandBehavior GetOrCreateBehavior()
        {
            // In case this method is called prior to this action being attached, 
            // the CommandBehavior would always keep a null target object (which isn't changeable afterwards).
            // Therefore, in that case the behavior shouldn't be created and this method should return null.
            if (_commandBehavior == null && AssociatedObject != null)
            {
                _commandBehavior = new ExecutableCommandBehavior(AssociatedObject);
            }

            return _commandBehavior;
        }

        private void OnAllowDisableChanged(bool newValue)
        {
            var behavior = GetOrCreateBehavior();
            if (behavior != null)
                behavior.AutoEnable = newValue;
        }

        private void OnCommandChanged(ICommand newValue)
        {
            var behavior = GetOrCreateBehavior();
            if (behavior != null)
                behavior.Command = newValue;
        }

        private void OnCommandParameterChanged(object newValue)
        {
            var behavior = GetOrCreateBehavior();
            if (behavior != null)
                behavior.CommandParameter = newValue;
        }
        #endregion


        /// <summary>
        ///     A CommandBehavior that exposes a public ExecuteCommand method. It provides the functionality to invoke commands and
        ///     update Enabled state of the target control.
        ///     It is not possible to make the <see cref="InvokeCommandAction" /> inherit from
        ///     <see cref="CommandBehaviorBase{T}" />, since the <see cref="InvokeCommandAction" />
        ///     must already inherit from <see cref="TriggerAction{T}" />, so we chose to follow the aggregation approach.
        /// </summary>
        private class ExecutableCommandBehavior: CommandBehaviorBase<UIElement>
        {
            #region  Constructors & Destructor
            /// <summary>
            ///     Constructor specifying the target object.
            /// </summary>
            /// <param name="target">The target object the behavior is attached to.</param>
            public ExecutableCommandBehavior(UIElement target)
                : base(target) { }
            #endregion


            #region Methods
            /// <summary>
            ///     Executes the command, if it's set.
            /// </summary>
            public new void ExecuteCommand(object parameter)
            {
                base.ExecuteCommand(parameter);
            }
            #endregion
        }
    }
}