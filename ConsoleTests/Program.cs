using DAL.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            LiteDatabase db = new LiteDatabase(new ConnectionString()
            {
                Filename = "test.db"

            }, new mapper());
            int priemsCount = 100;
            //RecreateHashtags(db, priemsCount);
            //RecreateNotes(db, priemsCount);

            var allNotesCount = db.GetCollection<Note>().Count();
            var AllGroups = db.GetCollection<PriemGroup>().Count();

            var notes = db.GetCollection<Note>().
                Include(BsonExpression.Create("$.PageInfo[*].Priems[*]")).
                Include(BsonExpression.Create("$.PageInfo[*].Priems[*].Group")).Query().First();

            //var priems = db.GetCollection<Priem>().FindAll();

            //IEnumerable<BsonExpression> queries = new[] { 3 }.
            //            Select(y => Query.Any().EQ("$.Group.$id", new BsonValue(y)));

            var b = db.GetCollection<Note>().
                Include("$.PageInfo[*].Priems[*]").
                Include("$.PageInfo[*].Priems[*].Group").
                Find(BsonExpression.Create("$.PageInfo[*].Priems[*].Group.$id ANY = @0", 1)).ToList();


            var priems2 = db.GetCollection<Priem>().
                Find(BsonExpression.Create("$.Group.$id = @0", 1)).Select(x => x.Id).ToArray();


            var c = db.GetCollection<Note>().
                Include("$.PageInfo[*].Priems[*]").
                FindAll().
                Where(x => x.PageInfo.Any(x => x.Priems.Any(x => priems2.Contains( x.Id)))).ToList();

            var d = db.GetCollection<Note>().
                Include("$.PageInfo[*].Priems[*]").
                Include("$.PageInfo[*].Priems[*].Group").
                FindAll().
                Where(x => x.PageInfo.Any(x => x.Priems.Any(x => x.Group.Id == 1))).ToList();


            //var c = db.GetCollection<Note>().Query().Where(x)

            //BsonExpression all = queries.First();

            //queries.Skip(1).Select(y => all = Query.Or(all, y));

            var priemIds = db.GetCollection<Priem>().
                Query().
                Where("$.Group.$id ANY = @0", 3).
                Select(x => x.Id).
                ToList();

            var list = new List<Note>();
            var coll = db.GetCollection<Note>();


            

            var or = Query.Or(priemIds.Select(x => BsonExpression.Create("$.PageInfo[*].Priems[*].$id ANY = @0", x)).ToArray());


            var notes1 = db.GetCollection<Note>().
                Include("$.PageInfo[*].Priems[*]").
                Query().
                Where(or).
                ToList();


            IEnumerable<BsonExpression> queries2 = priemIds.Select(x => Query.Contains("ANY($.PageInfo[*].Priems[*].$id) = true", x.ToString()));
            BsonExpression all2 = queries2.First();

            queries2.Skip(1).Select(y => all2 = Query.Or(all2, y));

            var note2 = b.FirstOrDefault(x => x.PageInfo.All(x => x.Priems.All(x => x.Group.Id != 2)));

            db.Dispose();
        }

        private static void RecreateNotes(LiteDatabase db, int prCount)
        {
            db.DropCollection("Note");

            Random rand = new Random();
            int count = rand.Next(400, 600);

            for (int i = 0; i < count; i++)
            {
                int infoCount = rand.Next(2, 5);

                ILiteCollection<Note> coll = db.GetCollection<Note>();

                var note = new Note
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    FileName = "file.fs",
                    LastUpdate = new DateTime(10, 10, 10),
                    PublishDate = new DateTime(10, 10, 10),
                };

                note.PageInfo = Enumerable.Range(0, 5).Select(y => rand.Next(1, 105)).Select(x =>
                {
                    int priemsCount = rand.Next(1, 10);

                    var pageInfo = new PageInfo()
                    {
                        Priems = Enumerable.Range(0, priemsCount).Select(x =>
                        {
                            return new Priem { Id = rand.Next(prCount) };
                        }).ToList(),
                        PageNumber = x,
                    };
                    return pageInfo;
                }).ToList();


                coll.Insert(note);
            }
        }

        private static void RecreateHashtags(LiteDatabase db, int prCount)
        {
            db.DropCollection("PriemGroup");
            db.DropCollection("Priem");
            var groups = new List<PriemGroup>
            {
                new PriemGroup { Id = 1, Name = "Тональности" },
                new PriemGroup { Id = 2, Name = "Жанры" },
                new PriemGroup { Id = 3, Name = "Штрихи" }
            };

            db.GetCollection<PriemGroup>().Insert(groups);
            db.GetCollection<Priem>().Insert(GeneratePriem(prCount, groups));
        }


        public static IEnumerable<Priem> GeneratePriem(int count, IReadOnlyList<PriemGroup> groups)
        {
            return Enumerable.Range(0, count).Select(x =>
            {
                return new Priem
                {
                    Id = x + 1,
                    Name = "прием_" + x,
                    Group = groups[new Random().Next(groups.Count)]
                };
            });
        }

    }
}
