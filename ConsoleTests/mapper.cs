using DAL.Models;
using LiteDB;

namespace ConsoleTests
{
    internal class mapper : BsonMapper
    {
        public mapper()
        {
            EnumAsInteger = true;

            Entity<PageInfo>().DbRef(x => x.Priems);
            Entity<Priem>().DbRef(x => x.Group);
        }
    }
}