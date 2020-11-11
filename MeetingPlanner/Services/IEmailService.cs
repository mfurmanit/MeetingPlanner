using MeetingPlanner.Models;

namespace MeetingPlanner.Services
{
    public interface IEmailService
    {
        void SendNotification(Event eventObject);
    }
}
