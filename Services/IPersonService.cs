using PassMoveAPI.Data.DTOs;
using PassMoveAPI.Data.Entities;

namespace PassMoveAPI.Services
{
    public interface IPersonService
    {
        Task<PersonDto> AddPersonAsync(FileModel file);
        Task<IEnumerable<PersonDto>> GetAllPersons();
        Task Move(List<Person> persons);
    }
}
