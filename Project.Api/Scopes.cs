namespace Project.Api
{
    public class SingletonService
    {
        public Guid Id { get; } = Guid.CreateVersion7();
    }

    public class ScopedService
    {
        public Guid Id { get; } = Guid.CreateVersion7();
    }

    public class TransientService : ITransient
    {
        public Guid Id { get; } = Guid.CreateVersion7();
    }

    public interface ITransient
    {
        Guid Id { get; }
    }
}
