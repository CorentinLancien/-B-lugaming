using Microsoft.EntityFrameworkCore;

namespace Belugaming.Data
{
    public class BelugamingContext : DbContext
    {
        #region Properties

        /// <summary>
        ///     Obtient ou définit le jeux de données des <see cref="Games"/>.
        /// </summary>
        public DbSet<Game> Games { get; set; }

        /// <summary>
        ///     Obtient ou définit le jeux de données des <see cref="Games"/>.
        /// </summary>
        public DbSet<Categorie> Categories  { get; set; }


        #endregion

        #region Constructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="BelugamingContext"/>.
        /// </summary>
        public BelugamingContext()
        {
            Games = Games ?? throw new Exception($"Le jeux de données {nameof(Games)} n'est pas initialisé.");
            Categories = Categories ?? throw new Exception($"Le jeux de données {nameof(Categories)} n'est pas initialisé.");
        }

        public BelugamingContext(DbContextOptions<BelugamingContext> options)
        : base(options)
        {
            Games = Games ?? throw new Exception($"Le jeux de données {nameof(Games)} n'est pas initialisé.");
            Categories = Categories ?? throw new Exception($"Le jeux de données {nameof(Categories)} n'est pas initialisé.");
        }


        #endregion

        #region  Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(e =>
            {
                e.HasKey(e => e.Id);

                e.HasMany(p => p.Categories)
                .WithMany(d => d.Games);

            });

            modelBuilder.Entity<Categorie>(e =>
            {
                e.HasKey(e => e.Id);

            });
        }

        #endregion
    }
}
