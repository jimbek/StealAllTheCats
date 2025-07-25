using Microsoft.AspNetCore.Http.HttpResults;
using StealAllTheCats.API.Models.Data;
using StealAllTheCats.API.Models.DTOs;

namespace StealAllTheCats.API
{
    public static class CatEndpoints
    {
        public static WebApplication MapCatEndpoints(this WebApplication app)
        {
            app
                .MapGet("/cats/{id}", GetCat)
                .WithName("GetCatById")
                .WithTags("Cats")
                .WithOpenApi();

            app
                .MapGet("/cats", GetCats)
                .WithName("GetCatsByTag")
                .WithTags("Cats")
                .WithOpenApi();

            return app;
        }

        public static async Task<Results<Ok<Image>, BadRequest, NotFound>> GetCat(IUnitOfWork unitOfWork, CancellationToken token, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return TypedResults.BadRequest();
            }

            var cat = await unitOfWork.CatRepository.GetCatEntityAsync(token, id);

            if (cat == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(new Image(cat));
        }

        public static async Task<Results<Ok<IEnumerable<Image>>, BadRequest>> GetCats(IUnitOfWork unitOfWork, CancellationToken token, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return TypedResults.BadRequest();
            }

            var cats = await unitOfWork.CatRepository.GetCatEntitiesAsync(token, page, pageSize);

            return TypedResults.Ok(cats.Select(x => new Image(x)));
        }
    }
}
