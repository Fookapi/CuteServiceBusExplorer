using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Spectre.Console;

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
        
        protected string GetRequiredValue(string valueName, string inputParm, Func<string, bool> validationFunction)
        {
            string value = string.Empty;
            bool formatOk = false;
            bool firstPass = true;
            
            while(!formatOk)
            {
                if (string.IsNullOrWhiteSpace(inputParm) || !firstPass)
                {
                    var prompt = new TextPrompt<string>($"{valueName}: ");
                    value = AnsiConsole.Prompt(prompt);
                }
                else
                    value = inputParm;

                formatOk = validationFunction(value);
                firstPass = false;
            }

            return value;
        }
    }
}