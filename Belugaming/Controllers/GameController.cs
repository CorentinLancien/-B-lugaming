using Belugaming.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Belugaming.Controllers
{
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {

        private GameDataService? GameSrv { get; set; }

        private CategorieDataService? CategorieSrv { get; set;}

        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger, GameDataService gameSrv, CategorieDataService CategorieSrv)
        {
            _logger = logger;
            this.GameSrv = gameSrv;
            this.CategorieSrv = CategorieSrv;
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

        [HttpGet("/api/game/save/{id}")]
        public async void saveGame(int id)
        {
            GameSrv.saveGame(id);
        }

        [HttpGet("/api/categories")]
        public async Task<List<Categorie>> GetCategories()
        {
            if (CategorieSrv == null)
            {
                throw new Exception($"Le service {nameof(CategorieSrv)} n'est pas initialisé.");
            }

            return await CategorieSrv.GetCategories();
        }
    }
}