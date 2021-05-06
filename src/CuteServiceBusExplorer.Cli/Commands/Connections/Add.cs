using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace CuteServiceBusExplorer.Cli.Commands.Connections
{
    [Command(
        Name = "add",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    public class Add : BaseCommand
    {
        private readonly IConnectionService _connectionService;
        private const string KeyValidationRegex = @"^[\w-]{0,24}[\w]$";
        private const string NameValidationRegex = @"^[\w][\w\s-&$#@()\[\]]{0,98}[\w)\]]$";
        
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "connectionString", Description = "Azure Service Bus connection string defining the connection", ShowInHelpText = true)]
        public string ConnectionString { get; set; }
        
        [Option(CommandOptionType.SingleValue, ShortName = "k", LongName = "key", Description = "Key to refer to this connection in future requests. Format regex: " + KeyValidationRegex + "", ShowInHelpText = true)]
        public string Key { get; set; }
        
        [Option(CommandOptionType.SingleValue, ShortName = "n", LongName = "name", Description = "Display name for the connection. Format regex: " + NameValidationRegex + "", ShowInHelpText = true)]
        public string Name { get; set; }
        
        public Add(IConnectionService connectionService, ILogger<Add> logger, IConsole console) : base(logger, console)
        {
            _connectionService = connectionService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            string connectionString; //See discussion and links in: https://github.com/Azure/azure-service-bus-dotnet/issues/651

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                var prompt = new TextPrompt<string>("Connection String: ");
                connectionString = AnsiConsole.Prompt(prompt); 
            }
            else
                connectionString = ConnectionString;

            var key = GetRequiredValue("Key", Key, ValidateKeyFormat);
            var name = GetRequiredValue("Name", Name, ValidateNameFormat);

            var result = await _connectionService.TryAddConnection(key, name, connectionString);

            if (result)
            {
                AnsiConsole.MarkupLine($"[grey]{key}[/]");
                return await ExitCodesResult.TaskForSuccess;
            }

            return await ExitCodesResult.TaskForGeneralError;
        }

        private bool ValidateNameFormat(string name)
        {
            if (Regex.IsMatch(name, NameValidationRegex))
                return true;
            
            
            AnsiConsole.MarkupLine($"[darkorange]Provided name value does not match the regex expression '{NameValidationRegex.EscapeMarkup()}'. Please try again.[/]");

            return false;
        }

        private bool ValidateKeyFormat(string key)
        {
            if (Regex.IsMatch(key, KeyValidationRegex))
                return true;
            
            AnsiConsole.MarkupLine($"[darkorange]Provided key value does not match the regex expression '{KeyValidationRegex.EscapeMarkup()}'. Please try again.[/]");

            return false;
        }

        
    }
}