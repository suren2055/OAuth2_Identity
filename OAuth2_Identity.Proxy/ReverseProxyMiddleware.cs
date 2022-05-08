namespace OAuth2_Identity.Proxy;

public class ReverseProxyMiddleware
{
    
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    public ReverseProxyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _configuration = configuration;
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        var serviceHeader = "";
        if (context.Request.Headers.TryGetValue("ServiceName", out var header))
            serviceHeader = header;

        var targetUri = BuildTargetUri(context.Request);
        if (targetUri != null)
        {
            var authorization = "";
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationValue))
            {
                authorization = authorizationValue;
                authorization = authorization.Trim();
            }

            using var _httpClient = new HttpClient();
            var targetRequestMessage = CreateTargetMessage(context, targetUri);
            if (!string.IsNullOrWhiteSpace(authorization))
                targetRequestMessage.Headers.Add("Authorization",authorization);
            using var responseMessage = await _httpClient.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
            context.Response.StatusCode = (int)responseMessage.StatusCode;
            CopyFromTargetResponseHeaders(context, responseMessage);
            
            await responseMessage.Content.CopyToAsync(context.Response.Body);
            return;
        }

        await _next(context);
    }

    private static HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
    {
        var requestMessage = new HttpRequestMessage();
        CopyFromOriginalRequestContentAndHeaders(context, requestMessage);

        requestMessage.RequestUri = targetUri;
        requestMessage.Headers.Host = targetUri.Host;
        requestMessage.Method = GetMethod(context.Request.Method);
        

        return requestMessage;
    }
    private static void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
    {
        var requestMethod = context.Request.Method;

        if (!HttpMethods.IsGet(requestMethod) &&
            !HttpMethods.IsHead(requestMethod) &&
            !HttpMethods.IsDelete(requestMethod) &&
            !HttpMethods.IsTrace(requestMethod))
        {
            var streamContent = new StreamContent(context.Request.Body);
            requestMessage.Content = streamContent;
        }

        foreach (var (key, value) in context.Request.Headers)
            requestMessage.Content?.Headers.TryAddWithoutValidation(key, value.ToArray());
        
    }
    private static void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
    {
        foreach (var (key, value) in responseMessage.Headers)
            context.Response.Headers[key] = value.ToArray();
        

        foreach (var (key, value) in responseMessage.Content.Headers)
            context.Response.Headers[key] = value.ToArray();
        

        context.Response.Headers.Remove("transfer-encoding");
    }
    private static HttpMethod GetMethod(string method)
    {
        if (HttpMethods.IsDelete(method)) return HttpMethod.Delete;
        if (HttpMethods.IsGet(method)) return HttpMethod.Get;
        if (HttpMethods.IsHead(method)) return HttpMethod.Head;
        if (HttpMethods.IsOptions(method)) return HttpMethod.Options;
        if (HttpMethods.IsPost(method)) return HttpMethod.Post;
        if (HttpMethods.IsPut(method)) return HttpMethod.Put;
        return HttpMethods.IsTrace(method) ? HttpMethod.Trace : new HttpMethod(method);
    }

    private Uri BuildTargetUri(HttpRequest request)
    {
        Uri targetUri = null;

        if (request.Path.StartsWithSegments("/proxy/identity", out var identityPath))
            targetUri = new Uri($"{_configuration.GetSection("Settings:OAuthServices:Identity").Value}/Connect" + identityPath);
        
        else if (request.Path.StartsWithSegments("/proxy/resources", out var resourcePath))
            targetUri = new Uri($"{_configuration.GetSection("Settings:OAuthServices:Resource").Value}" + resourcePath);
        
        return targetUri;
    }
   
}