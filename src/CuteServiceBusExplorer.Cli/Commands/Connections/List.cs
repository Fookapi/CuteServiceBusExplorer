using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace CuteServiceBusExplorer.Cli.Commands.Connections
{

    [Command(
        Name = "list",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    public class List : BaseCommand
    {
        private readonly IConnectionService _connectionService;
        
        [Option(CommandOptionType.NoValue, ShortName = "u", LongName = "unformatted", Description = "Don't show formatted output, only print keys", ShowInHelpText = true)]
        public bool Unformatted { get; set; } = false;
        public List(IConnectionService connectionService, ILogger<List> logger, IConsole console) : base(logger, console)
        {
            _connectionService = connectionService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var connectionsResponse = await _connectionService.GetConnectionsAsync();

            if (Unformatted)
                RenderPlainConnections(connectionsResponse);
            else
                RenderFormattedConnections(connectionsResponse);

            return await Task.FromResult((int) ExitCodes.Success);
        }

        private void RenderPlainConnections(GetConnectionsResponse connectionsResponse)
        {
            foreach (var connection in connectionsResponse.Connections)
            {
                AnsiConsole.WriteLine(connection.Key);
            }
        }

        private void RenderFormattedConnections(GetConnectionsResponse connectionsResponse)
        {
            var table = new Table();

            var keyColumn = new TableColumn("Key");
            var nameColumn = new TableColumn("Name");
            var namespaceColumn = new TableColumn("Namespace");
            var uriColumn = new TableColumn("URI");

            uriColumn.NoWrap();
            
            table.AddColumn(keyColumn);
            table.AddColumn(nameColumn);
            table.AddColumn(namespaceColumn);
            table.AddColumn(uriColumn);
            
            table.Border = TableBorder.Minimal;
            
            foreach (var connection in connectionsResponse.Connections)
            {
                var keyValue = new Markup($"[blue]{connection.Key}[/]");
                var nameValue = new Markup($"[grey]{connection.Name}[/]");
                var namespaceValue = new Markup($"[grey]{connection.Namespace}[/]");
                var uriValue = new Markup($"[grey]{connection.Uri}[/]");
                table.AddRow(keyValue, nameValue, namespaceValue, uriValue);
            }
            
            AnsiConsole.Render(table);
        }
    }
}