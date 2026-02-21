namespace Domain.Exceptions;

public class InvalidDataException : Exception, BaseException.IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public InvalidDataException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 401;
    }
}
