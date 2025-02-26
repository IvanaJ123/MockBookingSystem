using Microsoft.AspNetCore.Mvc;
using MockBookingSystem.Models;
using MockBookingSystem.Managers;

namespace MockBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckStatusController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CheckStatusController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<ActionResult<CheckStatusRes>> CheckStatus([FromBody] CheckStatusReq request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var httpClient = _httpClientFactory.CreateClient();
            var manager = ManagerFactory.CreateManager(new SearchReq(), httpClient); // We don't have the original SearchReq here, so we pass an empty one
            var result = await manager.CheckStatus(request);
            return Ok(result);
        }
    }
}
