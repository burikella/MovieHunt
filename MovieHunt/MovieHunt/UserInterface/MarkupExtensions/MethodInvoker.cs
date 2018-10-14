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
        private object lastTarget;
        private DateTime lastCallTime;
        private MethodInfoWrapper actionMethod;

        public MethodInvoker(string methodName, TimeSpan minimumInterval)
        {
            MethodName = methodName;
            MinimumTimeInterval = minimumInterval;
        }


        public MethodInvoker(string methodName)
            : this(methodName, TimeSpan.Zero)
        {
        }

        public string MethodName { get; set; }

        public TimeSpan MinimumTimeInterval { get; set; }

        public async Task InvokeOnObject(object target, params object[] arguments)
        {
            var now = DateTime.Now;
            if (now - lastCallTime < MinimumTimeInterval)
            {
                return;
            }

            lastCallTime = now;

            ActualizeMethodInfo(target);

            if (!actionMethod.MethodInfo.GetParameters().Any() && arguments.All(a => a == null))
            {
                arguments = Enumerable.Empty<object>().ToArray();
            }

            await InvokeMethod(arguments ?? Enumerable.Empty<object>());
        }

        private void ActualizeMethodInfo(object newContext)
        {
            if (newContext == null)
            {
                throw new Exception("CommandExtension: binding context is null.");
            }

            if (!ReferenceEquals(lastTarget, newContext))
            {
                lastTarget = newContext;
                GetMethodInfo();
            }
        }

        private async Task InvokeMethod(IEnumerable<object> arguments)
        {
            if (actionMethod.IsTask)
            {
                await actionMethod.InvokeAsync(arguments.ToArray()).ConfigureAwait(false);
            }
            else
            {
                actionMethod.Invoke(arguments.ToArray());
            }
        }

        private void GetMethodInfo(Type type = null)
        {
            var typeInfo = (type ?? lastTarget.GetType()).GetTypeInfo();
            var methodInfo = typeInfo.DeclaredMethods.SingleOrDefault(m => m.Name.Equals(MethodName) && m.IsPublic);
            if (methodInfo == null)
            {
                if (typeInfo.BaseType == typeof(object))
                {
                    string message = $"CommandExtension: can't find method {MethodName} on {lastTarget} binding context.";
                    throw new Exception(message);
                }
                GetMethodInfo(typeInfo.BaseType);
                return;
            }
            actionMethod = new MethodInfoWrapper(methodInfo, lastTarget);
        }

        private class MethodInfoWrapper
        {
            private readonly object owner;

            public MethodInfoWrapper(MethodInfo methodInfo, object owner)
            {
                this.owner = owner;
                MethodInfo = methodInfo;
                IsTask = MethodInfo.ReturnType.IsAssignableTo(typeof(Task));
            }

            public MethodInfo MethodInfo { get; }

            public bool IsTask { get; }

            public void Invoke(object[] arguments)
            {
                MethodInfo.Invoke(owner, arguments);
            }

            public Task InvokeAsync(object[] arguments)
            {
                return (Task)MethodInfo.Invoke(owner, arguments);
            }
        }
    }
}