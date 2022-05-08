using Microsoft.AspNetCore.Mvc;
using OAuth2_Identity.Recourses.Filters;
using OAuth2_Identity.Resources.Core.Concrete;

namespace OAuth2_Identity.Recourses.Controllers;

[Auth]
[ApiController]
[Route("[controller]")]
public class ResourceController : Controller
{
    private readonly EFDBContext _context;

    public ResourceController(EFDBContext context)
    {
        _context = context;
    }

    

    [HttpGet("GetAllProducts")]
    public IActionResult GetAllProducts()
    {
        return Ok(_context.Products.ToList());
    }
}