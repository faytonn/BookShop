namespace Domain.Exceptions.BaseException
{
    public interface IBaseException
    {
        public string? Message { get; }
        public int StatusCode { get; }
    }
}
