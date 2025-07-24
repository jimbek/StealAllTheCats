namespace StealAllTheCats.API.Models.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private readonly ICatRepository _catRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IJobRepository _jobRepository;

        public UnitOfWork
        (
            ApplicationDbContext context,
            ICatRepository catRepository,
            ITagRepository tagRepository,
            IJobRepository jobRepository
        )
        {
            _context = context;
            _catRepository = catRepository;
            _tagRepository = tagRepository;
            _jobRepository = jobRepository;
        }

        public ICatRepository CatRepository { get { return _catRepository; } }
        public ITagRepository TagRepository { get { return _tagRepository; } }
        public IJobRepository JobRepository { get { return _jobRepository; } }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
