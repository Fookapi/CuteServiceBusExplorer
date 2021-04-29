using System.Reflection;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli
{
    [Command(
        Name = "csbx", 
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw , 
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase )]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    /*[Subcommand(
        typeof(LoginCmd),
        typeof(ListTicketCmd))]*/
    internal class RootCommand : BaseCommand
    {      
        public RootCommand(ILogger<RootCommand> logger, IConsole console) : base(logger, console) {}

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();            
            return Task.FromResult(0);
        }

        private static string GetVersion()
            => typeof(RootCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    }
}