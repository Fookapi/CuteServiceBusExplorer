namespace CuteServiceBusExplorer.Domain
{
    public class Connection
    {
        public string Key { get; }
        public string Name { get; }
        public string Uri { get;  }
        public string Namespace { get; }

        public Connection(string key, string name, string uri, string @namespace)
        {
            Key       = key;
            Name      = name;
            Uri       = uri;
            Namespace = @namespace;
        }
    }
}