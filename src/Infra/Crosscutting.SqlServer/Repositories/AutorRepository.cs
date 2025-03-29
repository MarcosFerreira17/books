using Domain.Entities;
using Infra.Database.DbContexts;
using Infra.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Repositories;

public class AutorRepository : BaseRepository<Autor>, IAutorRepository
{
    public AutorRepository(ApplicationDbContext context) : base(context)
    {
    }
}
