namespace CuteServiceBusExplorer.Interface
{
    public class GetConnectionsResponse
    {
        public Connection[] Connections { get; set; }

        public class Connection
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public string Uri { get; set; }
            public string Namespace { get; set; }
        }
    }
}