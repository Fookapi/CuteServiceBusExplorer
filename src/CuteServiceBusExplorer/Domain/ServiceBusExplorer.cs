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
        private readonly ICollection<Topic> _topics;

        private ServiceBusExplorer(ServiceBusClient client, ServiceBusAdministrationClient adminClient)
        {
            _client      = client;
            _adminClient = adminClient;
            _topics      = new List<Topic>();
        }

        // public static async Task<ServiceBusExplorer> CreateAsync(string connectionString)
        // {
        //     // Service Bus client
        //     var client = new ServiceBusClient(connectionString);
        //     // Service Bus administration client
        //     var adminClient = new ServiceBusAdministrationClient(connectionString);
        //
        //     ServiceBusExplorer serviceBusExplorer = new ServiceBusExplorer(client, adminClient);
        //
        //     await Init(serviceBusExplorer);
        //
        //     return serviceBusExplorer;
        // }
        //
        // public async Task Refresh()
        // {
        //     await Init(this);
        // }

        public IEnumerable<Topic> GetTopics()
        {
            return _topics.ToList();
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

        private static async Task Init(ServiceBusExplorer serviceBusExplorer)
        {
            AsyncPageable<TopicProperties> topicsProperties = serviceBusExplorer._adminClient.GetTopicsAsync();

            //Load Topics
            await foreach (TopicProperties topic in topicsProperties)
            {
                serviceBusExplorer._topics.Add(new Topic(topic.Name));
            }

            foreach (Topic topic in serviceBusExplorer._topics)
            {
                var subscriptionProperties = serviceBusExplorer._adminClient.GetSubscriptionsAsync(topic.Name);
                await foreach (SubscriptionProperties subscription in subscriptionProperties)
                {
                    topic.AddSubscription(new Subscription(subscription.SubscriptionName));
                }
            }
        }
    }
}