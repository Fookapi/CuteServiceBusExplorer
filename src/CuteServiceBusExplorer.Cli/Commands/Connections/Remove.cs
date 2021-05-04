using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace CuteServiceBusExplorer.Cli.Commands.Connections
{
    [Command(
        Name = "remove",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    public class Remove : BaseCommand
    {
        private readonly IConnectionService _connectionService;
        
        [Required]
        [Argument(order: 0, Description = "Keys of the connection to remove from the connection store.", Name = "keys", ShowInHelpText = true)]
        public string[] ConnectionKeys { get; set; }
        
        [Option(CommandOptionType.NoValue, ShortName = "f", LongName = "force", Description = "Force. Don't prompt for confirmation.", ShowInHelpText = true)]
        public bool Force { get; set; } = false;
        public Remove(IConnectionService connectionService, ILogger<Remove> logger, IConsole console) : base(logger, console)
        {
            _connectionService = connectionService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var args = ConnectionKeys.Where(k => !string.IsNullOrWhiteSpace(k)).Distinct().ToArray();
            if (!args.Any())
            {
                AnsiConsole.MarkupLine("[darkorange]Please provide at least one connection key as an argument.[/]");
                return await Task.FromResult((int) ExitCodes.CommandLineUsageError);
            }

            var connections = await _connectionService.GetConnectionsAsync(args);
            
            if (!connections.Connections.Any())
            {
                AnsiConsole.MarkupLine("[darkorange]No connections found matching the provided keys.[/]");
                return await Task.FromResult((int) ExitCodes.Success);
            }
            else if (connections.Connections.Length != args.Length)
            {
                AnsiConsole.MarkupLine($"[darkorange]{args.Length} keys provided but only {connections.Connections.Length} matching connection{(connections.Connections.Length > 1 ? "s were" : " was")} found.[/]");
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
            
            foreach (var con in connections.Connections)
            {
                if(await _connectionService.RemoveConnectionAsync(con.Key))
                    AnsiConsole.MarkupLine($"[grey]{con.Key}[/]");
                else
                {
                    AnsiConsole.MarkupLine($"[bold red]Error: Could not remove key '{con.Key}'[/]");
                    return await Task.FromResult((int) ExitCodes.GeneralError);
                }
            }

            return await Task.FromResult((int) ExitCodes.Success);
        }
    }
}