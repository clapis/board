namespace Board.Core.Entities
{
    public class Company : BaseEntity
    {
        public Company(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string Summary { get; set; }

        public byte[] Logo { get; set; }

        public string Website { get; set; }
    }
}