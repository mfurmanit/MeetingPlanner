using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using MeetingPlanner.Dto;
using MeetingPlanner.Models;
using MeetingPlanner.Others.Exceptions;
using MeetingPlanner.Repositories;

namespace MeetingPlanner.Services
{
    public class EventService
    {
        private readonly UserService _userService;
        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;

        public EventService(UserService userService, IEventRepository repository, IMapper mapper)
        {
            _userService = userService;
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<EventResponse> GetAll(ClaimsPrincipal userContext)
        {
            var userId = _userService.GetUserId(userContext);
            var events = _repository.GetAll(userId);
            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
        }

        public IEnumerable<EventResponse> GetAllGlobal()
        {
            var events = _repository.GetAllGlobal();
            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
        }

        public EventResponse GetOneById(string id, bool global, ClaimsPrincipal? userContext)
        {
            var eventObject = global ? _repository.GetOneById(id, global) : _repository.GetOnePersonal(id);

            if (eventObject == null)
            {
                throw new ObjectNotFoundException("Spotkanie o wskazanym identyfikatorze nie istnieje!");
            }

            var globalEvent = global && eventObject.Global;
            var personalEvent = !globalEvent;
            var withUserContext = eventObject.User != null && userContext != null;
            var properUserContext = withUserContext && eventObject.User.Id.Equals(_userService.GetUserId(userContext));

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
                mappedObject.User = _userService.GetUser(userContext);
            }

            if (!mappedObject.Global && mappedObject.User == null)
            {
                throw new ArgumentNullException("Spotkanie prywatne musi być przypisane do użytkownika.");
            }

            var createdEvent = _repository.Add(mappedObject);
            return _mapper.Map<EventResponse>(createdEvent);
        }

        public EventResponse Update(string id, EventRequest request, ClaimsPrincipal userContext)
        {
            var eventObject = request.Global ? _repository.GetOneById(id, request.Global) : _repository.GetOnePersonal(id);
            var mappedObject = _mapper.Map(request, eventObject);

            if (!mappedObject.Global)
            {
                mappedObject.User = _userService.GetUser(userContext);
            }

            if (!mappedObject.Global && mappedObject.User == null)
            {
                throw new ArgumentNullException("Spotkanie prywatne musi być przypisane do użytkownika.");
            }

            var updatedEvent = _repository.Update(mappedObject);
            return _mapper.Map<EventResponse>(updatedEvent);
        }
    }
}