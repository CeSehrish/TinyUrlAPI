using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace TinyUrlWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;

            var authorizationTokenRebrandly = _configuration["BitlyConfig:AuthorizationTokenRebrandly"];
            _httpClient.DefaultRequestHeaders.Add("Authorization", authorizationTokenRebrandly);
        }
        [HttpPost]
        [Route("getTinyUrlBitly")]
        public async Task<IActionResult> getTinyUrlBitly(string longpath)
        {

            try
            {
                var endpoint = _configuration["BitlyConfig:Endpointbitly"];
                var request = new
                {
                    long_url = longpath,
                    domain = "bit.ly",
                    custom_bitlinks = new[]
                    {
                        new
                        {
                            title = "Sehrishkhan",
                            domain = "bit.ly"
                        }
                    }
                };

                var response = await _httpClient.PostAsJsonAsync(endpoint, request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<BitlyShortenResponse>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("getTinyUrl")]
        public async Task<IActionResult> GetTinyUrl(string longUrl, string alias)
        {
            var endpoint = _configuration["BitlyConfig:EndPointRebrandly"];
            var authorizationToken = _configuration["BitlyConfig:AuthorizationTokenRebrandly"];
            var baseUriRebrandly = _configuration["BitlyConfig:BaseUriRebrandly"];

            var payload = new
            {
                destination = longUrl,

                domain = new
                {
                    fullName = "rebrand.ly"
                }
                ,
                slashtag = alias
                //, title = "Rebrandly YouTube channel"
            };

            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUriRebrandly.ToString()) })
            {
                httpClient.DefaultRequestHeaders.Add("apikey", authorizationToken);
                var body = new StringContent(JsonConvert.SerializeObject(payload), UnicodeEncoding.UTF8, "application/json");

                BitlyShortenResponse tempObj = new BitlyShortenResponse();
                tempObj.shortUrl = "rebrand.ly/A_Ce_SLASHTAG";

                return Ok(tempObj);
                /* using (var response = await httpClient.PostAsync("/v1/links", body))
                 {
                     if (response.StatusCode.ToString().ToLower() == "forbidden")
                     {
                         BitlyShortenResponse obj= new BitlyShortenResponse();
                         obj.StatusCode = "forbidden";
                         return Ok(obj);
                     }
                     response.EnsureSuccessStatusCode();
                     var responseJson = await response.Content.ReadAsStringAsync();
                     var result = JsonConvert.DeserializeObject<BitlyShortenResponse>(responseJson);


                     return Ok(result);
                 }*/
            }

        }

    }
    public class BitlyShortenResponse
    {
        public string shortUrl { get; set; }
        public string StatusCode { get; set; }

    }
}
