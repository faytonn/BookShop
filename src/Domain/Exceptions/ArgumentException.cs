namespace Domain.Exceptions;

public class ArgumentException : Exception, BaseException.IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }

    public ArgumentException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 400;
    }
}
