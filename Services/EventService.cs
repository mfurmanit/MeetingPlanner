using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
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

        public IEnumerable<EventResponse> GetAllPersonal(DateTime date, ClaimsPrincipal userContext)
        {
            var userId = _userService.GetUserId(userContext);
            var events = _repository.GetAllPersonal(DateUtils.GetDateRange(date), userId);
            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
        }

        public IEnumerable<EventResponse> GetAllGlobal(DateTime date)
        {
            var events = _repository.GetAllGlobal(DateUtils.GetDateRange(date));
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

            var dbEvent = _repository.GetOne(id);
            var mappedObject = new Event();
            var stateChanged = false;

            if (dbEvent.Global && !request.Global)
            {
                throw new ArgumentException("Nie można zmienić wydarzenia ogólnego na prywatne!");
            }

            if (!dbEvent.Global && request.Global) // Changing personal event to global event
            {
                dbEvent = _repository.GetOnePersonal(id);
                mappedObject = _mapper.Map(request, dbEvent);
                mappedObject.Notifications = new List<Notification>();
                mappedObject.User = null;
                mappedObject.UserId = null;
                stateChanged = true;
            } else if ((dbEvent.Global && request.Global) || (!dbEvent.Global && !request.Global)) // Update global / personal event normally
            {
                dbEvent = request.Global ? _repository.GetOneGlobal(id) : _repository.GetOnePersonal(id);
                mappedObject = _mapper.Map(request, dbEvent);
            }

            if (!mappedObject.Global)
            {
                mappedObject.User = _userService.GetUser(userContext);
            }

            if (!mappedObject.Global && mappedObject.User == null)
            {
                throw new ArgumentNullException("Spotkanie prywatne musi być przypisane do użytkownika.");
            }

            var updatedEvent = _repository.Update(mappedObject, stateChanged);
            return _mapper.Map<EventResponse>(updatedEvent);
        }

        public void Delete(string id)
        {
            var dbEvent = _repository.GetOneWithNotifications(id);
            _repository.Delete(dbEvent);
        }
    }
}