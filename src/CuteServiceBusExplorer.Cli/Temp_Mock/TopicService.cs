using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;

namespace CuteServiceBusExplorer.Cli.Temp_Mock
{
    public class TopicService : ITopicService
    {
        private readonly IConnectionService _connectionService;

        private Dictionary<string, Topic[]> _topicsPerConnection = new Dictionary<string, Topic[]>
        {
            ["mySbCon"] = new List<Topic>
            {
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicOne",
                    CurrentSizeBytes = 250 + (1024 * 10),
                    MaximumSizeBytes = 3 * 1024 * 1024,
                    AutoDelete = null,
                    MessageTimeToLive = TimeSpan.FromMinutes(4)
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicTwo",
                    CurrentSizeBytes = 333 + (1024 * 5),
                    MaximumSizeBytes = 1 * 1024 * 1024,
                    AutoDelete = TimeSpan.FromDays(1),
                    MessageTimeToLive = TimeSpan.FromSeconds(213)
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicThree",
                    CurrentSizeBytes = 54 + (1024 * 56),
                    MaximumSizeBytes = 60 * 1024 * 1024,
                    AutoDelete = TimeSpan.FromHours(3),
                    MessageTimeToLive = TimeSpan.FromSeconds(32)
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicFour",
                    CurrentSizeBytes = 34 + (1024 * 67),
                    MaximumSizeBytes = 455 * 1024 * 1024,
                    AutoDelete = null,
                    MessageTimeToLive = TimeSpan.FromSeconds(234)
                }
            }.ToArray(),
            ["otherSbCon"] = new List<Topic>
            {
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicAlpha",
                    CurrentSizeBytes = 0 + (1024 * 1024 * 87),
                    MaximumSizeBytes = 879L * 1024L * 1024L * 1024L,
                    AutoDelete = TimeSpan.FromMilliseconds(333),
                    MessageTimeToLive = TimeSpan.FromMinutes(334)
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicBravo",
                    CurrentSizeBytes = 23L + (1024L * 1024L * 3324L),
                    MaximumSizeBytes = 1 * 1024 * 1024 * 1024,
                    AutoDelete = TimeSpan.MaxValue,
                    MessageTimeToLive = TimeSpan.FromHours(22)
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicCharlie",
                    CurrentSizeBytes = 43 + (1024 * 1024 * 54),
                    MaximumSizeBytes = 55L * 1024L * 1024L * 1024L,
                    AutoDelete = null,
                    MessageTimeToLive = TimeSpan.FromMilliseconds(9878)
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicDelta",
                    CurrentSizeBytes = 876 + (1024 * 1024 * 22),
                    MaximumSizeBytes = 876L * 1024L * 1024L * 1024L,
                    AutoDelete = TimeSpan.FromTicks(432987432),
                    MessageTimeToLive = TimeSpan.FromSeconds(3243)
                }
            }.ToArray(),
            ["prod"] = new List<Topic>
            {
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicPhi",
                    CurrentSizeBytes = 43432 + (1024 * 54),
                    MaximumSizeBytes = 1L * 1024L * 1024L * 1024L * 1024L,
                    AutoDelete = TimeSpan.FromHours(33),
                    MessageTimeToLive = TimeSpan.FromSeconds(32434)
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicPi",
                    CurrentSizeBytes = 0,
                    MaximumSizeBytes = 1,
                    AutoDelete = TimeSpan.FromSeconds(4355),
                    MessageTimeToLive = TimeSpan.FromMilliseconds(1121)
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicSigma",
                    CurrentSizeBytes = 3234,
                    MaximumSizeBytes = 5096,
                    AutoDelete = null,
                    MessageTimeToLive = TimeSpan.FromTicks(3298732987)
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicRu",
                    CurrentSizeBytes = 128,
                    MaximumSizeBytes = 256,
                    AutoDelete = TimeSpan.FromMinutes(5),
                    MessageTimeToLive = TimeSpan.FromMinutes(10)
                }
            }.ToArray()
        };

        private Dictionary<string, Subscription[]> _subscriptionsPerTopic = new Dictionary<string, Subscription[]>
        {
            ["TopicOne"] = new List<Subscription>
            {
                new CuteServiceBusExplorer.Interface.Subscription
                {
                    Name = "Audit",
                    ActiveMessageCount = 10,
                    AutoDeleteAfterIdleFor = TimeSpan.FromSeconds(900),
                    CreatedAt = new DateTimeOffset(2020, 01, 23, 23, 34,54, TimeSpan.FromHours(2)),
                    DeadLetteringEnabled = true,
                    DeadLetterMessageCount = 5,
                    ForwardMessagesTo = "/dev/null",
                    MaximumDeliveryCount = 1000,
                    MessageLockDuration = TimeSpan.FromSeconds(200),
                    MessageTimeToLive = TimeSpan.FromSeconds(500), 
                    Status = "Active", 
                    UpdatedAt = DateTimeOffset.Now.AddSeconds(-273), 
                    ScheduledMessageCount = 120, 
                    TransferMessageCount = 10000
                },
                new CuteServiceBusExplorer.Interface.Subscription
                {
                    Name = "MarketDayEndSubscription",
                    ActiveMessageCount = 1,
                    AutoDeleteAfterIdleFor = TimeSpan.FromSeconds(890),
                    CreatedAt =  new DateTimeOffset(2000, 01, 23, 23, 34,54, TimeSpan.FromHours(2)),
                    DeadLetteringEnabled = true,
                    DeadLetterMessageCount = 10,
                    ForwardMessagesTo = "n/a",
                    MaximumDeliveryCount = 100000,
                    MessageLockDuration = TimeSpan.FromDays(12),
                    MessageTimeToLive = TimeSpan.FromSeconds(49874), 
                    Status = "Pending", 
                    UpdatedAt = DateTimeOffset.Now.AddSeconds(-43442), 
                    ScheduledMessageCount = 1043870, 
                    TransferMessageCount = 12340
                },
                new CuteServiceBusExplorer.Interface.Subscription
                {
                    Name = "MessageSenderSubscription",
                    ActiveMessageCount = 4387,
                    AutoDeleteAfterIdleFor = TimeSpan.FromSeconds(0),
                    CreatedAt = DateTimeOffset.Now,
                    DeadLetteringEnabled = false,
                    DeadLetterMessageCount = 0,
                    ForwardMessagesTo = "admin",
                    MaximumDeliveryCount = 39845440,
                    MessageLockDuration = TimeSpan.FromSeconds(987092098),
                    MessageTimeToLive = TimeSpan.FromSeconds(9876556), 
                    Status = "Overflow", 
                    UpdatedAt = DateTimeOffset.Now, 
                    ScheduledMessageCount = 484587, 
                    TransferMessageCount = 0
                },
                new CuteServiceBusExplorer.Interface.Subscription
                {
                    Name = "Billing",
                    ActiveMessageCount = 0,
                    AutoDeleteAfterIdleFor = TimeSpan.FromSeconds(0),
                    CreatedAt = DateTimeOffset.Now,
                    DeadLetteringEnabled = false,
                    DeadLetterMessageCount = 0,
                    ForwardMessagesTo = "n/a",
                    MaximumDeliveryCount = 0,
                    MessageLockDuration = TimeSpan.FromSeconds(180),
                    MessageTimeToLive = TimeSpan.FromSeconds(600), 
                    Status = "Active", 
                    UpdatedAt = DateTimeOffset.Now, 
                    ScheduledMessageCount = 0, 
                    TransferMessageCount = 0
                },
            }.ToArray()
        };
    

    public TopicService(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
        
        public Task<GetTopicsResponse> GetTopicsAsync(string connectionKey)
        {
            Topic[] topics = null;
            _topicsPerConnection.TryGetValue(connectionKey, out topics);

            var result = new GetTopicsResponse();
            result.Topics = topics;

            return Task.FromResult(result);
        }

        public Task<GetSubscriptionsForTopicResponse> GetSubscriptionsForTopic(string topic, string connectionKey)
        {
            Topic[] topics = null;
            _topicsPerConnection.TryGetValue(connectionKey, out topics);

            var result = new GetSubscriptionsForTopicResponse();
            result.Subscriptions = null;

            if (topics == null)
                return Task.FromResult(result);
                
            
            Subscription[] subs = null;
            _subscriptionsPerTopic.TryGetValue(topic, out subs);

            result.Subscriptions = subs;

            return Task.FromResult(result);
            
        }
    }
}