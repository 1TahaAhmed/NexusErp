using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Common.Permissions;

namespace NexusErp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("users-view-only")]
        [HasPermission(Permissions.Users.View)]
        public IActionResult GetUsersView()
        {
            return Ok("You have access to View Users");
        }
    }
}
