using Microsoft.EntityFrameworkCore;
using StealAllTheCats.API.Models;
using StealAllTheCats.API.Models.Data;
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
app.MapPost("/cats/fetch", async (ICatService catService, ICatRepository catRepository, ITagRepository tagRepository) =>
{
    var images = await catService.GetImages(5, "live_JjS14tf7HhTCCvlq98caJMrhXkykVmAwlnD5yyHcIEjbzgImrX3cQnKosQbrBrwX");
    
    var cats = new List<CatEntity>();

    foreach (var image in images)
    {
        var cat = new CatEntity(image);

        bool catExists = await catRepository.ExistsAsync(cat.CatId);

        if (!catExists)
        {
            await catRepository.AddAsync(cat);
        }

        foreach (var tag in cat.TagEntities)
        {
            bool tagExists = await tagRepository.ExistsAsync(tag.Name);

            if (!tagExists)
            {
                await tagRepository.AddAsync(tag);
            }
        }

        cats.Add(cat);

        await catRepository.SaveChangesAsync();
    }

    return cats;
})
.WithName("FetchCats")
.WithOpenApi();

app.MapGet("/jobs/{id}", (string id) =>
{

})
.WithName("GetJobStatusById")
.WithOpenApi();

app.MapGet("/cats/{id}", async (ICatRepository catRepository, string id) =>
{
    return await catRepository.GetCatEntityAsync(id);
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
