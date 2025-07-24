using Microsoft.EntityFrameworkCore;

namespace StealAllTheCats.API.Models.Data
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.TagEntities.AnyAsync(x =>  x.Name == name);
        }

        public async Task AddAsync(TagEntity tag)
        {
            await _context.TagEntities.AddAsync(tag);
        }
    }
}
