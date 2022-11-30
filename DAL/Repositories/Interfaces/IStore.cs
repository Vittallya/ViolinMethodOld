using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IStore<TDb>
    {
        public TDb Database { get; }
        public IStoreRepo<Priem> Priems { get; }
        public IStoreRepo<PriemGroup> PriemGroups { get; }
        public IStoreRepo<Note> Notes { get; }
    }
}
