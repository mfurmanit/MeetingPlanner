using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using MeetingPlanner.Dto;
using MeetingPlanner.Models;
using MeetingPlanner.Others.Exceptions;
using MeetingPlanner.Others.Utils;
using MeetingPlanner.Repositories;
using Microsoft.Extensions.Logging;

namespace MeetingPlanner.Services
{
    public class EventService : IEventService
    {
        private readonly IUserService _userService;
        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        public EventService(IUserService userService, IEventRepository repository,
            IMapper mapper, ILogger<EventService> logger)
        {
            _userService = userService;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<EventResponse> GetAllPersonal(DateTime date, ClaimsPrincipal userContext)
        {
            var userId = _userService.GetUserId(userContext);
            _logger.LogInformation($"Method call - Get personal events for user with id '{userId}'.");
            var events = _repository.GetAllPersonal(DateUtils.GetDateRange(date), userId);
            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
        }

        public IEnumerable<EventResponse> GetAllGlobal(DateTime date)
        {
            _logger.LogInformation("Method call - Get global events.");
            var events = _repository.GetAllGlobal(DateUtils.GetDateRange(date));
            return _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
        }

        #nullable enable
        public EventResponse GetOneById(string id, bool global, ClaimsPrincipal? userContext)
        {
            _logger.LogInformation($"Method call - Get event with id '{id}'.");

            var eventObject = global ? _repository.GetOneGlobal(id) : _repository.GetOnePersonal(id);

            if (eventObject == null)
                throw new ObjectNotFoundException("Spotkanie o wskazanym identyfikatorze nie istnieje!");

            var globalEvent = global && eventObject.Global;
            var personalEvent = !globalEvent;

            var response = _mapper.Map<EventResponse>(eventObject);

            if (globalEvent)
                return response;

            var properUserContext = userContext != null && eventObject.User.Id.Equals(_userService.GetUserId(userContext));

            if (personalEvent && properUserContext)
                return response;

            throw new AccessViolationException("Nie posiadasz uprawnień do wyświetlenia wskazanego spotkania!");
        }

        public EventResponse Create(EventRequest request, ClaimsPrincipal userContext)
        {
            _logger.LogInformation($"Method call - Create new event with title '{request.Title}'.");

            TimeUtils.ValidateHourFormat(request.HourFrom);
            TimeUtils.ValidateHourFormat(request.HourTo);
            TimeUtils.ValidateBothHours(request.HourFrom, request.HourTo);
            DateUtils.ValidateEventDateOnCreate(request.Date);

            var eventObject = _mapper.Map<Event>(request);
            var mappedObject = _mapper.Map<Event>(eventObject);

            if (!mappedObject.Global)
                mappedObject.User = _userService.GetUser(userContext);

            if (!mappedObject.Global && mappedObject.User == null)
                throw new ArgumentException("Spotkanie prywatne musi być przypisane do użytkownika.");

            _logger.LogInformation(!mappedObject.Global
                ? $"Creating personal event ( '{mappedObject.Title}' ) for user with id '{mappedObject.User.Id}'."
                : $"Creating global event ( '{mappedObject.Title}' ).");

            var createdEvent = _repository.Add(mappedObject);
            return _mapper.Map<EventResponse>(createdEvent);
        }

        public EventResponse Update(string id, EventRequest request, ClaimsPrincipal userContext)
        {
            _logger.LogInformation($"Method call - Update existing event with id '{id}'.");

            TimeUtils.ValidateHourFormat(request.HourFrom);
            TimeUtils.ValidateHourFormat(request.HourTo);
            TimeUtils.ValidateBothHours(request.HourFrom, request.HourTo);

            var dbEvent = _repository.GetOne(id);
            DateUtils.ValidateEventDateOnUpdate(request.Date, dbEvent.Date);

            var mappedObject = new Event();
            var stateChanged = false;

            if (dbEvent.Global && !request.Global)
                throw new ArgumentException("Nie można zmienić spotkania ogólnego na prywatne!");

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
                mappedObject.User = _userService.GetUser(userContext);

            if (!mappedObject.Global && mappedObject.User == null)
                throw new ArgumentException("Spotkanie prywatne musi być przypisane do użytkownika.");

            _logger.LogInformation(!mappedObject.Global
                ? $"Updating personal event ( '{mappedObject.Title}' - {mappedObject.Id} ) for user with id '{mappedObject.User?.Id}'."
                : $"Updating global event ( '{mappedObject.Title}' - {mappedObject.Id} ).");

            var updatedEvent = _repository.Update(mappedObject, stateChanged);
            return _mapper.Map<EventResponse>(updatedEvent);
        }

        public void Delete(string id)
        {
            _logger.LogInformation($"Method call - Delete existing event with id '{id}'.");
            var dbEvent = _repository.GetOneWithNotifications(id);
            _repository.Delete(dbEvent);
        }
    }
}