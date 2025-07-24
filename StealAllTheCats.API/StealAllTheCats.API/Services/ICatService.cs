using StealAllTheCats.API.Models.DTOs;

namespace StealAllTheCats.API.Services
{
    public interface ICatService
    {
        Task<List<Image>> GetImages(int limit, string apiKey);
    }
}
