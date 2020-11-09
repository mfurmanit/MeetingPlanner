using System;
using MeetingPlanner.Dto;
using MeetingPlanner.Models;
using MeetingPlanner.Others.Builders;

namespace MeetingPlanner.Tests.Fixtures
{
    class EventFixtures
    {
        public static EventRequestBuilder SomeEventRequest()
        {
            return EventRequestBuilder.Start()
                .Title("Test event")
                .Date(DateTime.Now)
                .Global(true)
                .Description("Take notebooks to write down key information.");
        }

        public static EventBuilder SomeEvent()
        {
            return EventBuilder.Start()
                .Title("Test event")
                .Date(new DateTime(2020, 11, 9, 0, 0, 0))
                .Global(true)
                .Description("Take notebooks to write down key information.");
        }
    }
}
