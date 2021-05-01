using System.Collections.Generic;
using System.Linq;

namespace CuteServiceBusExplorer.Domain
{
    public class Topic
    {
        private readonly ICollection<Subscription> _subscriptions;
        public string Name { get; }
        
        public IEnumerable<Subscription> Subscriptions => _subscriptions.ToList();

        public Topic(string name)
        {
            Name = name;

            _subscriptions = new List<Subscription>();
        }
        
        protected bool Equals(Topic other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Topic) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public void AddSubscription(Subscription subscription)
        {
            _subscriptions.Add(subscription);
        }
    }
}