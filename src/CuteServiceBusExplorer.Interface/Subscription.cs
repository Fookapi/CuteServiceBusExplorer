using System;

namespace CuteServiceBusExplorer.Interface
{
    public class Subscription
    {
        public string Name { get; set; }
        public int MaximumDeliveryCount { get; set; }
        public TimeSpan MessageTimeToLive { get; set; }
        public TimeSpan MessageLockDuration { get; set; }
        public TimeSpan AutoDeleteAfterIdleFor { get; set; }
        public int ActiveMessageCount { get; set; }
        public int DeadLetterMessageCount { get; set; }
        public int TransferMessageCount { get; set; }
        public int ScheduledMessageCount { get; set; }
        public string Status { get; set; }
        public string ForwardMessagesTo { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool DeadLetteringEnabled { get; set; }
    }
}