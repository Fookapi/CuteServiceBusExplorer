using System.Collections.Generic;
using System.Threading.Tasks;

namespace CuteServiceBusExplorer.Domain
{
    public interface IConnectionStore
    {
        Task<Connection[]> GetConnectionsAsync();
        Task<Connection[]> GetConnectionsAsync(IEnumerable<string> keys);
        Task<Connection> GetConnectionAsync(string key);
        Task<bool> RemoveAsync(string key);
    }
}