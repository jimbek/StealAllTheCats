using Coravel;
using Microsoft.EntityFrameworkCore;
using StealAllTheCats.API;
using StealAllTheCats.API.Jobs;
using StealAllTheCats.API.Models;
using StealAllTheCats.API.Models.Data;
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

#region define endpoints
app.MapJobEndpoints(builder);
app.MapCatEndpoints();
#endregion

#region apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}
#endregion

app.Run();
