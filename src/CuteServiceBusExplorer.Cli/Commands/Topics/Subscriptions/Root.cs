using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli.Commands.Topics.Subscriptions
{
    [Command(
        Name = "subscriptions", 
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw , 
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    [Subcommand(typeof(List), typeof(Peek))]
    public class Root : BaseCommand
    {
        public Root(ILogger<Root> logger, IConsole console) : base(logger, console) {}
        
        protected override Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult((int)ExitCodes.CommandLineUsageError);
        }
    }
}