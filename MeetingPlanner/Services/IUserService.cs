using System.Security.Claims;
using MeetingPlanner.Models;

namespace MeetingPlanner.Services
{
    public interface IUserService
    {
        public ApplicationUser GetUser(ClaimsPrincipal userContext);
        public string GetUserId(ClaimsPrincipal userContext);
    }
}
