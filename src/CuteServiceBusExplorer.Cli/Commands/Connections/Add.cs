using System.Collections.Generic;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli.Commands.Connections
{
    [Command(
        Name = "add",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    public class Add : BaseCommand
    {
        public Add(ILogger<Add> logger, IConsole console) : base(logger, console)
        {
        }

        protected override Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult((int) ExitCodes.CommandLineUsageError);
        }
    }
}