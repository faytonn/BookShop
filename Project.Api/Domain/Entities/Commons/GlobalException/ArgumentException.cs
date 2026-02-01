namespace Project.Api.Domain.Entities.Commons.GlobalException;

public class ArgumentException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }

    public ArgumentException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 400;
    }
}
