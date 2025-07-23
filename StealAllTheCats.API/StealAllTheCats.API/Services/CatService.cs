using StealAllTheCats.API.Models.DTOs;
using System.Text.Json;

namespace StealAllTheCats.API.Services
{
    public class CatService : ICatService
    {
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://api.thecatapi.com/v1"),
        };

        public async Task<List<Image>> GetImages(int limit, string apiKey)
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"images/search?limit={limit}&api_key={apiKey}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(jsonResponse))
                {
                    return JsonSerializer.Deserialize<List<Image>>(jsonResponse) ?? [];
                }
            }

            return [];
        }
    }
}
