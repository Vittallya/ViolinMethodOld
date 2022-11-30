using DAL.Models;
using LiteDB;
using System;

namespace DAL.Repositories
{
    public class LiteDbStore : IStore
    {
        public object Database { get; }

        public LiteDbStore(ILiteDatabase database, string priemName = null, string priemGroupName = null, string notesName = null)
        {
            Database = database;
            Priems = new LiteDbRepo<Priem>(this, database, priemName);
            PriemGroups = new LiteDbRepo<PriemGroup>(this, database, priemGroupName);
            Notes = new LiteDbRepo<Note>(this, database, notesName);
        }

        public IStoreRepo<Priem> Priems { get; }

        public IStoreRepo<PriemGroup> PriemGroups {   get;}

        public IStoreRepo<Note> Notes { get; }
    }
}
