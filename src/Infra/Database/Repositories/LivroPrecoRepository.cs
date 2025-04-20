using Domain.Entities;
using Infra.Database.DbContexts;
using Infra.Database.Repositories.Interfaces;

namespace Infra.Database.Repositories;

public class LivroPrecoRepository : BaseRepository<LivroPreco>, ILivroPrecoRepository
{
    public LivroPrecoRepository(ApplicationDbContext context) : base(context)
    {
    }
}
