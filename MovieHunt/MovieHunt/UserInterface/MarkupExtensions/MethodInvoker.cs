using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DryIoc;

namespace MovieHunt.UserInterface.MarkupExtensions
{
    public class MethodInvoker
    {
        private object _lastTarget;
        private DateTime _lastCallTime;
        private MethodInfoWrapper _actionMethod;

        public MethodInvoker(string methodName, TimeSpan minimumInterval)
        {
            MethodName = methodName;
            MinimumTimeInterval = minimumInterval;
        }


        public MethodInvoker(string methodName)
            : this(methodName, TimeSpan.Zero)
        {
        }

        public string MethodName { get; }

        public TimeSpan MinimumTimeInterval { get; set; }

        public async Task InvokeOnObject(object target, Func<IList<Type>, object[]> argumentsProvider)
        {
            var now = DateTime.Now;
            if (now - _lastCallTime < MinimumTimeInterval)
            {
                return;
            }

            _lastCallTime = now;

            ActualizeMethodInfo(target);

            var parameterTypes = _actionMethod
                .MethodInfo
                .GetParameters()
                .Select(p => p.ParameterType)
                .ToList();

            var arguments = argumentsProvider?.Invoke(parameterTypes) ?? new object[0];

            await InvokeMethod(arguments);
        }

        private void ActualizeMethodInfo(object newContext)
        {
            if (newContext == null)
            {
                throw new ArgumentNullException(
                    nameof(newContext),
                    @"CommandExtension: binding context is null.");
            }

            if (!ReferenceEquals(_lastTarget, newContext))
            {
                _lastTarget = newContext;
                GetMethodInfo();
            }
        }

        private async Task InvokeMethod(IEnumerable<object> arguments)
        {
            if (_actionMethod.IsTask)
            {
                await _actionMethod.InvokeAsync(arguments.ToArray()).ConfigureAwait(false);
            }
            else
            {
                _actionMethod.Invoke(arguments.ToArray());
            }
        }

        private void GetMethodInfo(Type type = null)
        {
            var typeInfo = (type ?? _lastTarget.GetType()).GetTypeInfo();
            var methodInfo = typeInfo.DeclaredMethods.SingleOrDefault(m => m.Name.Equals(MethodName) && m.IsPublic);

            if (methodInfo == null)
            {
                if (typeInfo.BaseType == typeof(object))
                {
                    string message = $"CommandExtension: can't find method {MethodName} on {_lastTarget} binding context.";
                    throw new ArgumentException(message);
                }
                GetMethodInfo(typeInfo.BaseType);
                return;
            }

            _actionMethod = new MethodInfoWrapper(methodInfo, _lastTarget);
        }

        private class MethodInfoWrapper
        {
            private readonly object _owner;

            public MethodInfoWrapper(MethodInfo methodInfo, object owner)
            {
                this._owner = owner;
                MethodInfo = methodInfo;
                IsTask = MethodInfo.ReturnType.IsAssignableTo(typeof(Task));
            }

            public MethodInfo MethodInfo { get; }

            public bool IsTask { get; }

            public void Invoke(object[] arguments)
            {
                MethodInfo.Invoke(_owner, arguments);
            }

            public Task InvokeAsync(object[] arguments)
            {
                return (Task)MethodInfo.Invoke(_owner, arguments);
            }
        }
    }
}