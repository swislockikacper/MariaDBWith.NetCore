using MariaDBWith.NetCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MariaDBWith.NetCore.Repositories
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(string id);
        Task<List<Client>> GetAllAsync();
        Task InsertAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(string id);
    }
}
