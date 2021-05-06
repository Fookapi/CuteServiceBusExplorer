using System.Threading.Tasks;
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
            var topicResponse = await _topicService.GetTopics(ConnectionKey);
            
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
            //AnsiConsole.WriteLine();
            
            var table = new Table();

            var nameColumn = new TableColumn("Name");
            //var namespaceColumn = new TableColumn("Namespace");
            //var uriColumn = new TableColumn("URI");

            table.AddColumn(nameColumn);
            //table.AddColumn(namespaceColumn);
            //table.AddColumn(uriColumn);

            table.Border = TableBorder.Minimal;

            foreach (var topic in topicsResponse.Topics)
            {
                
                var nameValue = new Markup($"[blue]{topic.Name}[/]");
                //var namespaceValue = new Markup($"[grey]{topic.Namespace}[/]");
                //var uriValue = new Markup($"[grey]{topic.Uri}[/]");
                table.AddRow(nameValue);
            }

            AnsiConsole.Render(table);
        }

    }
}