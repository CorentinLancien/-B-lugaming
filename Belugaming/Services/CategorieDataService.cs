using Belugaming.Data;

namespace Belugaming.Services
{
    public class CategorieDataService
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
        public CategorieDataService(BelugamingContext context)
        {
            _Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

    }
}
