using System.Runtime.Serialization;

namespace SvxlinkManager.Application
{
    [Serializable]
    public class SvxlinkManagerException : Exception
    {
        public SvxlinkManagerException()
        {
        }

        public SvxlinkManagerException(string? message) : base(message)
        {
        }

        public SvxlinkManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SvxlinkManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}