using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // e.g. /api/users
    public class BaseApiController : ControllerBase
    {

    }
}