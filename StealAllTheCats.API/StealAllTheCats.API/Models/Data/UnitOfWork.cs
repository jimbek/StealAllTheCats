namespace StealAllTheCats.API.Models.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private readonly ICatRepository _catRepository;
        private readonly ITagRepository _tagRepository;

        public UnitOfWork
        (
            ApplicationDbContext context,
            ICatRepository catRepository,
            ITagRepository tagRepository
        )
        {
            _context = context;
            _catRepository = catRepository;
            _tagRepository = tagRepository;
        }

        public ICatRepository CatRepository { get { return _catRepository; } }

        public ITagRepository TagRepository { get { return _tagRepository; } }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
