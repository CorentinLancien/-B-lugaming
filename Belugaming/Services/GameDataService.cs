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
            return await _Context.Games
                .Include(i => i.Categories)
                .ToListAsync();
        }

        public async Task<List<Game>> GetGames(string Categories)
        {
            
            string[] CategoriesArray = Categories.Split('/');
            List<Game> Games = new List<Game>();

            foreach (string Category in CategoriesArray)
            {
                try
                {
                    Categorie categorie = await _Context.Categories
                        .Include(i => i.Games)
                        .Where(i => i.Name == Category)
                        .FirstOrDefaultAsync();

                    foreach (Game game in categorie.Games)
                    {
                        foreach (var childCategorie in game.Categories)
                        {
                            childCategorie.Games = new List<Game>();        
                        }
                        Games.Add(game);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return Games;
        }
        #endregion

    }
}
