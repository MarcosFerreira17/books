using Domain.Entities;
using Infra.Database.DbContexts;
using Infra.Database.Repositories.Interfaces;

namespace Infra.Database.Repositories;

public class LivroRepository : BaseRepository<Livro>, ILivroRepository
{
    public LivroRepository(ApplicationDbContext context) : base(context)
    {
    }
}
