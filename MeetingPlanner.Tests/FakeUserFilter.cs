using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MeetingPlanner.Tests
{
    class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, FakeUserSettings.Id),
                new Claim(ClaimTypes.Name, FakeUserSettings.Name),
                new Claim(ClaimTypes.Email, FakeUserSettings.Email),
                new Claim(ClaimTypes.Role, FakeUserSettings.Role)
            }));

            await next();
        }
    }
}
