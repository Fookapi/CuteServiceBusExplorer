using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using CuteServiceBusExplorer.Domain;
using CuteServiceBusExplorer.Interface;
using Topic = CuteServiceBusExplorer.Domain.Topic;

namespace CuteServiceBusExplorer.App
{
    public class ServiceBusTopicService:ITopicService
    {
        private readonly IConnectionStore _connectionStore;

        public ServiceBusTopicService(IConnectionStore connectionStore)
        {
            _connectionStore = connectionStore;
        }

        public async Task<GetTopicsResponse> GetTopicsAsync(string connectionKey)
        {
            ServiceBusExplorer serviceBusExplorer = await CreateServiceBusExplorer(connectionKey);
            Topic[]            topics             = await serviceBusExplorer.GetTopicsAsync();

            var response = new GetTopicsResponse();
            response.Topics = topics.Select(x => new Interface.Topic
            {
                Name              = x.Name,
                AutoDelete        = x.AutoDeleteOnIdle,
                CurrentSizeBytes  = default,
                MaximumSizeBytes  = x.MaxSizeInMegabytes,
                MessageTimeToLive = x.MessageTimeToLive
            }).ToArray();

            return response;
        }

        private async Task<ServiceBusExplorer> CreateServiceBusExplorer(string connectionKey)
        {
            var connection = await _connectionStore.GetConnectionAsync(connectionKey);
            
            // Service Bus client
            var client = new ServiceBusClient(connection.ConnectionString);
            
            // Service Bus administration client
            var adminClient = new ServiceBusAdministrationClient(connection.ConnectionString);
            
            
            ServiceBusExplorer serviceBusExplorer = new ServiceBusExplorer(client, adminClient);

            return serviceBusExplorer;
        }
    }
}