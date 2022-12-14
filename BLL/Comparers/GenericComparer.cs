using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Comparers
{
    public class GenericComparer<T>: IEqualityComparer<T>
    {
        private readonly Func<T, object> func;

        public GenericComparer(Func<T, object> func)
        {
            this.func = func;
        }

        public bool Equals([AllowNull] T x, [AllowNull] T y)
        {
            return func(x).Equals(func(y));
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return func(obj).GetHashCode();
        }
    }
}
