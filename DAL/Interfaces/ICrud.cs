using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentServices.DAL.Interfaces
{
    public interface ICrud<T>
    {
        IEnumerable<T> GetAll();
        T GetByName(string name);
        void Insert(T obj);
        void Update(T obj);
        void Delete(int id);
    }
}