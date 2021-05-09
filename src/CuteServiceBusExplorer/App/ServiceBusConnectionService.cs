using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Domain;
using CuteServiceBusExplorer.Interface;
using Connection = CuteServiceBusExplorer.Interface.Connection;

namespace CuteServiceBusExplorer.App
{
    public class ServiceBusConnectionService : IConnectionService
    {
        private readonly IConnectionStore _connectionStore;

        public ServiceBusConnectionService(IConnectionStore connectionStore)
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
                    Namespace = string.Empty,
                    Uri       = connection.ConnectionString
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
                    Namespace = string.Empty,
                    Uri       = connection.ConnectionString
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
                    Namespace = string.Empty,
                    Uri       = connection.ConnectionString
                }
            };

            return response;
        }

        public async Task<bool> TryRemoveConnectionAsync(string key)
        {
            try
            {
                bool result = await _connectionStore.RemoveAsync(key);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }
        }

        public async Task<IEnumerable<string>> PurgeConnectionsAsync()
        {
            var keys = await _connectionStore.Purge();

            return keys;
        }

        public async Task<bool> TryAddConnection(string key, string name, string connectionString)
        {
            try
            {
                await _connectionStore.AddConnection(key, name, connectionString);
                
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
                return false;
            }
        }
    }
}