using DAL.Models;
using LiteDB;
using System;

namespace DAL.Repositories
{
    public class LiteDbStore : IStore<ILiteDatabase>
    {
        public ILiteDatabase Database { get; }

        public LiteDbStore(ILiteDatabase database, string priemName = null, string priemGroupName = null, string notesName = null)
        {
            Database = database;
            Priems = new LiteDbRepo<Priem>(database, priemName);
            PriemGroups = new LiteDbRepo<PriemGroup>(database, priemGroupName);
            Notes = new LiteDbRepo<Note>(database, notesName);
        }

        public IStoreRepo<Priem> Priems { get; }

        public IStoreRepo<PriemGroup> PriemGroups {   get;}

        public IStoreRepo<Note> Notes { get; }
    }
}
