namespace Belugaming
{
    public class Game
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }

        public int Prix { get; set; }

        public int Name { get; set; }
        
        public List<Categorie>? Categories { get; set; }
        
    }
}