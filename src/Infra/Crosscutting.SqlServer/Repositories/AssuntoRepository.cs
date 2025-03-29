using Domain.Entities;
using Infra.Database.DbContexts;
using Infra.Database.Repositories.Interfaces;

namespace Infra.Database.Repositories;

public class AssuntoRepository : BaseRepository<Assunto>, IAssuntoRepository
{
    public AssuntoRepository(ApplicationDbContext context) : base(context)
    {
    }
}
