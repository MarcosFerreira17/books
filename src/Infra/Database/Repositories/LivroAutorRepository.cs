using Domain.Entities;
using Infra.Database.DbContexts;
using Infra.Database.Repositories.Interfaces;

namespace Infra.Database.Repositories;

public class LivroAutorRepository : BaseRepository<LivroAutor>, ILivroAutorRepository
{
    public LivroAutorRepository(ApplicationDbContext context) : base(context)
    {
    }
}
