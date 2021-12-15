using Belugaming.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Belugaming.Services
{
    public class GameDataService
    {
        #region Fields
        private readonly BelugamingContext _Context;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="CategorieDataService"/>.
        /// </summary>
        /// <param name="context">Contexte de données à utiliser.</param>
        /// <exception cref="ArgumentNullException">Exception déclenchée si un argument obligatoire n'est pas fourni.</exception>
        public GameDataService(BelugamingContext context)
        {
            _Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region Methods
        public async Task<List<Game>> GetGames()
        {
            List<Game> Games = new List<Game>();

            var GameQuery = _Context.Games
                .Include(i => i.Categories);

            foreach (var game in GameQuery) {

                if(game.Categories != null)
                {
                    foreach (var child in game.Categories)
                    {
                        child.Games = new List<Game>();
                    }
                }

                Games.Add(game);
            }

            return Games;
        }

        public async Task<List<Game>> GetGames(int Prix, string Name, string Categories)
        {
            
            List<Game> Games = new List<Game>();


            if (Prix != 0)
            {
                var GameQuery = _Context.Games
                    .Include(i => i.Categories)
                    .Where(i => i.Prix == Prix);

                foreach (var queryGame in GameQuery)
                {
                    Games.Add(queryGame);
                }
            }
            if(Name != "")
            {
                var GameQuery = _Context.Games
                    .Include(i => i.Categories)
                    .Where(i => i.Name.Contains(Name));

                foreach (var queryGame in GameQuery)
                {
                    Games.Add(queryGame);
                }
            }
            if(Categories != "")
            {
                string[] CategoriesArray = Categories.Split("/");
                foreach (string Category in CategoriesArray)
                {
                    try
                    {
                        Categorie categorie = await _Context.Categories
                            .Include(i => i.Games)
                            .Where(i => i.Name == Category)
                            .FirstOrDefaultAsync();

                        if (categorie != null)
                        {
                            foreach (Game game in categorie.Games)
                            {
                                foreach (var child in game.Categories)
                                {
                                    child.Games = new List<Game>();
                                }
                                Games.Add(game);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
 
            Games = Games.Distinct().ToList();
            return Games;
        }
        #endregion

    }
}
