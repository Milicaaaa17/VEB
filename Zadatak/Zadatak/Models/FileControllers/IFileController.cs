using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak.Models.FileControllers
{
    public interface IFileController<T>
    {
        List<T> GetAll();
        T Get(int id);
        void Delete(int id);
        void Update(int id, T obj);
        void Insert(T obj);
    }
}
