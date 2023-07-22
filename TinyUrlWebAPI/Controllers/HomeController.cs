using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TinyUrlWebAPI.DAL;
using TinyUrlWebAPI.Models;

namespace TinyUrlWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly TinyURLsDAL _tinyURLsDAL;
        
        public HomeController(IConfiguration configuration, TinyURLsDAL tinyURLsDAL)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _tinyURLsDAL = tinyURLsDAL;

            var baseUriRebrandly = _configuration["RebrandlyConfig:BaseUriRebrandly"];
            var authorizationToken = _configuration["RebrandlyConfig:AuthorizationTokenRebrandly"];
            _httpClient.BaseAddress = new Uri(baseUriRebrandly.ToString());
            _httpClient.DefaultRequestHeaders.Add("apikey", authorizationToken);
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

                var result = await response.Content.ReadFromJsonAsync<TinyURLapiResponse>();
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
        public async Task<IActionResult> GetTinyUrl(string longUrl, string alias, int userId)
        {
            string endPointRebrandly = _configuration["RebrandlyConfig:EndPointRebrandly"].ToString();
            var payload = new
            {
                destination = longUrl,
                domain = new
                {
                    fullName = "rebrand.ly"
                },
                slashtag = alias
                //, title = "Rebrandly YouTube channel"
            };

            try
            {
                var body = new StringContent(JsonConvert.SerializeObject(payload), UnicodeEncoding.UTF8, "application/json");

                using (var response = await _httpClient.PostAsync(endPointRebrandly, body))
                {
                    if (response.StatusCode.ToString().ToLower() == "forbidden")
                    {
                        TinyURLapiResponse obj = new TinyURLapiResponse();
                        obj.statusCode = "forbidden";
                        return Ok(obj);
                    }
                    response.EnsureSuccessStatusCode();
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<TinyURLapiResponse>(responseJson);
                    _tinyURLsDAL.AddUserData(result, userId);
                    result.id = userId.ToString();

                    return Ok(result);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        
        [HttpGet]
        [Route("GetUserData")]
        public List<UrlLink> GetUserData(int userId)
        {
            List<UrlLink> userLinks =  _tinyURLsDAL.GetUserData(userId); 
            return userLinks;
        }
    }
}
