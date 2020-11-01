using System.Collections.Generic;
using System.Security.Claims;
using MeetingPlanner.Dto;

namespace MeetingPlanner.Services
{
    public interface IEventService
    {
        public IEnumerable<EventResponse> GetAllPersonal(ClaimsPrincipal userContext);
        public IEnumerable<EventResponse> GetAllGlobal();
        public EventResponse GetOneById(string id, bool global, ClaimsPrincipal? userContext);
        public EventResponse Create(EventRequest request, ClaimsPrincipal userContext);
        public EventResponse Update(string id, EventRequest request, ClaimsPrincipal userContext);
    }
}
