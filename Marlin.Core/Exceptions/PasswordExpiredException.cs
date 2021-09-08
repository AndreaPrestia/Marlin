using System;
using System.Runtime.Serialization;

namespace Marlin.Core.Exceptions
{
    public class PasswordExpiredException : Exception
    {
        public PasswordExpiredException()
        {
        }

        public PasswordExpiredException(string message) : base(message)
        {
        }

        public PasswordExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PasswordExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
