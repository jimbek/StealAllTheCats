using StealAllTheCats.API.Models;
using StealAllTheCats.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICatService, CatService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region requests
app.MapPost("/cats/fetch",async (ICatService catService) =>
{
    var images = await catService.GetImages(5, "live_JjS14tf7HhTCCvlq98caJMrhXkykVmAwlnD5yyHcIEjbzgImrX3cQnKosQbrBrwX");
    
    var cats = new List<CatEntity>();

    foreach (var image in images)
    {
        cats.Add(new CatEntity(image));
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

app.MapGet("/cats/{id}", (string id) =>
{
    
})
.WithName("GetCatById")
.WithOpenApi();

app.MapGet("/cats", (string tag = "", int page = 1, int pageSize = 10) =>
{

})
.WithName("GetCatsByTag")
.WithOpenApi();
#endregion

app.Run();
