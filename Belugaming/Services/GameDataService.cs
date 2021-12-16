using Belugaming.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Belugaming.Services
{
    public class GameDataService
    {
        #region Fields
        private readonly BelugamingContext _Context;

        private List<Game> Games = new List<Game>();

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

            Games =  await _Context.Games
                .AsNoTracking()
                .Include(i => i.Categories).ToListAsync();

            return Games;
        }

        public async Task<List<Game>> GetGames(string year, int Prix, string Name, string Categories)
        {
            GetGames();

            if (Categories != "")
            {
                List<Game> GameTemp = new List<Game>();
                string[] CategoriesArray = Categories.Split("/");
                foreach (string Category in CategoriesArray)
                {
                    try
                    {
                        Categorie categorie =  _Context.Categories
                            .Include(i => i.Games)
                            .Where(i => i.Name.ToLower().Contains(Category.ToLower()))
                            .FirstOrDefault();

                        if (categorie != null)
                        {
                            foreach (Game game in categorie.Games)
                            {
                                foreach (var child in game.Categories)
                                {
                                    child.Games = new List<Game>();
                                }
                                GameTemp.Add(game);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                Games = GameTemp;
            }

            if (year != "")
            {
                try
                {
                    List<Game> gamesTemps = new List<Game>();
                    DateTime Year = new DateTime(Convert.ToInt32(year), 1, 1);
                    var gameQuery = Games
                    .Where(i => i.Date.Year == Year.Year);

                    foreach (var queryGame in gameQuery)
                    {
                        foreach (var child in queryGame.Categories)
                        {
                            child.Games = new List<Game>();
                        }
                        gamesTemps.Add(queryGame);
                    }
                    Games = gamesTemps;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            if (Prix != 0)
            {
                List<Game> GameQuery = Games
                    .Where(i => i.Prix != Prix)
                    .ToList();

                foreach (Game queryGame in GameQuery)
                {
                    Games.Remove(queryGame);
                }
            }
            if(Name != "")
            {
                List<Game> GameQuery = Games
                   .Where(i => i.Name.ToLower().Contains(Name.ToLower()))
                   .ToList();

                Games = GameQuery;
            }
 
            Games = Games.Distinct().ToList();
            return Games;
        }
        #endregion

    }
}
