using System.Threading.Tasks;

namespace CuteServiceBusExplorer.Cli
{
    public static class ExitCodesResult
    {
        public static Task<int> TaskFromResult(ExitCodes exitCode) => Task.FromResult<int>((int) exitCode);
        public static Task<int> TaskForSuccess => TaskFromResult(ExitCodes.Success);
        public static Task<int> TaskForGeneralError => TaskFromResult(ExitCodes.GeneralError);
        public static Task<int> TaskForCommandLineUsageError => TaskFromResult(ExitCodes.CommandLineUsageError);
    }
}