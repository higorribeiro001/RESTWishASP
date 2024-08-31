using Microsoft.AspNetCore.Http.HttpResults;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository;

namespace RestWithASPNET.Business.Implementations
{
    public class BookBusinessImplementation : IBookBusiness // vai implementar os metodos desta class
    {
        public readonly IRepository<Book> _repository; // quem vai acessar diretamente no contexto com mysql é o repository

        public BookBusinessImplementation(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public List<Book> FindAll()
        {
            return _repository.FindAll();
        }

        public Book FindByID(long id)
        {
            return _repository.FindByID(id);
        }

        public Book Create(Book book)
        {
            return _repository.Create(book);
        }

        public Book Update(Book book)
        {
            return _repository.Update(book);
        }

        public Book Delete(long id)
        {
            _repository.Delete(id);
            return null;
        }
    }
}