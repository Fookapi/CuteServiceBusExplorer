using System;
using System.Collections.Generic;
using System.Linq;

namespace CuteServiceBusExplorer.Domain
{
    public class Topic
    {
        private readonly ICollection<Subscription> _subscriptions;
        
        public string Name { get; }
        public TimeSpan? AutoDeleteOnIdle { get; }
        public long MaxSizeInMegabytes { get; }
        public TimeSpan MessageTimeToLive { get; }
        
        public IEnumerable<Subscription> Subscriptions => _subscriptions.ToList();

        public Topic(string name, TimeSpan? autoDeleteOnIdle, long maxSizeInMegabytes, TimeSpan messageTimeToLive)
        {
            Name               = name;
            AutoDeleteOnIdle   = autoDeleteOnIdle;
            MaxSizeInMegabytes = maxSizeInMegabytes;
            MessageTimeToLive  = messageTimeToLive;

            _subscriptions = new List<Subscription>();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Topic) obj);
        }

        public void AddSubscription(Subscription subscription)
        {
            _subscriptions.Add(subscription);
        }
        
        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
        
        private bool Equals(Topic other)
        {
            return Name == other.Name;
        }
    }
}