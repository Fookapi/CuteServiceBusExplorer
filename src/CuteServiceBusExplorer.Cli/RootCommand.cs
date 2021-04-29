using System.Reflection;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli
{
    [Command(
        Name = "csbx", 
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw , 
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    /*[Subcommand(
        typeof(LoginCmd),
        typeof(ListTicketCmd))]*/
    internal class RootCommand : BaseCommand
    {      
        public RootCommand(ILogger<RootCommand> logger, IConsole console) : base(logger, console) {}

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHint();
            return Task.FromResult((int)ExitCodes.CommandLineUsageError);
        }

        private static string GetVersion()
            => typeof(RootCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        //See: https://edi.wang/post/2018/9/27/get-app-version-net-core
    }
}