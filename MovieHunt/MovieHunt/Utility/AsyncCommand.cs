using System;
using System.Threading.Tasks;
using Prism.Commands;

namespace MovieHunt.Utility
{
    public class AsyncCommand : DelegateCommand
    {
        private readonly Func<Task> _asyncMethod;
        private readonly Func<object, Task> _asyncMethodWithParameter;
        
        public AsyncCommand(Func<Task> executeAsyncMethod, Func<bool> canExecuteMethod)
            : base(() => { }, canExecuteMethod)
        {
            _asyncMethod = executeAsyncMethod;
        }

        public AsyncCommand(Func<object, Task> executeAsyncMethod, Func<bool> canExecuteMethod)
            : base(() => { }, canExecuteMethod)
        {
            _asyncMethodWithParameter = executeAsyncMethod;
        }

        protected override async void Execute(object parameter)
        {
            if (_asyncMethod != null)
            {
                await _asyncMethod().ConfigureAwait(false);
            }
            else if (_asyncMethodWithParameter != null)
            {
                await _asyncMethodWithParameter(parameter).ConfigureAwait(false);
            }
            else
            {
                base.Execute(parameter);
            }
        }
    }
}