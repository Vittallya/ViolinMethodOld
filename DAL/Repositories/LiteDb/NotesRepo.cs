using DAL.Models;
using LiteDB;
using System.Collections.Generic;

namespace DAL.Repositories.LiteDb
{
    internal class NotesRepo : LiteDbRepo<Note>
    {
        public NotesRepo(IStore store, ILiteDatabase database, string tableName = null) : base(store, database, tableName)
        {
        }

        public override IEnumerable<Note> GetAll()
        {
            return Database.GetCollection<Note>().
                Include("$.PageInfo[*].Priems[*]").
                Include("$.PageInfo[*].Priems[*].Group").
                Query().
                ToEnumerable();
        }

    }
}
