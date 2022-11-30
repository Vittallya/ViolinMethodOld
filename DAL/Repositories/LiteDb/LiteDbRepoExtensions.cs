using DAL.Models;
using LiteDB;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public static class LiteDbRepoExtensions
    {
        public static IEnumerable<Priem> GetPriemsWithIncluded(this LiteDbRepo<Priem> priemRepo)
        {
            var db = priemRepo.Database;
            return db.GetCollection<Priem>(priemRepo.TableName).
                Include(x => x.Group).Query().ToEnumerable();
        }

        public static Note GetNoteWithIncluded(this LiteDbRepo<Note> priemRepo, BsonValue id)
        {
            var db = priemRepo.Database;
            return db.GetCollection<Note>().
                Include(BsonExpression.Create("$.PageInfo[*].Priems[*]")).
                FindById(id);
        }
    }
}
