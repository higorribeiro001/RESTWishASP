using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Business
{
    public interface IBookBusiness
    {
        Book Create(Book person);
        Book FindByID(long id);
        List<Book> FindAll();
        Book Update(Book person);
        Book Delete(long id);
    }
}