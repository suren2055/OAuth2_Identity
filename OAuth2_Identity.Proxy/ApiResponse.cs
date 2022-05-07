namespace OAuth2_Identity.Proxy;

public class ApiResponse<T>
{
    public int ResponseCode { get; set; }
    public ResponseMessage Message { get; set; }
    public T Data { get; set; }
    public bool Success { get; set; }

    public ApiResponse()
    {
        Message = new ResponseMessage();
    }
}

public class ResponseMessage
{
    public string Eng { get; set; }
    public string Arm { get; set; }
    public string Rus { get; set; }
}