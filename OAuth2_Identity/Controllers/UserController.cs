using Microsoft.AspNetCore.Mvc;
using OAuth2_Identity.Authentication;
using OAuth2_Identity.Core.Concrete;
using OAuth2_Identity.Models;

namespace OAuth2_Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _uow;

    public UserController(IUserService userService, IUnitOfWork uow)
    {
        _userService = userService;
        _uow = uow;
    }

    [HttpPost("Authenticate")]
    public IActionResult Authenticate([FromBody] UserRequestDTO model)
    {
        var response = _userService.Authenticate(model);
        return response.ResponseCode == 0 ? Ok(response) : BadRequest(response);
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] UserRequestDTO model)
    {
        var t = _userService.Register(model);
        _uow.Commit();
        return Ok(t);
    }
}