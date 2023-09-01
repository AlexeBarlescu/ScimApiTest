using Microsoft.AspNetCore.Mvc;

namespace TestScimRest.Controllers
{
    [ApiController]
    [Route("[controller]/v2/")]
    public class ScimController : ControllerBase
    {
        

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            return Ok("alex");
        }
    }
}