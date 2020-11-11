using System;
using System.Collections.Generic;
using System.Security.Claims;
using MeetingPlanner.Dto;

namespace MeetingPlanner.Services
{
    public interface IEventService
    {
        IEnumerable<EventResponse> GetAllPersonal(DateTime date, ClaimsPrincipal userContext);
        IEnumerable<EventResponse> GetAllGlobal(DateTime date);
        #nullable enable
        EventResponse GetOneById(string id, bool global, ClaimsPrincipal? userContext);
        EventResponse Create(EventRequest request, ClaimsPrincipal userContext);
        EventResponse Update(string id, EventRequest request, ClaimsPrincipal userContext);
        void Delete(string id);
    }
}
