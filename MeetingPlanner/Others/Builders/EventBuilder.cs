using System;
using MeetingPlanner.Models;

namespace MeetingPlanner.Others.Builders
{
    public class EventBuilder
    {
        private Event _event;

        private EventBuilder()
        {
            _event = new Event();
        }

        public static EventBuilder Start()
        {
            return new EventBuilder();
        }

        public EventBuilder Date(DateTime dateTime)
        {
            _event.Date = dateTime;
            return this;
        }

        public EventBuilder Title(string title)
        {
            _event.Title = title;
            return this;
        }

        public EventBuilder HourFrom(string hourFrom)
        {
            _event.HourFrom = hourFrom;
            return this;
        }

        public EventBuilder HourTo(string hourTo)
        {
            _event.HourTo = hourTo;
            return this;
        }

        public EventBuilder Global(bool global)
        {
            _event.Global = global;
            return this;
        }

        public EventBuilder Description(string description)
        {
            _event.Description = description;
            return this;
        }

        public Event Build()
        {
            return _event;
        }
    }
}
