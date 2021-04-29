namespace CuteServiceBusExplorer.Cli
{
    /// <summary>
    /// Exit codes inspired by https://tldp.org/LDP/abs/html/exitcodes.html
    /// </summary>
    public enum ExitCodes
    {
        Success = 0,
        GeneralError = 1,
        CommandLineUsageError = 2, //https://stackoverflow.com/a/40484670
        /// <summary>
        /// SIGINT: Interrupt from keyboard
        /// </summary>
        ControlC = 128 + 2
    }
}