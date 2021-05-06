using System.Threading.Tasks;

namespace CuteServiceBusExplorer.Interface
{
    public interface ITopicService
    {
        public Task<GetTopicsResponse> GetTopics(string connectionKey);
    }
}