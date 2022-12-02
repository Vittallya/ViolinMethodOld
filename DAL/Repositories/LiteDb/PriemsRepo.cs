using DAL.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    class PriemsRepo : LiteRepository, IStoreRepo<Priem>
    {
        public IStore Store { get; }

        public PriemsRepo(IStore store, ILiteDatabase db, string tableName): base(db)
        {
            Store = store;
            TableName = tableName;
        }

        public string TableName { get; }

        public void Delete(object id)
        {
            Delete<Priem>(new BsonValue( id));
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Priem FindById(object id)
        {
            return Database.GetCollection<Priem>().FindById(new BsonValue(id));
        }

        public Task<Priem> FindByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Priem> GetAll()
        {
            return Database.GetCollection<Priem>().Include(x => x.Group).FindAll();
        }

        public Task<IEnumerable<Priem>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Priem> GetAllClear()
        {
            return Database.GetCollection<Priem>().FindAll();

        }

        public Task<IEnumerable<Priem>> GetAllClearAsync()
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            return Database.GetCollection<Priem>().Count();
        }

        public void Insert(Priem item)
        {
            Insert<Priem>(item);
        }

        public void Insert(IEnumerable<Priem> items)
        {
            Insert<Priem>(items);
        }

        public Task InsertAsync(Priem item)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(IEnumerable<Priem> items)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(Priem item)
        {
            Upsert(item);
        }

        public Task InsertOrUpdateAsync(Priem item)
        {
            throw new NotImplementedException();
        }

        public void Update(Priem item)
        {
            Update<Priem>(item);
        }

        public Task UpdateAsync(Priem item)
        {
            throw new NotImplementedException();
        }
    }
}
