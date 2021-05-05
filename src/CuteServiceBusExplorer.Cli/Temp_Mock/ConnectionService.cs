using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;

namespace CuteServiceBusExplorer.Cli.Temp_Mock
{
    public class ConnectionService : IConnectionService
    {
        private Dictionary<string, CuteServiceBusExplorer.Interface.Connection> connections = (new List<Connection>
        {
            new CuteServiceBusExplorer.Interface.Connection
            {
                Key = "mySbCon",
                Name = "Development Test",
                Uri = "https://mynamespace.servicebus.azure.net",
                Namespace = "myservicebus"
            },
            new CuteServiceBusExplorer.Interface.Connection
            {
                Key = "otherSbCon",
                Name = "Quality Assurance Test",
                Uri = "https://othernamespace.servicebus.azure.net",
                Namespace = "othernamespace"
            },
            new CuteServiceBusExplorer.Interface.Connection
            {
                Key = "pre-prod",
                Name = "Pre-Production Test",
                Uri = "https://mypreprodnamespace.servicebus.azure.net",
                Namespace = "mypreprodnamespace"
            },
            new CuteServiceBusExplorer.Interface.Connection
            {
                Key = "prod",
                Name = "Production",
                Uri = "https://myproduction.servicebus.azure.net",
                Namespace = "myproduction"
            }
        }).ToDictionary(v=> v.Key);
        public Task<GetConnectionsResponse> GetConnectionsAsync()
        {
            var result = new GetConnectionsResponse();
            result.Connections = connections.Values.ToArray();

            return Task.FromResult(result);
        }
        
        public Task<GetConnectionsResponse> GetConnectionsAsync(IEnumerable<string> keys)
        {
            var result = new GetConnectionsResponse();
            result.Connections = connections.Where(c => keys.Contains(c.Key)).Select(e => e.Value).ToArray();

            return Task.FromResult(result);
        }

        public Task<GetConnectionResponse> GetConnectionAsync(string key)
        {
            Connection conn = null;
            connections.TryGetValue(key, out conn);
            
            var result = new GetConnectionResponse();
            result.Connection = conn;

            return Task.FromResult(result);
        }

        public Task<bool> TryRemoveConnectionAsync(string key)
        {
            if (!connections.TryGetValue(key, out var conn))
                return Task.FromResult(false);

            var result = connections.Remove(key);
            
            return Task.FromResult(result);
        }

        public Task<IEnumerable<string>> PurgeConnectionsAsync()
        {
            IEnumerable<string> result = connections.Select(c => c.Key).ToArray();
            
            connections.Clear();

            return Task.FromResult(result);
        }

        public Task<bool> TryAddConnection(string key, string name, string connectionString)
        {
            //See links in: https://github.com/Azure/azure-service-bus-dotnet/issues/651
            var conn = new CuteServiceBusExplorer.Interface.Connection
            {
                Key = key,
                Name = name,
                Uri = "https://othernamespace.servicebus.azure.net",
                Namespace = "othernamespace"
            };
            
            var result = connections.TryAdd(conn.Key, conn);
            
            return Task.FromResult(result);
        }
    }
}