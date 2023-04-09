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
                Include(BsonExpression.Create("$.PageInfo[*].Priems[*].Group")).
                FindById(id);
        }

        public static IEnumerable<Note> GetNotes(this LiteDbRepo<Note> noteRepo,
                                                 int take,
                                                 int skip,
                                                 IEnumerable<Guid> guids = null,
                                                 IEnumerable<int> priems = null,
                                                 int? groupId = null)
        {
            var db = noteRepo.Database;

            var coll = db.GetCollection<Note>().Query();

            if (guids == null && priems == null && !groupId.HasValue)
            {
                return coll.Skip(skip).Limit(take).ToEnumerable();
            }
            else
            {
                if(priems != null && priems.Any())
                {
                    BsonExpression[] expressions = priems.
                        Select(x => Query.Any().EQ("$.PageInfo[*].Priems[*].$id", x)).
                        ToArray();

                    BsonExpression query = expressions.Length > 1 ? Query.And(expressions) : expressions[0];
                    coll = coll.Where(query);
                }

                if(guids != null && guids.Any())
                {
                    BsonExpression[] expressions = guids.
                        Select(x => Query.Any().EQ("$.id", x)).
                        ToArray();
                    BsonExpression query = expressions.Length > 1 ? Query.And(expressions) : expressions[0];
                    coll = coll.Where(query);
                }

                if(groupId.HasValue)
                {
                    //return db.GetCollection<Note>().
                    //    Include("$.PageInfo[*].Priems[*]").
                    //    Include("$.PageInfo[*].Priems[*].Group").
                    //    Find(x => x.PageInfo.Any(x => x.Priems.Any(x => x.Group.Id == groupId.Value)));

                    coll = coll.
                        Include("$.PageInfo[*].Priems[*]").
                        Include("$.PageInfo[*].Priems[*].Group").
                        Where("$.PageInfo[*].Priems[*].Group.$id ANY = @0", groupId.Value);

                }

                return coll.ToEnumerable();
            }

            

            //return db.GetCollection<Note>(noteRepo.TableName).
            //    Query().
            //    Skip(skip).
            //    Limit(take).
            //    ToEnumerable();
        }

        public static int GetNotesCount(this LiteDbRepo<Note> repo, IEnumerable<int> priemsId)
        {
            var db = repo.Database;

            if(priemsId != null && priemsId.Any())
            {

                BsonExpression query = default;

                if(priemsId.Count() > 1)
                {
                    query = Query.And(priemsId.Select(x => Query.Any().EQ("$.PageInfo[*].Priems[*].$id", x)).ToArray());
                }
                else
                {
                    query = Query.Any().EQ("$.PageInfo[*].Priems[*].$id", priemsId.First());
                }

                int count = db.GetCollection<Note>().Query().Where(query).Count();
                return count;

            }
            return db.GetCollection<Note>().Count();
        }
    }
}
