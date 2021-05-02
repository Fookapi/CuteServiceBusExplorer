using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli.Commands.Connections
{
    [Command(
        Name = "remove",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    public class Remove : BaseCommand
    {
        public Remove(ILogger<Remove> logger, IConsole console) : base(logger, console)
        {
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult((int) ExitCodes.CommandLineUsageError);
        }
    }
}