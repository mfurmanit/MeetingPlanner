using System.Security.Claims;
using MeetingPlanner.Models;

namespace MeetingPlanner.Services
{
    public interface IUserService
    {
        ApplicationUser GetUser(ClaimsPrincipal userContext);
        string GetUserId(ClaimsPrincipal userContext);
    }
}
