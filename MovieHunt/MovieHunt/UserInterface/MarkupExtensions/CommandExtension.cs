using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieHunt.Utility;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieHunt.UserInterface.MarkupExtensions
{
    [ContentProperty(nameof(MethodName))]
    public class CommandExtension : IMarkupExtension
    {
        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(CommandExtension), null);

        private BindableObject _targetObject;
        private MethodInvoker _methodInvoker;
        private AsyncCommand _command;
        private bool _isExecuting;

        public string MethodName { get; set; }

        public TimeSpan MinimumTimeInterval { get; set; } = TimeSpan.Zero;

        public BindableObject Target { get; set; }

        public bool DisableWhileExecuting { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(MethodName))
            {
                throw new ArgumentException(@"Name should contain valid method name.", nameof(MethodName));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentException(@"IServiceProvider is required for ActionCommand markup extension.", nameof(serviceProvider));
            }

            ExtractTarget(serviceProvider);

            _command = new AsyncCommand(RunMethodOnBindingContext, CanExecute);
            _methodInvoker = new MethodInvoker(MethodName, MinimumTimeInterval);

            return _command;
        }

        private async Task RunMethodOnBindingContext(object argument)
        {
            _isExecuting = true;
            _command.RaiseCanExecuteChanged();
            var target = Target ?? _targetObject.BindingContext;

            try
            {
                await _methodInvoker.InvokeOnObject(
                    target,
                    t => GetParameters(t, argument));
            }
            finally
            {
                _isExecuting = false;
                _command.RaiseCanExecuteChanged();
            }
        }

        private object[] GetParameters(ICollection<Type> types, object argument)
        {
            if (types.Count > 1)
            {
                throw new NotSupportedException(
                    @"CommandExtension: Can't call method with more that one parameter.");
            }

            if (types.Count == 0 && argument != null)
            {
                throw new InvalidOperationException(
                    $"CommandException: Can't pass argument of type {argument.GetType().Name} " +
                    $"to method {_methodInvoker.MethodName} since it doesn't take any arguments.");
            }

            return types.Count == 0 
                ? Array.Empty<object>() 
                : new[] { argument };
        }

        private bool CanExecute()
        {
            return !DisableWhileExecuting || !_isExecuting;
        }

        private void ExtractTarget(IServiceProvider serviceProvider)
        {
            var targetProvider = GetTargetProvider(serviceProvider);
            _targetObject = GetTargetObject(targetProvider);
        }

        private IProvideValueTarget GetTargetProvider(IServiceProvider serviceProvider)
        {
            var targetProvider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (targetProvider == null)
            {
                throw new Exception("CommandExtension: can't get IProvideValueTarget service.");
            }
            return targetProvider;
        }

        private BindableObject GetTargetObject(IProvideValueTarget targetProvider)
        {
            var target = Target ?? (BindableObject)targetProvider.TargetObject;
            if (target == null)
            {
                throw new Exception("CommandExtension: retrieved TargetObject is null.");
            }
            return target;
        }
    }
}