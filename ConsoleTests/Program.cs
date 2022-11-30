using DAL.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var notes = db.GetCollection<Note>().
                Include(BsonExpression.Create("$.PageInfo[*].Priems[*]")).
                Include(BsonExpression.Create("$.PageInfo[*].Priems[*].Group")).Query().First();

            var priems = db.GetCollection<Priem>().
                Include(x => x.Group).Query().ToList();

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
