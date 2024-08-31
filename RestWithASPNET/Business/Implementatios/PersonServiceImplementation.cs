using Microsoft.AspNetCore.Http.HttpResults;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository;

namespace RestWithASPNET.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness // vai implementar os metodos desta class
    {
        public readonly IPersonRepository _repository; // quem vai acessar diretamente no contexto com mysql é o repository

        public PersonBusinessImplementation(IPersonRepository repository)
        {
            _repository = repository;
        }

        public List<Person> FindAll()
        {
            return _repository.FindAll();
        }

        public Person FindByID(long id)
        {
            return _repository.FindByID(id);
        }

        public Person Create(Person person)
        {
            return _repository.Create(person);
        }

        public Person Update(Person person)
        {
            return _repository.Update(person);
        }

        public Person Delete(long id)
        {
            _repository.Delete(id);
            return null;
        }
    }
}