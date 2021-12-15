using Belugaming.Services;
using Microsoft.AspNetCore.Mvc;

namespace Belugaming.Controllers
{
    [ApiController]
    public class GameController : ControllerBase
    {

        private GameDataService? GameSrv { get; set; }

        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger, GameDataService gameSrv)
        {
            _logger = logger;
            this.GameSrv = gameSrv;
        }

        [HttpGet("/api/games")]
        public async Task<List<Game>> Get() {
            if (GameSrv == null)
            {
                throw new Exception($"Le service {nameof(GameSrv)} n'est pas initialisé.");
            }
            return GameSrv.GetGames();
        }

        [HttpGet("/api/games/{categorieId}")]
        public async Task<List<Game>> Get(int categorieId)
        {
            if (GameSrv == null)
            {
                throw new Exception($"Le service {nameof(GameSrv)} n'est pas initialisé.");
            }

            return await GameSrv.GetGames(categorieId);
        }
    }
}