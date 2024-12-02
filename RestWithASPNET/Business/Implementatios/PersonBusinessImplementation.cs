using RestWithASPNET.Data.Converter.Implentations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
using RestWithASPNET.Model;
using RestWithASPNET.Repository;

namespace RestWithASPNET.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness // vai implementar os metodos desta class
    {
        private readonly IPersonRepository _repository; // quem vai acessar diretamente no contexto com mysql é o repository

        private readonly PersonConverter _converter; // readonly é um atributo que so é atruido valor uma vez, depois não se altera

        public PersonBusinessImplementation(IPersonRepository repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public List<PersonVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public PagedSearchVO<PersonVO> FindWithPagedSearch(string? name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection)) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            string query = @"select * from person p where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(name)) query = query + $" and p.first_name like '%{name}%' ";
            query += $" order by p.first_name {sort} limit {size} offset {offset}"; 
            string countQuery = @"select count(*) from person p where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(name)) countQuery = countQuery + $" and p.first_name like '%{name}%' ";
            var persons = _repository.FindWithPagedSearch(query);
            int totalResults = _repository.GetCount(countQuery);

            return new PagedSearchVO<PersonVO> {
                CurrentPage = page,
                List = _converter.Parse(persons),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults,
            };
        }

        public PersonVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id)); //vai converter esta entidade para VO
        }

        public List<PersonVO> FindByName(string firstName, string lastName)
        {
            return _converter.Parse(_repository.FindByName(firstName, lastName));
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

        public PersonVO Disable(long id)
        {
            var personEntity = _repository.Disable(id);
            return _converter.Parse(personEntity);
        }

        public Person Delete(long id) //não precisa do VO pois so passa o id
        {
            _repository.Delete(id);
            return null;
        }
    }
}