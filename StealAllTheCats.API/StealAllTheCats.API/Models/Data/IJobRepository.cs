namespace StealAllTheCats.API.Models.Data
{
    public interface IJobRepository
    {
        Task<Job?> GetJobAsync(Guid id);
        Task AddAsync(Job job);
        Task UpdateIfExistsAsync(Guid id, Status status);
    }
}
