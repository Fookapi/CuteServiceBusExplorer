using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli.Commands
{
    [HelpOption("--help")]
    public abstract class BaseCommand
    {
        protected readonly ILogger Logger;  
        protected  readonly IConsole Console;
        
        
        protected BaseCommand(ILogger logger, IConsole console)
        {
            Logger = logger;
            Console = console;
        }

        protected virtual Task<int> OnExecuteAsync(CommandLineApplication app)
        {            
            app.ShowHelp();
            return Task.FromResult(0);
        }
    }
}