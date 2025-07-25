using Coravel;
using Coravel.Queuing.Interfaces;
using Microsoft.EntityFrameworkCore;
using StealAllTheCats.API.Jobs;
using StealAllTheCats.API.Jobs.Payloads;
using StealAllTheCats.API.Models;
using StealAllTheCats.API.Models.Data;
using StealAllTheCats.API.Models.DTOs;
using StealAllTheCats.API.Services;

var builder = WebApplication.CreateBuilder(args);

#region define services
builder
    .Services
    .AddDbContext<ApplicationDbContext>(options =>
    {
        string? db = builder.Configuration.GetConnectionString("Db");

        if (string.IsNullOrWhiteSpace(db))
        {
            throw new InvalidOperationException("Connection string 'Db'" + " not found.");
        }

        options.UseSqlServer(db);
    });

builder
    .Services
    .AddQueue();

builder
    .Services
    .AddScoped<ICatRepository, CatRepository>()
    .AddScoped<ITagRepository, TagRepository>()
    .AddScoped<IJobRepository, JobRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddTransient<BulkInsertToDb>()
    .AddSingleton<ICatService, CatService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region define app settings
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
#endregion

#region define requests
app.MapPost("/cats/fetch", async (CancellationToken token, IQueue queue, ICatService catService, IUnitOfWork unitOfWork) =>
{
    Guid id = Guid.NewGuid();

    var payload = new BulkInsertToDbPayload(id, builder.Configuration["ApiKey"] ?? string.Empty);

    queue.QueueInvocableWithPayload<BulkInsertToDb, BulkInsertToDbPayload?>(payload);

    await unitOfWork.JobRepository.AddAsync(new Job(id));
    await unitOfWork.SaveChangesAsync();

    return id;
})
.WithName("FetchCats")
.WithOpenApi();

app.MapGet("/jobs/{id}", async (IUnitOfWork unitOfWork, string id) =>
{
    if (string.IsNullOrWhiteSpace(id))
    {
        return Results.BadRequest();
    }

    var job = await unitOfWork.JobRepository.GetJobAsync(Guid.Parse(id));

    if (job == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(Enum.GetName(job.Status));
})
.WithName("GetJobStatusById")
.WithOpenApi();

app.MapGet("/cats/{id}", async (IUnitOfWork unitOfWork, CancellationToken token, string id) =>
{
    if (string.IsNullOrWhiteSpace(id))
    {
        return Results.BadRequest();
    }

    var cat = await unitOfWork.CatRepository.GetCatEntityAsync(token, id);

    if (cat == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new Image(cat));
})
.WithName("GetCatById")
.WithOpenApi();

app.MapGet("/cats", async (IUnitOfWork unitOfWork, CancellationToken token, int page = 1, int pageSize = 10) =>
{
    if (page < 1 || pageSize < 1)
    {
        return Results.BadRequest();
    }

    var cats = await unitOfWork.CatRepository.GetCatEntitiesAsync(token, page, pageSize);

    return Results.Ok(cats.Select(x => new Image(x)));
})
.WithName("GetCatsByTag")
.WithOpenApi();
#endregion

#region apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}
#endregion

app.Run();
