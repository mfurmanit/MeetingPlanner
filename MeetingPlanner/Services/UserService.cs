using System.Security.Claims;
using MeetingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MeetingPlanner.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public ApplicationUser GetUser(ClaimsPrincipal userContext)
        {
            var applicationUser = _userManager.FindByIdAsync(GetUserId(userContext)).Result;
            _logger.LogInformation($"Found user username - '{applicationUser}'.");
            return applicationUser;
        }

        public string GetUserId(ClaimsPrincipal userContext)
        {
            var userId = userContext.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation($"Found user id - '{userId}'.");
            return userId;
        }
    }
}
