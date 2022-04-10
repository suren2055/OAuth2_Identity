namespace OAuth2_Identity.Models;

public class ApiResponse<T>
{
    public int ResponseCode { get; set; }
    public ResponseMessage Message { get; set; }
    public bool Success { get; set; }
    public T Data { get; set; }

    public ApiResponse()
    {
        Message = new ResponseMessage();
    }
        
}

public class ResponseMessage
{
    public string Eng { get; set; }

    public ResponseMessage()
    {
                
    }

    public ResponseMessage(string eng)
    {
        Eng = eng;
    }
}