using System.Collections.Generic;
using System.Threading.Tasks;

namespace CuteServiceBusExplorer.Interface
{
    public interface IConnectionService
    {
        public Task<GetConnectionsResponse> GetConnectionsAsync();
        public Task<GetConnectionsResponse> GetConnectionsAsync(IEnumerable<string> keys);
        public Task<GetConnectionResponse> GetConnectionAsync(string key);
        public Task<bool> TryRemoveConnectionAsync(string key);
        public Task<IEnumerable<string>> PurgeConnectionsAsync();
        public Task<bool> TryAddConnection(string key, string name, string connectionString);
    }
}