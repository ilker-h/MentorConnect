using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    // This Action Filter gets added as a service to ApplicationServiceExtensions.
    // This Action Filter's job is to update the LastActive property of the user whenever they interact with the API.
    public class LogUserActivity : IAsyncActionFilter
    {
        // Action Filters are similar to middleware in that they can be take a certain action upon a request coming into the API,
        // but middleware runs for every single request whereas Action Filters can run for only certain requests.
        // The Action Filter can be either run before "next" or after "next" (i.e. before/after the API endpoint has executed its functionality/action)
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) // this is async because it's returning a task
        {
            var resultContext = await next(); // ActionExecutedContext is after the API action has been completed. To get something before that, you'd use ActionExecutingContext

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId(); // the User is coming from the Claims Principal from the token

            var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            var user = await uow.UserRepository.GetUserByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}