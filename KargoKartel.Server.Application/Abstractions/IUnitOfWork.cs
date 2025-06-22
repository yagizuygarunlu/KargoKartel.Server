namespace KargoKartel.Server.Infrastructure.Contexts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}