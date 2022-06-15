using System;
using System.Runtime.Serialization;

namespace DBAccess.Exceptions
{

    [Serializable]
    public class UserManagementException : Exception

    {
        public UserManagementException(string message) : base(message)
        {
        }

        protected UserManagementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
