using DAL.Models;
using LiteDB;

namespace Main.Options.LiteDb
{
    public class LiteDbMapper: BsonMapper
    {
        public LiteDbMapper()
        {
            EnumAsInteger = true;

            Entity<PageInfo>().DbRef(x => x.Priems);
            Entity<Priem>().DbRef(x => x.Group);
        }
    }
}
