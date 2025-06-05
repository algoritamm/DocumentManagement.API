using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Rest.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Welcome to Core system Rest API.";
        }
    }
}
