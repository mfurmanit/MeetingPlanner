using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

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
