namespace StealAllTheCats.API.Models
{
    public enum Status
    {
        Queued, Started, Succeed, Failed
    }

    public class Job
    {
        public Guid Id { get; set; }
        public Status Status { get; set; } = Status.Queued;
        public DateTime Created { get; set; }

        public Job(Guid id)
        {
            Id = id;
        }
    }
}
