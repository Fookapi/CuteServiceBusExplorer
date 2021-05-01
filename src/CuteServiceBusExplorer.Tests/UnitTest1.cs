using System;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Domain;
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
            var serviceBusConnectionString = Environment.GetEnvironmentVariable("SBC");

            using (ServiceBus serviceBus = await ServiceBus.CreateAsync(serviceBusConnectionString))
            {
                var topics = serviceBus.GetTopics();
                foreach (var topic in topics)
                {
                    Console.WriteLine($"Topic: {topic.Name}");
                    foreach (var topicSubscription in topic.Subscriptions)
                    {
                        Console.WriteLine($"Subscription: {topicSubscription.Name}");
                    }
                }
            }

            Assert.Pass();
        }
    }
}