namespace DAL.Models
{
    public class Priem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PriemGroup Group { get; set; }
    }
}
