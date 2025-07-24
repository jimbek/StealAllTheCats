namespace StealAllTheCats.API.Models.Data
{
    public class Repository : IRepository
    {
        protected ApplicationDbContext _context;

        protected Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
