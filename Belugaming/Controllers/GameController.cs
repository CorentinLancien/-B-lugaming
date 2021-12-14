using Microsoft.AspNetCore.Mvc;

namespace Belugaming.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Games")]
        public List<Game> Get()
        {
            return null;
        }
    }
}