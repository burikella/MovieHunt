using System;
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
                throw new ArgumentException("Name should contain valid method name.", nameof(MethodName));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentException("IServiceProvider is required for ActionCommand markup extension.", nameof(serviceProvider));
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
            try
            {
                await _methodInvoker.InvokeOnObject(Target ?? _targetObject.BindingContext, argument);
            }
            finally
            {
                _isExecuting = false;
                _command.RaiseCanExecuteChanged();
            }
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