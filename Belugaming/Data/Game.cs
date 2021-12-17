
namespace Belugaming
{
    public class Game
    {
        public int? Id { get; set; }

        public DateTime Date { get; set; }

        public int Prix { get; set; }

        public string Name { get; set; }

        public bool isCommander { get; set; } = false;

        public List<Categorie>? Categories { get; set; } = new List<Categorie>();

    }
}