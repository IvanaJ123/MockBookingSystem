using Microsoft.AspNetCore.Mvc;
using MockBookingSystem.Models;
using MockBookingSystem.Managers;

namespace MockBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BookController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<ActionResult<BookRes>> Book([FromBody] BookReq request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var httpClient = _httpClientFactory.CreateClient();
            var manager = ManagerFactory.CreateManager(request.SearchReq, httpClient);
            var result = await manager.Book(request);
            return Ok(result);
        }
    }
}
