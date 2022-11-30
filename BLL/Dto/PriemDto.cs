namespace BLL.Dto
{
    public class PriemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PriemGroupDto Group { get; set; }
    }
}