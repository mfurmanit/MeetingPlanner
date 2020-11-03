using System;
using MeetingPlanner.Dto;

namespace MeetingPlanner.Others.Builders
{
    public class EventRequestBuilder
    {
        private EventRequest _eventRequest;

        private EventRequestBuilder()
        {
            _eventRequest = new EventRequest();
        }

        public static EventRequestBuilder Start()
        {
            return new EventRequestBuilder();
        }

        public EventRequestBuilder Date(DateTime dateTime)
        {
            _eventRequest.Date = dateTime;
            return this;
        }

        public EventRequestBuilder Title(string title)
        {
            _eventRequest.Title = title;
            return this;
        }

        public EventRequestBuilder HourFrom(string hourFrom)
        {
            _eventRequest.HourFrom = hourFrom;
            return this;
        }

        public EventRequestBuilder HourTo(string hourTo)
        {
            _eventRequest.HourTo = hourTo;
            return this;
        }

        public EventRequestBuilder Global(bool global)
        {
            _eventRequest.Global = global;
            return this;
        }

        public EventRequestBuilder Description(string description)
        {
            _eventRequest.Description = description;
            return this;
        }

        public EventRequest Build()
        {
            return _eventRequest;
        }
    }
}
