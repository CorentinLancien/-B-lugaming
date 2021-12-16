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
            return await GameSrv.GetGames();
        }

        [HttpGet("/api/games/params")]
        public async Task<List<Game>> GetQueryString()
        {
            int prix = 0;
            string name = "";
            string categories = "";
            string year = "";

            if (HttpContext.Request.Query["prix"].ToString() != "")
            {
                prix = Int32.Parse(HttpContext.Request.Query["prix"]);
            }
            if (HttpContext.Request.Query["name"].ToString() != null)
            {
                name = HttpContext.Request.Query["name"].ToString();
            }
            if(HttpContext.Request.Query["name"].ToString() != null)
            {
               categories = HttpContext.Request.Query["categories"].ToString();
            }
            if(HttpContext.Request.Query["year"].ToString() != null)
            {
                year = HttpContext.Request.Query["year"].ToString();
            }

            if (GameSrv == null)
            {
                throw new Exception($"Le service {nameof(GameSrv)} n'est pas initialisé.");
            }

            return await GameSrv.GetGames(year, prix, name, categories);
        }
    }
}