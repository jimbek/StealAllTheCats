using Microsoft.EntityFrameworkCore;

namespace StealAllTheCats.API.Models.Data
{
    public class CatRepository : Repository, ICatRepository
    {
        public CatRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.CatEntities.AnyAsync(x => x.CatId == id);
        }

        public async Task AddAsync(CatEntity cat)
        {
            await _context.CatEntities.AddAsync(cat);
        }

        public async Task<CatEntity?> GetCatEntityAsync(string id)
        {
            return await _context.CatEntities.FirstOrDefaultAsync(x => x.CatId == id);
        }
    }
}
