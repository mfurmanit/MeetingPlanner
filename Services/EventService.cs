using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using MeetingPlanner.Dto;
using MeetingPlanner.Models;
using MeetingPlanner.Others.Exceptions;
using MeetingPlanner.Repositories;
using Microsoft.AspNetCore.Identity;

namespace MeetingPlanner.Services
{
    public class EventService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository repository, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public IEnumerable<EventResponse> GetAll(ClaimsPrincipal userContext)
        {
            var events = _repository.GetAll(_userManager.GetUserId(userContext));
            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
        }

        public IEnumerable<EventResponse> GetAllGlobal()
        {
            var events = _repository.GetAllGlobal();
            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
        }

        public EventResponse GetOneById(string id, bool global, ClaimsPrincipal? userContext)
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

            var response = _mapper.Map<EventResponse>(eventObject);

            if (globalEvent)
            {
                return response;
            }

            if (personalEvent && properUserContext)
            {
                return response;
            }

            throw new AccessViolationException("Nie posiadasz uprawnień do wyświetlenia wskazanego spotkania!");
        }

        public EventResponse Create(EventRequest request, ClaimsPrincipal userContext)
        {
            var eventObject = _mapper.Map<Event>(request);
            var mappedObject = _mapper.Map<Event>(eventObject);

            if (!mappedObject.Global)
            {
                mappedObject.User = _userManager.GetUserAsync(userContext).Result;
            }

            var createdEvent = _repository.Add(mappedObject);
            return _mapper.Map<EventResponse>(createdEvent);
        }

        public EventResponse Update(string id, EventRequest request, ClaimsPrincipal userContext)
        {
            var eventObject = _repository.GetOneById(id, request.Global);
            var mappedObject = _mapper.Map(request, eventObject);

            if (!mappedObject.Global)
            {
                mappedObject.User = _userManager.GetUserAsync(userContext).Result;
            }

            var updatedEvent = _repository.Update(mappedObject);
            return _mapper.Map<EventResponse>(updatedEvent);
        }
    }
}