using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Domain;

namespace CuteServiceBusExplorer.Infra
{
    public class ConnectionStore:IConnectionStore
    {
        private readonly string connectionsPath;

        public ConnectionStore()
        {
            string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            baseFolder += "/.csbx";
            
            bool folderExists = Directory.Exists(baseFolder);
            if (!folderExists)
            {
                Directory.CreateDirectory(baseFolder);
            }
            
            connectionsPath = $"{baseFolder}/connections.json";
            
            bool connectionFileExists = File.Exists(connectionsPath);

            if (!connectionFileExists)
            {
                SaveConnections(new List<Connection>()).GetAwaiter().GetResult();
            }
        }

        public async Task<Connection[]> GetConnectionsAsync()
        {
            var connections = await GetConnections();

            return connections?.ToArray();
        }
        
        public async Task<Connection[]> GetConnectionsAsync(IEnumerable<string> keys)
        {
            var connections = await GetConnections();

            var result = connections.Where(c => keys.Contains(c.Key));

            return result.ToArray();
        }

        public async Task<Connection> GetConnectionAsync(string key)
        {
            ICollection<Connection> connections = await GetConnections();

            return connections.Single(c => c.Key == key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            ICollection<Connection> connections = await GetConnections();
            
            var connectionToRemove = connections.Single(c => c.Key == key);

            var success = connections.Remove(connectionToRemove);

            await SaveConnections(connections);
            
            return success;
        }

        public async Task<string[]> Purge()
        {
            var connections = await GetConnections();

            var keysPurged = connections?.Select(c => c.Key);

            await SaveConnections(new List<Connection>());

            return keysPurged.ToArray();
        }

        public async Task AddConnection(string key, string name, string uri)
        {
            var connections = await GetConnections();
            
            connections.Add(new Connection(key, name, uri));

            await SaveConnections(connections);
        }

        private async Task<ICollection<Connection>> GetConnections()
        {
            await using var openStream = File.OpenRead(connectionsPath);

            var connections = await JsonSerializer.DeserializeAsync<ICollection<Connection>>(openStream);

            return connections;
        }

        private async Task SaveConnections(ICollection<Connection>connections  )
        {
            await using var createStream = File.Create(connectionsPath);
            await JsonSerializer.SerializeAsync(createStream, connections);
        }
    }
}