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
        #endregion

    }
}
