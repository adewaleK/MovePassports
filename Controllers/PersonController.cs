using Microsoft.AspNetCore.Mvc;
using PassMoveAPI.Data.Entities;
using PassMoveAPI.Services;

namespace PassMoveAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly string connectionString;

        public PersonController(IPersonService personService, IPhotoAcessor photoAcessor, IConfiguration configuration)
        {
            _personService = personService;
            connectionString = configuration["AzureBlobStorageConnectionString"];
        }

        [HttpPost("create-person")]
        public async Task<IActionResult> CreatePerson([FromForm] FileModel file)
        {
            var data = await _personService.AddPersonAsync(file);
            return Ok(data);
        }

        [HttpPost("move-passports")]
        public async Task<IActionResult> MovePassports()
        {
            var persons = await _personService.GetAllPersons();
            var allPersons = persons.Select(x => new Person { Name = x.Name, Passport = x.Passport }).ToList();
            await _personService.Move(allPersons);
            return Ok("images moved successfully");
        }

    }
}