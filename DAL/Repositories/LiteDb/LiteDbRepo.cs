using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LiteDbRepo<T> : LiteRepository, IStoreRepo<T>
    {
        public LiteDbRepo(IStore store, ILiteDatabase database, string tableName = null) : base(database)
        {
            Store = store;
            TableName = tableName ?? typeof(T).Name;
        }

        public string TableName { get; }

        public IStore Store { get; }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public T FindById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllClear()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllClearAsync()
        {
            throw new NotImplementedException();
        }

        public void Insert(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(T item)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(T item)
        {
            throw new NotImplementedException();
        }

        public Task InsertOrUpdateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }
    }
}
