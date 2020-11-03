using AutoMapper;
using MeetingPlanner.Dto;
using MeetingPlanner.Models;

namespace MeetingPlanner.Others.Utils
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<EventRequest, Event>();
            CreateMap<Event, EventResponse>();
            CreateMap<NotificationRequest, Notification>();
            CreateMap<Notification, NotificationResponse>();
        }
    }
}
