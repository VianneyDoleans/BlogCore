using System;
using System.Runtime.Serialization;

namespace DBAccess.Exceptions
{

    [Serializable]
    public class RoleManagementException : Exception

    {
        public RoleManagementException(string message) : base(message)
        {
        }

        protected RoleManagementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
