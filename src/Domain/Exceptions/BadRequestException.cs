namespace Domain.Exceptions;

public class BadRequestException : Exception, BaseException.IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public BadRequestException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 400;
    }
}
