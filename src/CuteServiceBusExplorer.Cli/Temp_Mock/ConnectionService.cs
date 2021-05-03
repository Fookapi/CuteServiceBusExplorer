using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;

namespace CuteServiceBusExplorer.Cli.Temp_Mock
{
    public class ConnectionService : IConnectionService
    {
        public Task<GetConnectionsResponse> GetConnectionsAsync()
        {
            var connections = new[]
            {
                new GetConnectionsResponse.Connection
                {
                    Key = "mySbCon",
                    Name = "Development Test",
                    Uri = "https://mynamespace.servicebus.azure.net",
                    Namespace = "myservicebus"
                },
                new GetConnectionsResponse.Connection
                {
                    Key = "otherSbCon",
                    Name = "Quality Assurance Test",
                    Uri = "https://othernamespace.servicebus.azure.net",
                    Namespace = "othernamespace"
                },
                new GetConnectionsResponse.Connection
                {
                    Key = "pre-prod",
                    Name = "Pre-Production Test",
                    Uri = "https://mypreprodnamespace.servicebus.azure.net",
                    Namespace = "mypreprodnamespace"
                },
                new GetConnectionsResponse.Connection
                {
                    Key = "prod",
                    Name = "Production",
                    Uri = "https://myproduction.servicebus.azure.net",
                    Namespace = "myproduction"
                }
            };

            var result = new GetConnectionsResponse();
            result.Connections = connections;

            return Task.FromResult(result);
        }
    }
}