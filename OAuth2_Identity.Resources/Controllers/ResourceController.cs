using Microsoft.AspNetCore.Mvc;
using OAuth2_Identity.Recourses.Filters;

namespace OAuth2_Identity.Recourses.Controllers;

[Auth]
[ApiController]
[Route("api/[controller]")]
public class ResourceController : Controller
{
    [HttpGet("GET")]
    public IActionResult Get()
    {
        return Ok("Success from resource");
    }

    [HttpGet("GetAllProducts")]
    public IActionResult GetAllProducts()
    {
        return null;
    }
}