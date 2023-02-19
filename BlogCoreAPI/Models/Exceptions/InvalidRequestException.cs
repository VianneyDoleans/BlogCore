using System;
using System.Runtime.Serialization;

namespace BlogCoreAPI.Models.Exceptions
{
    [Serializable]
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException(string message) : base(message)
        {
        }

        protected InvalidRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
