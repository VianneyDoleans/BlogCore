using System;
using System.Runtime.Serialization;

namespace DBAccess.Exceptions
{
    [Serializable]
    public class DatabaseProviderException : Exception

    {
        public DatabaseProviderException(string message) : base(message)
        {
        }

        protected DatabaseProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
