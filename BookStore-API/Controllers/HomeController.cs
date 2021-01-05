using BookStore_API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILoggerService _logger;

        public HomeController(ILoggerService logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Get the Homepage
        /// </summary>
        /// <returns>Response</returns>
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogError("Hello world");
            return Ok("Hello world");
        }
    }
}