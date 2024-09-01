using RestWithASPNET.Data.Converter.Implentations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Repository;

namespace RestWithASPNET.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness // vai implementar os metodos desta class
    {
        private readonly IRepository<Person> _repository; // quem vai acessar diretamente no contexto com mysql é o repository

        private readonly PersonConverter _converter; // readonly é um atributo que so é atruido valor uma vez, depois não se altera

        public PersonBusinessImplementation(IRepository<Person> repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public List<PersonVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public PersonVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id)); //vai converter esta entidade para VO
        }

        public PersonVO Create(PersonVO person)
        {
            var personEntity = _converter.Parse(person); // recebe o VO, parsea ele para a entidade
            personEntity = _repository.Create(personEntity);

            return _converter.Parse(personEntity); // converte a entidade para VO
        }

        public PersonVO Update(PersonVO person)
        {
            var personEntity = _converter.Parse(person); // recebe o VO, parsea ele para a entidade
            personEntity = _repository.Update(personEntity);

            return _converter.Parse(personEntity); // converte a entidade para VO
        }

        public Person Delete(long id) //não precisa do VO pois so passa o id
        {
            _repository.Delete(id);
            return null;
        }
    }
}