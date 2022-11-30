namespace Main.ViewModels
{
    public class PriemModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PriemGroupModel Group { get; set; }
    }
}