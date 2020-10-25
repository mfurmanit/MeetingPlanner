using System;
using System.Collections.Generic;
using System.Security.Claims;
using MeetingPlanner.Models;
using MeetingPlanner.Others.Exceptions;
using MeetingPlanner.Repositories;
using Microsoft.AspNetCore.Identity;

namespace MeetingPlanner.Services
{
    public class EventService
    {
        private static IEventRepository _repository;
        private static UserManager<ApplicationUser> _userManager;

        public EventService(IEventRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public IEnumerable<Event> GetAll(ClaimsPrincipal userContext)
        {
            return _repository.GetAll(_userManager.GetUserId(userContext));
        }

        public IEnumerable<Event> GetAllGlobal()
        {
            return _repository.GetAllGlobal();
        }

        public Event GetOneById(string id, bool global, ClaimsPrincipal? userContext)
        {
            var eventObject = _repository.GetOneById(id, global);

            if (eventObject == null)
            {
                throw new ObjectNotFoundException("Spotkanie o wskazanym identyfikatorze nie istnieje!");
            }

            var globalEvent = global && eventObject.Global;
            var personalEvent = !globalEvent;
            var withUserContext = eventObject.User != null && userContext != null;
            var properUserContext = withUserContext && eventObject.User.Id.Equals(_userManager.GetUserId(userContext));

            if (globalEvent)
            {
                return eventObject;
            }

            if (personalEvent && properUserContext)
            {
                return eventObject;
            }

            throw new AccessViolationException("Nie posiadasz uprawnień aby wyświetlić wskazane spotkanie!");
        }
    }
}