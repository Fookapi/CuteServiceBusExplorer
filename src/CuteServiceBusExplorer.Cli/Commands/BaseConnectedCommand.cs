using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli.Commands
{
    public class BaseConnectedCommand : BaseCommand
    {
        [Required]
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "connectionKey",
            Description = "[Required] Connection key for the connection to use for this execution.", ShowInHelpText = true)]
        public string ConnectionKey { get; set; }
        
        protected BaseConnectedCommand(ILogger logger, IConsole console) : base(logger, console) { }
    }
}