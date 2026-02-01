namespace Project.Api.Domain.Entities.Commons.GlobalException;

public class DbUpdateException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public DbUpdateException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 409;
    }
}