using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using MeetingPlanner.Dto;
using MeetingPlanner.Models;
using MeetingPlanner.Others.Exceptions;
using MeetingPlanner.Others.Utils;
using MeetingPlanner.Repositories;

namespace MeetingPlanner.Services
{
    public class EventService : IEventService
    {
        private readonly IUserService _userService;
        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;

        public EventService(IUserService userService, IEventRepository repository, IMapper mapper)
        {
            _userService = userService;
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<EventResponse> GetAllPersonal(ClaimsPrincipal userContext)
        {
            var userId = _userService.GetUserId(userContext);
            var events = _repository.GetAllPersonal(userId);
            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
        }

        public IEnumerable<EventResponse> GetAllGlobal()
        {
            var events = _repository.GetAllGlobal();
            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
        }

        #nullable enable
        public EventResponse GetOneById(string id, bool global, ClaimsPrincipal? userContext)
        {
            var eventObject = global ? _repository.GetOneGlobal(id) : _repository.GetOnePersonal(id);

            if (eventObject == null)
            {
                throw new ObjectNotFoundException("Spotkanie o wskazanym identyfikatorze nie istnieje!");
            }

            var globalEvent = global && eventObject.Global;
            var personalEvent = !globalEvent;

            var response = _mapper.Map<EventResponse>(eventObject);

            if (globalEvent)
            {
                return response;
            }

            var properUserContext = userContext != null && eventObject.User.Id.Equals(_userService.GetUserId(userContext));

            if (personalEvent && properUserContext)
            {
                return response;
            }

            throw new AccessViolationException("Nie posiadasz uprawnień do wyświetlenia wskazanego spotkania!");
        }

        public EventResponse Create(EventRequest request, ClaimsPrincipal userContext)
        {
            TimeUtils.ValidateHourFormat(request.HourFrom);
            TimeUtils.ValidateHourFormat(request.HourTo);

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
            TimeUtils.ValidateHourFormat(request.HourFrom);
            TimeUtils.ValidateHourFormat(request.HourTo);

            var eventObject = request.Global ? _repository.GetOneGlobal(id) : _repository.GetOnePersonal(id);
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