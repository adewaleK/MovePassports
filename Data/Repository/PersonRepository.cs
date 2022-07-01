using Microsoft.EntityFrameworkCore;
using PassMoveAPI.Data.Entities;

namespace PassMoveAPI.Data.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _appDbContext;
        public PersonRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddPersonAsync(Person model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            await _appDbContext.Persons.AddAsync(model);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Person> GetPerson(string name)
        {
            var result = await _appDbContext.Persons.FirstOrDefaultAsync(x => x.Name.ToLower() == name);
            return result == null ? null : result;
        }

        public async Task<IEnumerable<Person>> GetPersonsAsync()
        {
            return await _appDbContext.Persons.ToListAsync();
        }
    }
}
