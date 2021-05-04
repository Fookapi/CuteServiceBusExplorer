using System.Collections.Generic;
using System.Threading.Tasks;

namespace CuteServiceBusExplorer.Interface
{
    public interface IConnectionService
    {
        public Task<GetConnectionsResponse> GetConnectionsAsync();
        public Task<GetConnectionsResponse> GetConnectionsAsync(IEnumerable<string> keys);
        public Task<GetConnectionResponse> GetConnectionAsync(string key);
        public Task<bool> RemoveConnectionAsync(string key);
    }
}