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

        public async Task<bool> ExistsAsync(CancellationToken token, string name)
        {
            return await _context.TagEntities.AnyAsync(x =>  x.Name == name, token);
        }

        public async Task AddAsync(CancellationToken token, TagEntity tag)
        {
            await _context.TagEntities.AddAsync(tag, token);
        }
    }
}
