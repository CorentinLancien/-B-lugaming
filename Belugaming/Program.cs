using Belugaming;
using Belugaming.Data;
using Belugaming.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//EntityFramwork & db
builder.Services.AddDbContext<BelugamingContext>(options => options.UseSqlite(@"Data Source=.\bin\belugaming.db;"));
builder.Services.AddTransient<CategorieDataService>();
builder.Services.AddTransient<GameDataService>();

// JWT & User services
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IUserRepositoryService, UserRepositoryService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
	opt.TokenValidationParameters = new()
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
		ValidateLifetime = true,
		ValidateIssuer = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidateAudience = true,
		ValidAudience = builder.Configuration["Jwt:Issuer"],
	};
});


var app = builder.Build();

bool generateFakeData = true;

if (generateFakeData == true)
{
    Random rnd = new Random();

    using (IServiceScope scope = app.Services.CreateScope())
    {
        BelugamingContext context = scope.ServiceProvider.GetService<BelugamingContext>() ?? throw new Exception($"Impossible d'initialiser le service {nameof(BelugamingContext)}");

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Categorie strategieCategory = new Categorie()
        {
            Name = "Strategie"
        };
        Categorie rpgCategory = new Categorie()
        {
            Name = "RPG"
        };
        Categorie fpsCategory = new Categorie()
        {
            Name = "FPS"
        };

        context.Categories.Add(strategieCategory);
        context.Categories.Add(rpgCategory);
        context.Categories.Add(fpsCategory);

        Categorie[] categories = new Categorie[]
        {
                strategieCategory,
                rpgCategory,
                fpsCategory
        };

        Func<Task<Game>> generateGameAsync = async () =>
        {
            return await Task.Run(async () =>
            {
                Game game = new Game()
                {
                    Date = DateTime.Now.AddDays(rnd.NextDouble() * -1 * rnd.Next(0, 365)),
                    Prix = Convert.ToInt16(Math.Floor(rnd.NextDouble() * -1 * rnd.Next(0, 80))),
                    Name = "test",
                };



                for (int i = 0; i < rnd.Next(0, 3); i++)
                {
                    Categorie cat = categories[rnd.Next(0, categories.Length - 1)];

                    if (game.Categories.Contains(cat) == false)
                    {
                        game.Categories.Add(cat);
                    }
                }

                return game;
            });
        };

        Func<int, List<Game>> generateGamesAsync = (count) => Enumerable.Range(0, count).Select(async i => await generateGameAsync()).Select(t => t.Result).ToList();

        context.Games.AddRange(generateGamesAsync(50));

        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();
