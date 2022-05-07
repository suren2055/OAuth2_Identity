using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OAuth2_Identity.Resources.Models;

namespace OAuth2_Identity.Recourses.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class Auth : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new JsonResult(new ApiResponse<string>
            {
                ResponseCode = (int) HttpStatusCode.Unauthorized,
                Message = new ResponseMessage {Eng = HttpStatusCode.Unauthorized.ToString()},
                Data = HttpStatusCode.Unauthorized.ToString()
            }) {StatusCode = StatusCodes.Status401Unauthorized};
        }
    }
}