using System.Threading.Tasks;

namespace CuteServiceBusExplorer.Interface
{
    public interface IConnectionService
    {
        public Task<GetConnectionsResponse> GetConnectionsAsync();
    }
}