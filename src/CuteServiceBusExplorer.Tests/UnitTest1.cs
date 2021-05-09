using System;
using System.Threading.Tasks;
using CuteServiceBusExplorer.App;
using CuteServiceBusExplorer.Domain;
using CuteServiceBusExplorer.Infra;
using CuteServiceBusExplorer.Interface;
using NUnit.Framework;

namespace CuteServiceBusExplorer.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            //var serviceBusConnectionString = Environment.GetEnvironmentVariable("SBC");

            IConnectionService connectionService = new ServiceBusExplorerService(new ConnectionStore());
            var                test              = await connectionService.GetConnectionsAsync();
            var                conn              = test.Connections;
            // using (ServiceBusExplorer serviceBusExplorer = await ServiceBusExplorer.CreateAsync(serviceBusConnectionString))
            // {
            //     var topics = serviceBusExplorer.GetTopics();
            //     foreach (var topic in topics)
            //     {
            //         Console.WriteLine($"Topic: {topic.Name}");
            //         foreach (var topicSubscription in topic.Subscriptions)
            //         {
            //             Console.WriteLine($"Subscription: {topicSubscription.Name}");
            //         }
            //     }
            // }

            Assert.Pass();
        }
    }
}