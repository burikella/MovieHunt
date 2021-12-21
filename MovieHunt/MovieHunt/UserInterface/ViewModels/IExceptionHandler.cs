using System;
using System.Threading.Tasks;

namespace MovieHunt.UserInterface.ViewModels
{
    internal interface IExceptionHandler
    {
        Task<bool> TryHandleException(Exception exception);
    }
}