using System.Threading.Tasks;

namespace CuteServiceBusExplorer.Interface
{
    public interface ITopicService
    {
        public Task<GetTopicsResponse> GetTopicsAsync(string connectionKey);
        public Task<GetSubscriptionsForTopicResponse> GetSubscriptionsForTopic(string topic, string connectionKey);
    }
}