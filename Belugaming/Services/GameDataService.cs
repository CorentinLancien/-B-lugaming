using Belugaming.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Game>> GetGames(int categorieId)
        {
            List<Game> games = new List<Game>();
            List<Categorie> categories = await _Context.Categories
                .Include(i => i.Games)
                .Where(i => i.Id == categorieId)
                .ToListAsync();


            foreach (var categorie in categories)
            {
                foreach (var game in categorie.Games)
                {
                    games.Add(game);
                }
            }
            return games;
        }
        #endregion

    }
}
