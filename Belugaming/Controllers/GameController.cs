using Belugaming.Services;
using Microsoft.AspNetCore.Mvc;

namespace Belugaming.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {

        private GameDataService? GameSrv { get; set; }

        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Games")]
        public async Task<List<Game>> Get() {
            if (GameSrv == null)
            {
                throw new Exception($"Le service {nameof(GameSrv)} n'est pas initialisé.");
            }
            return await GameSrv.GetGames();
        }
    }
}