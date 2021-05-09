namespace CuteServiceBusExplorer.Domain
{
    public class Connection
    {
        public string Key { get; }
        public string Name { get; }
        public string ConnectionString { get; }

        public Connection(string key, string name, string connectionString)
        {
            Key              = key;
            Name             = name;
            ConnectionString = connectionString;
        }
    }
}