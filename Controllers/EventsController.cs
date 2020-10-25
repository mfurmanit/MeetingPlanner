using System.Collections.Generic;
using MeetingPlanner.Dto;
using MeetingPlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventService _service;
        
        public EventsController(EventService service)
        {
            _service = service;
        }

        // GET: api/events
        [Authorize]
        [HttpGet]
        public IEnumerable<EventResponse> GetAll()
        {
            return _service.GetAll(this.User);
        }

        // GET: api/global-events
        [HttpGet("global")]
        public IEnumerable<EventResponse> GetAllGlobal()
        {
            return _service.GetAllGlobal();
        }

        // GET api/events/{id}
        [Authorize]
        [HttpGet("{id}")]
        public EventResponse Get(string id)
        {
            return _service.GetOneById(id, false, this.User);
        }

        // GET api/global-events/{id}
        [HttpGet("global/{id}")]
        public EventResponse GetGlobal(string id)
        {
            return _service.GetOneById(id, true, null);
        }

        // POST api/events
        [HttpPost]
        public EventResponse Post([FromBody] EventRequest request)
        {
            return _service.Create(request, this.User);
        }

        // PUT api/events/{id}
        [HttpPut("{id}")]
        public EventResponse Put(string id, [FromBody] EventRequest request)
        {
            return _service.Update(id, request, this.User);
        }

        // DELETE api/events/{id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
