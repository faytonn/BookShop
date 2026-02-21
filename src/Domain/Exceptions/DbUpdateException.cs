namespace Domain.Exceptions;

public class DbUpdateException : Exception, BaseException.IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public DbUpdateException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 409;
    }
}
