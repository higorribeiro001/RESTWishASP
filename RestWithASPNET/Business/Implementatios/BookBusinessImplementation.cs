using RestWithASPNET.Data.Converter.Implentations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Repository;

namespace RestWithASPNET.Business.Implementations
{
    public class BookBusinessImplementation : IBookBusiness // vai implementar os metodos desta class
    {
        private readonly IRepository<Book> _repository; // quem vai acessar diretamente no contexto com mysql é o repository

        private readonly BookConverter _converter;

        public BookBusinessImplementation(IRepository<Book> repository)
        {
            _repository = repository;
            _converter = new BookConverter();
        }

        public List<BookVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public BookVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }

        public BookVO Create(BookVO book)
        {
            var bookEntity = _converter.Parse(book); // recebe o VO, parsea ele para a entidade
            bookEntity = _repository.Create(bookEntity);

            return _converter.Parse(bookEntity);
        }

        public BookVO Update(BookVO book)
        {
            var bookEntity = _converter.Parse(book); // recebe o VO, parsea ele para a entidade
            bookEntity = _repository.Update(bookEntity);

            return _converter.Parse(bookEntity);
        }

        public Book Delete(long id)
        {
            _repository.Delete(id);
            return null;
        }
    }
}