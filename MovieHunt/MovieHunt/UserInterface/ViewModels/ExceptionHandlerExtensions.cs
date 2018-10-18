using System;
using System.Threading.Tasks;

namespace MovieHunt.UserInterface.ViewModels
{
    internal static class ExceptionHandlerExtensions
    {
        public static async Task RunWithExceptionHandling(this IExceptionHandler exceptionHandler, Task task)
        {
            try
            {
                await task;
            }
            catch (Exception exception)
            {
                bool isHandled = await exceptionHandler.TryHandleException(exception);
                if (!isHandled)
                {
                    throw;
                }
            }
        }
    }
}