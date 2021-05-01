using System.Threading.Tasks;
using CuteServiceBusExplorer.Domain;

namespace CuteServiceBusExplorer.App
{
    public class ServiceBusExplorer
    {
        public async Task Connect(string connectionString)
        {
            ServiceBus serviceBus = await ServiceBus.CreateAsync(connectionString);
            
        }
    }
}