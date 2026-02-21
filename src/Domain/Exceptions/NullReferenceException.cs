namespace Domain.Exceptions;

public class NullReferenceException : Exception, BaseException.IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public NullReferenceException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 404;
    }
}
