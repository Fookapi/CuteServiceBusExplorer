using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli.Commands
{
    [Command(
        Name = "info", 
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw , 
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    public class Info : BaseCommand
    {
        public Info(ILogger<Info> logger, IConsole console) : base(logger, console) {}
        
        protected override Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult((int)ExitCodes.CommandLineUsageError);
        }
    }
}