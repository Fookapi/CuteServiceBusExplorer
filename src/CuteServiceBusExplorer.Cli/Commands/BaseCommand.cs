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
        
        //TODO: Replace these options with applicable options. This is just examples.

        [Option(CommandOptionType.SingleValue, ShortName = "", LongName = "profile", Description = "local profile name", ValueName = "profile name", ShowInHelpText = true)]
        public string Profile { get; set; } = "default";

        [Option(CommandOptionType.SingleValue, ShortName = "", LongName = "output-format", Description = "output format", ValueName = "output format", ShowInHelpText = true)]
        public string OutputFormat { get; set; } = "json";

        [Option(CommandOptionType.SingleValue, ShortName = "o", LongName = "output", Description = "output file", ValueName = "output file", ShowInHelpText = true)]
        public string OutputFile { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "", LongName = "xslt", Description = "xslt input file for transformation", ValueName = "xslt file", ShowInHelpText = true)]
        public string XSLTFile { get; set; }

        protected string FileNameSuffix { get; set; }

        protected BaseCommand(ILogger logger, IConsole console)
        {
            Logger = logger;
            Console = console;
        }

        protected virtual Task<int> OnExecute(CommandLineApplication app)
        {            
            return Task.FromResult(0);
        }
    }
}