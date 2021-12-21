using System;

namespace MovieHunt.MovieDb.Api
{
    [Serializable]
    public class NetworkProblemException : Exception
    {
        private const string DefaultMessage = @"Web request was failed due network issues.";

        public bool ConnectionAvailable { get; }

        public NetworkProblemException(bool connectionAvailable)
            : this(connectionAvailable, null)
        {
        }

        public NetworkProblemException(bool connectionAvailable, Exception innerException)
            : base(DefaultMessage, innerException)
        {
            ConnectionAvailable = connectionAvailable;
        }
    }
}