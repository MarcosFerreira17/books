using Domain.Entities;
using Infra.Database.DbContexts;
using Infra.Database.Repositories.Interfaces;

namespace Infra.Database.Repositories;

public class LivroAssuntoRepository : BaseRepository<LivroAssunto>, ILivroAssuntoRepository
{
    public LivroAssuntoRepository(ApplicationDbContext context) : base(context)
    {
    }
}
