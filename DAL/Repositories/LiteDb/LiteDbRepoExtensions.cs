using DAL.Models;
using LiteDB;
using System;
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

        public static IEnumerable<Note> GetNotes(this LiteDbRepo<Note> noteRepo, int take, int skip, IEnumerable<Guid> guids = null, IEnumerable<int> priems = null)
        {
            var db = noteRepo.Database;
            return db.GetCollection<Note>(noteRepo.TableName).
                Query().
                Skip(skip).
                Limit(take).
                ToEnumerable();
        }
    }
}
