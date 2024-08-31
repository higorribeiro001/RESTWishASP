using RestWithASPNET.Model;
using RestWithASPNET.Model.Base;
using System.Collections.Generic;

namespace RestWithASPNET.Repository
{
    public interface IRepository<T> where T : BaseEntity // o tipo T vai precisar extender o BaseEntity
    {
        T Create(T item);
        T FindByID(long id);
        List<T> FindAll();
        T Update(T item);
        T Delete(long id);
        bool Exists(long id);
    }
}