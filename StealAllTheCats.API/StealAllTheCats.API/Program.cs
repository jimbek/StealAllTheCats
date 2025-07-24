using Microsoft.EntityFrameworkCore;
using StealAllTheCats.API.Models;
using StealAllTheCats.API.Models.Data;
using StealAllTheCats.API.Models.DTOs;
using StealAllTheCats.API.Services;

var builder = WebApplication.CreateBuilder(args);

#region services
builder
    .Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=StealAllTheCats;Trusted_Connection=True;TrustServerCertificate=true;"));

builder
    .Services
    .AddScoped<ICatRepository, CatRepository>()
    .AddScoped<ITagRepository, TagRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
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

#region requests
app.MapPost("/cats/fetch", async (CancellationToken token, ICatService catService, IUnitOfWork unitOfWork) =>
{
    var images = await catService.GetImages(token, 15, "live_JjS14tf7HhTCCvlq98caJMrhXkykVmAwlnD5yyHcIEjbzgImrX3cQnKosQbrBrwX");
    
    var cats = new List<CatEntity>();

    foreach (var image in images)
    {
        var cat = new CatEntity(image);

        bool catExists = await unitOfWork.CatRepository.ExistsAsync(token, cat.CatId);

        if (!catExists)
        {
            await unitOfWork.CatRepository.AddAsync(token, cat);
        }

        foreach (var tag in cat.TagEntities)
        {
            bool tagExists = await unitOfWork.TagRepository.ExistsAsync(token, tag.Name);

            if (!tagExists)
            {
                await unitOfWork.TagRepository.AddAsync(token, tag);
            }
        }

        cats.Add(cat);
    }

    await unitOfWork.SaveChangesAsync(token);

    return cats;
})
.WithName("FetchCats")
.WithOpenApi();

app.MapGet("/jobs/{id}", (string id) =>
{

})
.WithName("GetJobStatusById")
.WithOpenApi();

app.MapGet("/cats/{id}", async (CancellationToken token, IUnitOfWork unitOfWork, string id) =>
{
    var cat = await unitOfWork.CatRepository.GetCatEntityAsync(token, id);

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
