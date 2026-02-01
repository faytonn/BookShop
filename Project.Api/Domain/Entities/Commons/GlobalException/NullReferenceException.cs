namespace Project.Api.Domain.Entities.Commons.GlobalException;

public class NullReferenceException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public NullReferenceException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 404;
    }
}