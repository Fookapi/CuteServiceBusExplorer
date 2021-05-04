using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;

namespace CuteServiceBusExplorer.Cli.Temp_Mock
{
    public class ConnectionService : IConnectionService
    {
        private List<CuteServiceBusExplorer.Interface.Connection> connections = new List<Connection>
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
        };
        public Task<GetConnectionsResponse> GetConnectionsAsync()
        {
            var result = new GetConnectionsResponse();
            result.Connections = connections.ToArray();

            return Task.FromResult(result);
        }
        
        public Task<GetConnectionsResponse> GetConnectionsAsync(IEnumerable<string> keys)
        {
            var result = new GetConnectionsResponse();
            result.Connections = connections.Where(c => keys.Contains(c.Key)).ToArray();

            return Task.FromResult(result);
        }

        public Task<GetConnectionResponse> GetConnectionAsync(string key)
        {
            var conn = connections.SingleOrDefault(c => c.Key == key);
            var result = new GetConnectionResponse();
            result.Connection = conn;

            return Task.FromResult(result);
        }

        public Task<bool> RemoveConnectionAsync(string key)
        {
            var conn = connections.SingleOrDefault(c => c.Key == key);

            if (conn == null)
                return Task.FromResult(false);

            var result = connections.Remove(conn);
            
            return Task.FromResult(result);
        }
    }
}