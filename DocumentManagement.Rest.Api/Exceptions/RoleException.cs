using System.Runtime.Serialization;

namespace DocumentManagement.Rest.Api.Exceptions
{
    public class RoleException : Exception
    {
        public RoleException()
        {
        }

        public RoleException(string? message) : base(message)
        {
        }

        public RoleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RoleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
