using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;

namespace RestWithASPNET.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO person);
        BookVO FindByID(long id);
        List<BookVO> FindAll();
        BookVO Update(BookVO person);
        Book Delete(long id);
    }
}