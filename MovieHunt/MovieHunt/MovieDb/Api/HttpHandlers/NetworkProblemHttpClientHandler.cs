using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieHunt.MovieDb.Api
{
    /// <summary>
    /// This handler should intercept exceptions, figure out whether they're related to network issues,
    /// and in case they are, wrap exception in <see cref="NetworkProblemException"></see>.
    /// </summary>
    internal class NetworkProblemHttpClientHandler : DelegatingHandler
    {
        public NetworkProblemHttpClientHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception) when (IsBadNetworkException(exception))
            {
                throw new NetworkProblemException(@"Web request was failed due network issues.", exception);
            }
        }

        private static bool IsBadNetworkException(Exception exception)
        {
            return exception is WebException || IsJavaUnknownHostException(exception) ||
                   IsCancelledWithoutCancellationRequest(exception);
        }

        private static bool IsJavaUnknownHostException(Exception exception)
        {
            return exception?.GetType().ToString() == "Java.Net.UnknownHostException";
        }

        private static bool IsCancelledWithoutCancellationRequest(Exception exception)
        {
            return exception is OperationCanceledException cancelledException
                   && !cancelledException.CancellationToken.IsCancellationRequested;
        }
    }
}