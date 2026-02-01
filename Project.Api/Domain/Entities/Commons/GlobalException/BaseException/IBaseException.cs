namespace Project.Api.Domain.Entities.Commons.GlobalException.BaseExceptions
{
    public interface IBaseException
    {
        public string? Message { get; }
        public int StatusCode { get; }
    }
}
