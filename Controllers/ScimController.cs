using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TestScimRest.DataAccess;
using TestScimRest.Schemas;

namespace TestScimRest.Controllers
{
    [ApiController]
    [Route("[controller]/v2/")]
    public class ScimController : ControllerBase
    {
        private readonly DbContext context;

        public ScimController()
        {
            context = new DbContext();
        }

        // GET: Users?filter=bob.johnson@example.com&count=1&startIndex=1
        [HttpGet("Users")]
        public IActionResult GetUsers([FromQuery] ListRequest request)
        {
            ListResponse<Schemas.User> response = new ListResponse<Schemas.User>();
            response.Resources = new ();

            var pageIndex = request.StartIndex;
            var count = request.Count;

            var users = context.GetUsers(pageIndex, count);

            response.TotalResults = users.Count();
            response.StartIndex = pageIndex;
            response.ItemsPerPage = count;

            foreach ( var user in users )
            {
                response.Resources.Add(user.ToSchemaUser());
            }

            return Ok(response);
        }


        // GET: Users/11111111-1111-1111-1111-111111111111
        [HttpGet("Users/{id}")]
        public IActionResult GetUser(Guid id)
        {
            var user = context.GetUser(id);

            if(user == null)
            {
                return NotFound(new ErrorResponse()
                {
                    Detail = "User not found.",
                    Status = "404"
                });            
            }

            var response = user.ToSchemaUser();
            response.Groups = context.GetUserGroups(user);

            return Ok(response);
        }


        [HttpPost("Users")]
        public IActionResult CreateUser(CreateUserRequest request)
        {
           
            var exitingUser = context.GetUser(request.UserName);
            if(exitingUser != null)
            {
                return Ok(new ErrorResponse()
                {
                    Detail = $"User {request.UserName} already exists.",
                    Status = "409"
                });
            }

            var user = context.CreateUser(request);

            var response = user.ToSchemaUser();
            response.Groups = context.GetUserGroups(user);

            return Ok(response);
        }

        // PUT: Users/id
        // Map attributes, see which are different and update the user in db
        // Return updated user object


        //PATCH: Users/id
        //For OIN integrations this is PATCH
        //For custom app integrations this is a PUT
        //deactivate a user, see docs for payload info
        //Response is the updated user object



        [HttpGet("Groups")]
        public IActionResult GetGroups([FromQuery] ListRequest request)
        {
            ListResponse<Schemas.Group> response = new ListResponse<Schemas.Group>();
            response.Resources = new();

            var pageIndex = request.StartIndex;
            var count = request.Count;

            var groups = context.GetGroups(pageIndex, count);

            response.Resources = groups;
            response.TotalResults = groups.Count();
            response.StartIndex = pageIndex;
            response.ItemsPerPage = count;

            return Ok(response);
        }
    }
}