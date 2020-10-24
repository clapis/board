using Board.Core.Abstractions;

namespace Board.Core.Entities
{
    public class PublicationType : Enumeration
    {
        public static PublicationType Free = new PublicationType(0, nameof(Free));
        public static PublicationType Featured = new PublicationType(1, nameof(Featured));
        public static PublicationType Scraped = new PublicationType(3, nameof(Scraped));

        private PublicationType(int id, string name) : base(id, name) { }
    }
}
