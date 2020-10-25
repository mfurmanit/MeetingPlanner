using System.Collections.Generic;
using MeetingPlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private EventService _service;
        
        public EventsController(EventService service)
        {
            _service = service;
        }

        // GET: api/events
        [Authorize]
        [HttpGet]
        public IEnumerable<Event> GetAll()
        {
            return _service.GetAll(this.User);
        }

        // GET: api/global-events
        [HttpGet]
        public IEnumerable<Event> GetAllGlobal()
        {
            return _service.GetAllGlobal();
        }

        // GET api/events/{id}
        [Authorize]
        [HttpGet("{id}")]
        public Event Get(string id)
        {
            return _service.GetOneById(id, false, this.User);
        }

        // GET api/global-events/{id}
        [HttpGet("{id}")]
        public Event GetGlobal(string id)
        {
            return _service.GetOneById(id, true, null);
        }

        // POST api/events
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/events/{id}
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/events/{id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
