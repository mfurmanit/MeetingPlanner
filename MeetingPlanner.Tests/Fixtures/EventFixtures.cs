using System;
using MeetingPlanner.Dto;
using MeetingPlanner.Others.Builders;

namespace MeetingPlanner.Tests.Fixtures
{
    class EventFixtures
    {
        public static EventRequest SomeEventRequest()
        {
            return EventRequestBuilder.Start()
                .Title("Test event")
                .Date(DateTime.Now)
                .Global(true)
                .Description("Take notebooks to write down key information.")
                .Build();
        }
    }
}
