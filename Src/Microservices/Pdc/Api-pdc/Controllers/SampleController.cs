using Api_pdc_Entities;
using Api_pdc_Interfaces.MongoDbRepository;
using Microsoft.AspNetCore.Mvc;

namespace Api_pdc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly IMongoRepository<Person> _peopleRepository;

        public SampleController(IMongoRepository<Person> peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        [HttpPost("registerPerson")]
        public async Task AddPerson(string firstName, string lastName)
        {
            var person = new Person()
            {
                FirstName = "John",
                LastName = "Doe"
            };

            await _peopleRepository.InsertOneAsync(person);
        }

        [HttpGet("getPeopleData")]
        public IEnumerable<string> GetPeopleData()
        {
            var people = _peopleRepository.FilterBy(
                filter => filter.FirstName != "test",
                projection => projection.FirstName
            );
            return people;
        }
    }
}
