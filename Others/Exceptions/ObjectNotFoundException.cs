using System;

namespace MeetingPlanner.Others.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException()
        { }

        public ObjectNotFoundException(string message)
            : base(message)
        { }

        public ObjectNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
