using PassMoveAPI.Data.Entities;

namespace PassMoveAPI.Data.Repository
{
    public interface IPersonRepository
    {
        Task AddPersonAsync(Person model);
        Task<IEnumerable<Person>> GetPersonsAsync();
        Task<Person> GetPerson(string name);
    }
}
