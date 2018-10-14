using System;
using System.Threading.Tasks;
using Prism.Commands;

namespace MovieHunt.Extensions
{
    public class AsyncCommand : DelegateCommand
    {
        private readonly Func<Task> _asyncMethod;
        private readonly Func<object, Task> _asyncMethodWithParameter;

        public AsyncCommand(Func<Task> executeAsynMethod)
            : this(executeAsynMethod, () => true)
        {
        }

        public AsyncCommand(Func<object, Task> executeAsynMethod)
            : this(executeAsynMethod, () => true)
        {
        }

        public AsyncCommand(Func<Task> executeAsynMethod, Func<bool> canExecuteMethod)
            : base(() => { }, canExecuteMethod)
        {
            _asyncMethod = executeAsynMethod;
        }

        public AsyncCommand(Func<object, Task> executeAsynMethod, Func<bool> canExecuteMethod)
            : base(() => { }, canExecuteMethod)
        {
            _asyncMethodWithParameter = executeAsynMethod;
        }

        public AsyncCommand(Action executeMethod) : base(executeMethod)
        {
        }

        public AsyncCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {
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