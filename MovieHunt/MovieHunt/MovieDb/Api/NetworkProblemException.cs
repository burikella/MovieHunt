using System;
using System.Runtime.Serialization;

namespace MovieHunt.MovieDb.Api
{
    [Serializable]
    public class NetworkProblemException : Exception
    {
        public NetworkProblemException()
        {
        }

        public NetworkProblemException(string message) : base(message)
        {
        }

        public NetworkProblemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NetworkProblemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}