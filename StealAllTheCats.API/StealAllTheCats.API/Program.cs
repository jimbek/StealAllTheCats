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
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=StealAllTheCats;Trusted_Connection=True;TrustServerCertificate=true;"));

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region define requests
app.MapPost("/cats/fetch", async (CancellationToken token, IQueue queue, ICatService catService, IUnitOfWork unitOfWork) =>
{
    Guid id = Guid.NewGuid();

    var payload = new CancellationTokenPayload(id, "live_JjS14tf7HhTCCvlq98caJMrhXkykVmAwlnD5yyHcIEjbzgImrX3cQnKosQbrBrwX");

    queue.QueueInvocableWithPayload<BulkInsertToDb, CancellationTokenPayload?>(payload);

    await unitOfWork.JobRepository.AddAsync(new Job(id));
    await unitOfWork.SaveChangesAsync();

    return id;
})
.WithName("FetchCats")
.WithOpenApi();

app.MapGet("/jobs/{id}", async (IUnitOfWork unitOfWork, string id) =>
{
    var job = await unitOfWork.JobRepository.GetJobAsync(Guid.Parse(id));

    if (job == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(Enum.GetName(job.Status));
})
.WithName("GetJobStatusById")
.WithOpenApi();

app.MapGet("/cats/{id}", async (IUnitOfWork unitOfWork, string id) =>
{
    var cat = await unitOfWork.CatRepository.GetCatEntityAsync(id);

    if (cat == null)
    {
        return Results.NotFound();
    }

    cat.Url = Image.Prefix + cat.Image + Image.Suffix;

    return Results.Ok(cat);
})
.WithName("GetCatById")
.WithOpenApi();

app.MapGet("/cats", (ICatService catService, string tag = "", int page = 1, int pageSize = 10) =>
{

})
.WithName("GetCatsByTag")
.WithOpenApi();
#endregion

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
