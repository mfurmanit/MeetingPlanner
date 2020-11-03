using System.Security.Claims;
using MeetingPlanner.Models;
using Microsoft.AspNetCore.Identity;

namespace MeetingPlanner.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser GetUser(ClaimsPrincipal userContext)
        {
            return _userManager.FindByIdAsync(GetUserId(userContext)).Result;
        }

        public string GetUserId(ClaimsPrincipal userContext)
        {
            return userContext.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
