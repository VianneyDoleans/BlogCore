using System;
using System.Runtime.Serialization;

namespace DBAccess.Exceptions
{
    [Serializable]
    public class ResourceNotFoundException : Exception

    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
