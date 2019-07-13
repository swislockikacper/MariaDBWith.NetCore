using System.Collections.Generic;
using System.Threading.Tasks;
using MariaDBWith.NetCore.Models;
using MariaDBWith.NetCore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MariaDBWith.NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository clientRepository;

        public ClientsController(IClientRepository clientRepository) => this.clientRepository = clientRepository;

        [HttpGet]
        public async Task<IEnumerable<Client>> Get() => await clientRepository.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<Client> Get(string id) => await clientRepository.GetByIdAsync(id);

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Client client)
        {
            await clientRepository.InsertAsync(client);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Client client)
        {
            await clientRepository.UpdateAsync(client);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await clientRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
