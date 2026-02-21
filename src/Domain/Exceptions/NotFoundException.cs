namespace Domain.Exceptions;

public class NotFoundException : Exception, BaseException.IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public NotFoundException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 404;
    }
}
