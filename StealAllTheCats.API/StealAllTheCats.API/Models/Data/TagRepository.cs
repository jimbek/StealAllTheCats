using Microsoft.EntityFrameworkCore;

namespace StealAllTheCats.API.Models.Data
{
    public class TagRepository : Repository, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context) { }

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
