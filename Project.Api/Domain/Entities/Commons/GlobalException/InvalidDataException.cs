namespace Project.Api.Domain.Entities.Commons.GlobalException;

public class InvalidDataException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public InvalidDataException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 401;
    }
}