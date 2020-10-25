using AutoMapper;
using MeetingPlanner.Dto;
using MeetingPlanner.Models;

namespace MeetingPlanner.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventRequest, Event>();
            CreateMap<Event, EventResponse>();
            CreateMap<NotificationRequest, Notification>();
            CreateMap<Notification, NotificationResponse>();
        }
    }
}
