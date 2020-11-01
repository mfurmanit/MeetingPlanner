using MeetingPlanner.Models;

namespace MeetingPlanner.Services
{
    public interface IEmailService
    {
        public void SendNotification(Event eventObject);
    }
}
