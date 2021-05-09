using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Domain;
using CuteServiceBusExplorer.Interface;
using Connection = CuteServiceBusExplorer.Interface.Connection;

namespace CuteServiceBusExplorer.App
{
    public class ServiceBusExplorerService:IConnectionService
    {
        private readonly IConnectionStore _connectionStore;

        public ServiceBusExplorerService(IConnectionStore connectionStore)
        {
            _connectionStore = connectionStore;
        }

        public async Task<GetConnectionsResponse> GetConnectionsAsync()
        {
            var connections = await _connectionStore.GetConnectionsAsync();

            var response = new GetConnectionsResponse
            {
                Connections = connections.Select(connection => new Connection
                {
                    Key       = connection.Key,
                    Name      = connection.Name,
                    Namespace = connection.Namespace,
                    Uri       = connection.Uri
                }).ToArray()
            };
            
            return response;
        }

        public async Task<GetConnectionsResponse> GetConnectionsAsync(IEnumerable<string> keys)
        {
            var connections = await _connectionStore.GetConnectionsAsync(keys);

            var response = new GetConnectionsResponse
            {
                Connections = connections.Select(connection => new Connection
                {
                    Key       = connection.Key,
                    Name      = connection.Name,
                    Namespace = connection.Namespace,
                    Uri       = connection.Uri
                }).ToArray()
            };
            
            return response;
        }

        public async Task<GetConnectionResponse> GetConnectionAsync(string key)
        {
            var connection = await _connectionStore.GetConnectionAsync(key);
            var response = new GetConnectionResponse
            {
                Connection = new Connection
                {
                    Key       = connection.Key,
                    Name      = connection.Name,
                    Namespace = connection.Namespace,
                    Uri       = connection.Uri
                }
            };
            
            return response;
        }

        public async Task<bool> RemoveConnectionAsync(string key)
        {
            bool result = await _connectionStore.RemoveAsync(key);

            return result;
        }
    }
}