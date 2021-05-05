using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace CuteServiceBusExplorer.Cli.Commands.Connections
{
    [Command(
        Name = "purge",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    public class Purge : BaseCommand
    {
        private readonly IConnectionService _connectionService;
        
        [Option(CommandOptionType.NoValue, ShortName = "f", LongName = "force", Description = "Force. Don't prompt for confirmation.", ShowInHelpText = true)]
        public bool Force { get; set; } = false;
        
        public Purge(IConnectionService connectionService, ILogger<Purge> logger, IConsole console) : base(logger, console)
        {
            _connectionService = connectionService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var connections = await _connectionService.GetConnectionsAsync();

            if (!connections.Connections.Any())
            {
                AnsiConsole.MarkupLine("[darkorange]No connections found to purge.[/]");
                return await Task.FromResult((int) ExitCodes.Success);
            }
            
            bool confirmed;
            if (Force)
                confirmed = true;
            else
                confirmed = AnsiConsole.Confirm($"[default]Are you sure you want to remove [bold]{connections.Connections.Length}[/] connection{(connections.Connections.Length > 1 ? "s" : "")}?[/]", 
                    defaultValue: false);

            if (!confirmed)
            {
                AnsiConsole.MarkupLine("[default]Nothing removed.[/]");
                return await Task.FromResult((int) ExitCodes.Success);
            }

            var keysRemoved = await _connectionService.PurgeConnectionsAsync();

            foreach (var key in keysRemoved)
            {
                AnsiConsole.MarkupLine($"[grey]{key}[/]");
            }
            
            return await Task.FromResult((int) ExitCodes.Success);
        }
    }
}