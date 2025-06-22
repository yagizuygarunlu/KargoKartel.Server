using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Infrastructure.Contexts;
using KargoKartel.Server.Infrastructure.Shared;

namespace KargoKartel.Server.Infrastructure.Repositories
{
    internal sealed class CargoRepository : Repository<Cargo, ApplicationDbContext>, ICargoRepository
    {
        public CargoRepository(ApplicationDbContext context): base(context)
        {
        }       
    }
}
