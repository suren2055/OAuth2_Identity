using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OAuth2_Identity.Core.Exceptions;
using OAuth2_Identity.Models;


namespace OAuth2_Identity.Filters;

    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            if (context.Exception.GetType() == typeof(CoreException))
            {
                var json = new ApiResponse<object>
                {
                    Data = new[] {context.Exception.Message}
                };

                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            }
            else
            {
                var json = new ApiResponse<object>()
                {
                    Data = new[] {"An error occurred. Try it again."}
                };

                if (_env.IsDevelopment())
                {
                    json = new ApiResponse<object>()
                    {
                        ResponseCode = StatusCodes.Status500InternalServerError,
                        Message = new ResponseMessage("An error occurred."),
                        Data =  context.Exception
                    };
                }

                context.Result = new InternalServerErrorObjectResult(json);
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }
    }
