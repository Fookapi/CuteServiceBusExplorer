using System.Reflection;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace CuteServiceBusExplorer.Cli.Commands
{
    [Command(
        Name = "csbx", 
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw , 
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(Connections.Root), typeof(Info), typeof(Topics.Root))]
    internal class Root : BaseCommand
    {      
        public Root(ILogger<Root> logger, IConsole console) : base(logger, console) {}

        protected override Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult((int)ExitCodes.CommandLineUsageError);
        }

        private static string GetVersion()
            => typeof(Root).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        //See: https://edi.wang/post/2018/9/27/get-app-version-net-core
    }
}