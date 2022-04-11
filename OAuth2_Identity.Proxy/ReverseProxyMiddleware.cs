namespace OAuth2_Identity.Proxy;

public class ReverseProxyMiddleware
{
    private static readonly HttpClient _httpClient = new();
    private readonly RequestDelegate _nextMiddleware;


    public ReverseProxyMiddleware(RequestDelegate nextMiddleware)
    {
        _nextMiddleware = nextMiddleware;
    }

    public async Task Invoke(HttpContext context)
    {
        var targetUri = BuildTargetUri(context.Request);

        if (targetUri != null)
        {
            var targetRequestMessage = CreateTargetMessage(context, targetUri);

            using var responseMessage = await _httpClient.SendAsync(targetRequestMessage,
                HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
            context.Response.StatusCode = (int) responseMessage.StatusCode;
            CopyFromTargetResponseHeaders(context, responseMessage);
            await responseMessage.Content.CopyToAsync(context.Response.Body);

            return;
        }

        await _nextMiddleware(context);
    }

    private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
    {
        var requestMessage = new HttpRequestMessage();
        CopyFromOriginalRequestContentAndHeaders(context, requestMessage);

        requestMessage.RequestUri = targetUri;
        requestMessage.Headers.Host = targetUri.Host;
        requestMessage.Method = GetMethod(context.Request.Method);

        return requestMessage;
    }

    private void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
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

        foreach (var header in context.Request.Headers)
        {
            requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        }
    }

    private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
    {
        foreach (var (key, value) in responseMessage.Headers)
        {
            context.Response.Headers[key] = value.ToArray();
        }

        foreach (var (key, value) in responseMessage.Content.Headers)
        {
            context.Response.Headers[key] = value.ToArray();
        }

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

    private static Uri BuildTargetUri(HttpRequest request)
    {
        Uri targetUri = null;

        if (request.Path.StartsWithSegments("/googleforms", out var remainingPath))
            targetUri = new Uri("https://docs.google.com/forms" + remainingPath);

        return targetUri;
    }
}