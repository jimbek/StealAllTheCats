using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using StealAllTheCats.API.Jobs;
using StealAllTheCats.API.Jobs.Payloads;
using StealAllTheCats.API.Models;
using StealAllTheCats.API.Models.Data;
using StealAllTheCats.API.Services;

namespace StealAllTheCats.API
{
    public static class JobEndpoints
    {
        private static WebApplicationBuilder? _builder { get; set; }

        public static WebApplication MapJobEndpoints(this WebApplication app, WebApplicationBuilder builder)
        {
            _builder = builder;

            app
                .MapPost("/cats/fetch", Fetch)
                .WithName("FetchCats")
                .WithTags("Jobs")
                .WithOpenApi();

            app
                .MapGet("/jobs/{id}", GetJobStatus)
                .WithName("GetJobStatusById")
                .WithTags("Jobs")
                .WithOpenApi();

            return app;
        }

        public static async Task<Guid> Fetch(CancellationToken token, IQueue queue, ICatService catService, IUnitOfWork unitOfWork)
        {
            Guid id = Guid.NewGuid();

            var payload = new BulkInsertToDbPayload(id, _builder!.Configuration["ApiKey"] ?? string.Empty);

            queue.QueueInvocableWithPayload<BulkInsertToDb, BulkInsertToDbPayload?>(payload);

            await unitOfWork.JobRepository.AddAsync(new Job(id));
            await unitOfWork.SaveChangesAsync();

            return id;
        }

        public static async Task<Results<Ok<string>, BadRequest, NotFound>> GetJobStatus(IUnitOfWork unitOfWork, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return TypedResults.BadRequest();
            }

            var job = await unitOfWork.JobRepository.GetJobAsync(Guid.Parse(id));

            if (job == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(Enum.GetName(job.Status));
        }
    }
}
