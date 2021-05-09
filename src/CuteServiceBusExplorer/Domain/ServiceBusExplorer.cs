using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace CuteServiceBusExplorer.Domain
{
    public class ServiceBusExplorer : IDisposable
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusAdministrationClient _adminClient;
        
        private ICollection<Topic> _topics;

        public ServiceBusExplorer(ServiceBusClient client, ServiceBusAdministrationClient adminClient)
        {
            _client      = client;
            _adminClient = adminClient;

            _topics = new List<Topic>();
        }

        public async Task<Topic[]> GetTopicsAsync()
        {
            await LoadTopicsAsync();
            
            return _topics.ToArray();
        }

        public async Task SendMessageToTopicAsync(string topicName)
        {
            // create a sender for the topic
            ServiceBusSender sender = _client.CreateSender(topicName);
            await sender.SendMessageAsync(new ServiceBusMessage("Tests"));
        }

        public async void Dispose()
        {
            await _client.DisposeAsync();
        }

        private async Task LoadTopicsAsync()
        {
            AsyncPageable<TopicProperties> topicsProperties = _adminClient.GetTopicsAsync();
            
            //Load Topics
            await foreach (TopicProperties topic in topicsProperties)
            {
                if (_topics.All(t => t.Name != topic.Name))
                    _topics.Add(new Topic(topic.Name, topic.AutoDeleteOnIdle, topic.MaxSizeInMegabytes,
                        topic.DefaultMessageTimeToLive));
            }
        }
    }
}