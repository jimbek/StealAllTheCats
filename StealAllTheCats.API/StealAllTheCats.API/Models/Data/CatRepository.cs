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

        public async Task<bool> ExistsAsync(CancellationToken token, string id)
        {
            return await _context.CatEntities.AnyAsync(x => x.CatId == id, token);
        }

        public async Task AddAsync(CancellationToken token, CatEntity cat)
        {
            await _context.CatEntities.AddAsync(cat, token);
        }

        public async Task<CatEntity?> GetCatEntityAsync(CancellationToken token, string id)
        {
            return await _context.CatEntities.FirstOrDefaultAsync(x => x.CatId == id, token);
        }
    }
}
