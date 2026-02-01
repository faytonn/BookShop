namespace Project.Api.Domain.Entities.Commons.GlobalException;

public class BadRequestException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public BadRequestException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 400;
    }
}
