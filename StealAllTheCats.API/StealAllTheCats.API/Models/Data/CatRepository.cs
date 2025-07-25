using Microsoft.EntityFrameworkCore;

namespace StealAllTheCats.API.Models.Data
{
    public class CatRepository : ICatRepository
    {
        private readonly ApplicationDbContext _context;

        public CatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.CatEntities.AsNoTracking().AnyAsync(x => x.CatId == id);
        }

        public async Task AddAsync(CatEntity cat)
        {
            await _context.CatEntities.AddAsync(cat);
        }

        public async Task<CatEntity?> GetCatEntityAsync(CancellationToken token, string id)
        {
            return await _context.CatEntities.AsNoTracking().FirstOrDefaultAsync(x => x.CatId == id, token);
        }

        public async Task<IList<CatEntity>> GetCatEntitiesAsync(CancellationToken token, int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            
            return await _context.CatEntities.AsNoTracking().Skip(skip).Take(pageSize).ToListAsync(token);
        }
    }
}
