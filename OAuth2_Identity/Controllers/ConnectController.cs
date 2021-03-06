using Microsoft.AspNetCore.Mvc;
using OAuth2_Identity.Authentication;
using OAuth2_Identity.Core.Concrete;
using OAuth2_Identity.Models;

namespace OAuth2_Identity.Controllers;

[ApiController]
[Route("[controller]")]
public class ConnectController : Controller
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _uow;

    public ConnectController(IUserService userService, IUnitOfWork uow)
    {
        _userService = userService;
        _uow = uow;
    }

    [HttpPost("UserToken")]
    public IActionResult UserToken([FromBody] UserRequestDTO model)
    {
        var response = _userService.AuthenticateUser(model);
        return response.ResponseCode == 0 ? Ok(response) : BadRequest(response);
    }

    [HttpPost("ClientToken")]
    public IActionResult ClientToken([FromBody] ClientRequestDTO model)
    {
        var response = _userService.AuthenticateClient(model);
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