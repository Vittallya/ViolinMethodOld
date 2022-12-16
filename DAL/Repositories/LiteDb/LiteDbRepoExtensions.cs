using DAL.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var coll = db.GetCollection<Note>().Query();

            if (guids == null && priems == null)
            {
                return coll.Skip(skip).Limit(take).ToEnumerable();
            }
            else
            {
                ILiteQueryable<Note> queryable = default;

                if(priems != null)
                {
                    IEnumerable<BsonExpression> queries = priems.
                        Select(y => Query.Any().EQ("$.PageInfo[*].Priems[*].$id", new BsonValue(y)));
                    BsonExpression all = queries.First();

                    queries.Skip(1).Select(y => all = Query.And(all, y));
                    queryable = coll.Where(all);
                }

                if(guids != null)
                {
                    IEnumerable<BsonExpression> queries = guids.
                        Select(y => Query.Any().EQ("$.id", new BsonValue(y)));
                    BsonExpression all = queries.First();

                    queries.Skip(1).Select(y => all = Query.And(all, y));
                    queryable = coll.Where(all);
                }

                return queryable.ToEnumerable();
            }

            return db.GetCollection<Note>(noteRepo.TableName).
                Query().
                Skip(skip).
                Limit(take).
                ToEnumerable();
        }
    }
}
