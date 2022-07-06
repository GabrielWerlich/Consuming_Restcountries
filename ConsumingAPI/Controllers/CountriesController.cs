using System.Text.Json;
using ConsumingAPI.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ConsumingAPI.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {   
        private readonly IMemoryCache _memoryCache;
        private const string COUNTRIES_KEY = "Countries";

        public CountriesController(IMemoryCache memoryCache)
        {
           _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            const string restCountriesUrl = "https://restcountries.com/v3.1/region/europe";

            if (_memoryCache.TryGetValue(COUNTRIES_KEY, out List<CountryViewModel> countries))
            {
                return Ok(countries);
            }
        
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(restCountriesUrl);

                var responseData = await response.Content.ReadAsStringAsync();

                countries = JsonSerializer.Deserialize<List<CountryViewModel>>(responseData, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

                var memoryCacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                    SlidingExpiration = TimeSpan.FromSeconds(1200),
                }; 

                _memoryCache.Set(COUNTRIES_KEY, countries, memoryCacheEntryOptions);

                return Ok(countries);
            }

        }
       

    }
    
}