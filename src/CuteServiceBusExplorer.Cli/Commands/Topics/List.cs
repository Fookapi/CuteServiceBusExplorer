using System.Threading.Tasks;
using CuteServiceBusExplorer.Infrastructure;
using CuteServiceBusExplorer.Interface;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using AnsiConsole = Spectre.Console.AnsiConsole;

namespace CuteServiceBusExplorer.Cli.Commands.Topics
{

    [Command(
        Name = "list",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    public class List : BaseConnectedCommand
    {
        private readonly ITopicService _topicService;

        [Option(CommandOptionType.NoValue, ShortName = "u", LongName = "unformatted",
            Description = "Don't show formatted output, only print topic names.", ShowInHelpText = true)]
        public bool Unformatted { get; set; } = false;

        public List(ITopicService topicService, ILogger<List> logger, IConsole console) : base(logger, console)
        {
            _topicService = topicService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var topicResponse = await _topicService.GetTopicsAsync(ConnectionKey);
            
            if (topicResponse?.Topics == null)
                return await ExitCodesResult.TaskForSuccess;

            if (Unformatted)
                RenderPlainTopics(topicResponse);
            else
                RenderFormattedTopics(topicResponse);

            return await ExitCodesResult.TaskForSuccess;
        }

        private void RenderPlainTopics(GetTopicsResponse topicsResponse)
        {
            foreach (var topic in topicsResponse.Topics)
            {
                AnsiConsole.WriteLine(topic.Name);
            }
        }

        private void RenderFormattedTopics(GetTopicsResponse topicsResponse)
        {
            AnsiConsole.MarkupLine($"Connection Key: [blue]{ConnectionKey}[/]");
            
            var table = new Table();

            var nameColumn = new TableColumn("Name");
            var currentSizeColumn = new TableColumn("Current Size");
            var maxSizeColumn = new TableColumn("Maximum Size");
            var freeSpaceColumn = new TableColumn("Free Space");
            var autoDeleteColumn = new TableColumn("Time To Auto Delete");
            var messageTtlColumn = new TableColumn("Message Time to Live");
            

            table.AddColumn(nameColumn);
            table.AddColumn(freeSpaceColumn);
            table.AddColumn(currentSizeColumn);
            table.AddColumn(maxSizeColumn);
            table.AddColumn(messageTtlColumn);
            table.AddColumn(autoDeleteColumn);

            table.Border = TableBorder.Minimal;

            foreach (var topic in topicsResponse.Topics)
            {
                
                var nameValue = new Markup($"[blue]{topic.Name}[/]");
                var currentSizeValue = new Markup($"[grey]{ByteHelper.BytesToFriendlyString(topic.CurrentSizeBytes)}[/]");
                var maxSizeValue = new Markup($"[grey]{ByteHelper.BytesToFriendlyString(topic.MaximumSizeBytes)}[/]");
                var freeSpaceValue = new Markup($"[default]{topic.FreeSpace:P2}[/]");
                var autoDeleteValue = new Markup($"[grey]{(topic.AutoDelete.HasValue ? topic.AutoDelete.Value.ToString("G") : "n/a")}[/]");
                var messageTtlValue = new Markup($"[grey]{topic.MessageTimeToLive:G}[/]");
                
                table.AddRow(nameValue, freeSpaceValue, currentSizeValue, maxSizeValue, messageTtlValue, autoDeleteValue);
            }

            AnsiConsole.Render(table);
        }

    }
}