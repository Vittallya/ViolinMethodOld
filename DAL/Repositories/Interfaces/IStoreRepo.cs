using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IStoreRepo<T>
    {
        string TableName { get; }
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllClear();
        T FindById(object id);
        void Delete(object id);
        void Insert(T item);
        void Insert(IEnumerable<T> items);
        void Update(T item);
        void InsertOrUpdate(T item);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllClearAsync();
        Task<T> FindByIdAsync(object id);
        Task DeleteAsync(object id);
        Task InsertAsync(T item);
        Task InsertAsync(IEnumerable<T> items);
        Task UpdateAsync(T item);
        Task InsertOrUpdateAsync(T item);
    }
}
