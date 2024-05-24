using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace API.Controllers
{
    // or BuggyController
    public class ErrorTestingController : BaseApiController
    {
        private readonly DataContext _context;

        public ErrorTestingController(DataContext context)
        {
            _context = context;
        }

        // Expecting to see a 401 Unauthorized error, due to not sending up the authorization header despite having the [Authorize] attribute
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        // Expecting to see "Not Found" error with status "404"
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if (thing == null) return NotFound();

            return thing;
        }

        // Expecting to see a 500 Internal Server error with a null reference exception.
        // "System.NullReferenceException: Object reference not set to an instance of an object."
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString(); // thing will be null, so you can't use ToString()

            return thingToReturn;
        }

        // Expecting to just see this message with status "200 OK"
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}
