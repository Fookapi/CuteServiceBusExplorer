namespace CuteServiceBusExplorer.Domain
{
    public class Subscription
    {
        public string Name { get; }

        public Subscription(string name)
        {
            Name = name;
        }
        
        protected bool Equals(Subscription other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Subscription) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}