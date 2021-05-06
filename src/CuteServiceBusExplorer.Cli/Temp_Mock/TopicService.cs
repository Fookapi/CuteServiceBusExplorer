using System.Collections.Generic;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;

namespace CuteServiceBusExplorer.Cli.Temp_Mock
{
    public class TopicService : ITopicService
    {
        private readonly IConnectionService _connectionService;

        private Dictionary<string, Topic[]> _topicsPerConnection = new Dictionary<string, Topic[]>
        {
            ["mySbCon"] = new List<Topic>
            {
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicOne"
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicTwo"
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicThree"
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicFour"
                }
            }.ToArray(),
            ["otherSbCon"] = new List<Topic>
            {
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicAlpha"
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicBravo"
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicCharlie"
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicDelta"
                }
            }.ToArray(),
            ["prod"] = new List<Topic>
            {
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicPhi"
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicPi"
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicSigma"
                },
                new CuteServiceBusExplorer.Interface.Topic
                {
                    Name = "TopicRu"
                }
            }.ToArray()
        };
        
        public TopicService(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
        
        public Task<GetTopicsResponse> GetTopics(string connectionKey)
        {
            Topic[] topics = null;
            _topicsPerConnection.TryGetValue(connectionKey, out topics);

            var result = new GetTopicsResponse();
            result.Topics = topics;

            return Task.FromResult(result);
        }
    }
}