using Belugaming;
using Belugaming.Data;
using Belugaming.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Security;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//EntityFramwork & db
builder.Services.AddDbContext<BelugamingContext>(options => options.UseSqlite(@"Data Source=.\bin\belugaming.db;"));
builder.Services.AddTransient<CategorieDataService>();
builder.Services.AddTransient<GameDataService>();

builder.Services.AddCors();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

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
        int compteur = 0;
        BelugamingContext context = scope.ServiceProvider.GetService<BelugamingContext>() ?? throw new Exception($"Impossible d'initialiser le service {nameof(BelugamingContext)}");

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Categorie strategieCategory = new Categorie()
        {
            Name = "strategie"
        };
        Categorie rpgCategory = new Categorie()
        {
            Name = "rpg"
        };
        Categorie fpsCategory = new Categorie()
        {
            Name = "fps"
        };
        Categorie arcadeCategory = new Categorie()
        {
            Name = "arcade"
        };
        Categorie mmoCategory = new Categorie()
        {
            Name = "mmo"
        };
        Categorie singleplayerCategory = new Categorie()
        {
            Name = "singleplayer"
        };
        Categorie cooperationCategory = new Categorie()
        {
            Name = "cooperation"
        };
        Categorie adventureCategory = new Categorie()
        {
            Name = "adventure"
        };

        context.Categories.Add(strategieCategory);
        context.Categories.Add(rpgCategory);
        context.Categories.Add(fpsCategory);

        Categorie[] categories = new Categorie[]
        {
                strategieCategory,
                rpgCategory,
                fpsCategory,
                arcadeCategory,
                mmoCategory, 
                singleplayerCategory,
                cooperationCategory,
                adventureCategory,
        };

        Func<Task<Game>> generateGameAsync = async () =>
        {
            return await Task.Run(async () =>
            {
                Game game = new Game()
                {
                    Date = DateTime.Now.AddDays(rnd.NextDouble() * -1 * rnd.Next(0, 365)),
                    Prix = Convert.ToInt16(Math.Floor(rnd.NextDouble() * rnd.Next(0, 80))),
                    Name = getName(compteur),
                };
                compteur++;

                for (int i = 0; i < rnd.Next(1, 3); i++)
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

        context.Games.AddRange(generateGamesAsync(20));

        context.SaveChanges();
    }
}

string getName(int compteur)
{
    List<string> names = new List<string>(new string[] { "League of legends", "Mario party", "Mario Galaxy", "Zelda Breath of the Wild", "Donjons & Dragons", "Battlefield 1", "Call of Duty : Black Ops", "Counter Strike", "Pokemon", "Overwatch", "Starcraft 2", "World of Warcraft", "Diablo 2", "Rocket League", "Worms", "Nintendogs", "Escape from Tarkov", "Final Fantasy XI", "Dead Cells", "Tekken 7" });

    return names[compteur];
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

    app.UseCors(
        options => options.WithOrigins("http://localhost:8080").AllowAnyMethod()
    );


app.UseRouting();

app.MapControllers();

app.Run();

