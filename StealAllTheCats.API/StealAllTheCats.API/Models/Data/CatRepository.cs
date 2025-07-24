using Microsoft.EntityFrameworkCore;

namespace StealAllTheCats.API.Models.Data
{
    public class CatRepository : ICatRepository
    {
        private ApplicationDbContext _context;

        public CatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.CatEntities.AnyAsync(x => x.CatId == id);
        }

        public async Task<int> AddAsync(CatEntity cat)
        {
            await _context.CatEntities.AddAsync(cat);

            return await _context.SaveChangesAsync();
        }

        public async Task<CatEntity?> GetCatEntityAsync(int id)
        {
            return await _context.CatEntities.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
