using Microsoft.AspNetCore.Mvc;
using MockBookingSystem.Models;
using MockBookingSystem.Managers;

namespace MockBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SearchController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<ActionResult<SearchRes>> Search([FromBody] SearchReq request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var httpClient = _httpClientFactory.CreateClient();
            var manager = ManagerFactory.CreateManager(request, httpClient);
            var result = await manager.Search(request);
            return Ok(result);
        }
    }
}
