
namespace Project.Api.Domain.Entities.Commons.GlobalException;

public class NotFoundException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string? Message { get; }
    public NotFoundException(string? message = null) : base(message)
    {
        Message = message;
        StatusCode = 404;
    }
}
