using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Interface;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace CuteServiceBusExplorer.Cli.Commands.Topics.Subscriptions
{
    [Command(
        Name = "peek",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase, 
        ExtendedHelpText = "Note: Either a summary of messages will be displayed with --list or message details will be displayed by providing options for --sequenceNumber and/or arguments for messageIds.")]
    public class Peek : BaseConnectedCommand
    {
        private const int DefaultListDepth = 32;
        private const int MinimumListDepth = 1;
        private const int MaximumListDepth = 100;
        private readonly ITopicService _topicService;

        [Option(CommandOptionType.NoValue, ShortName = "u", LongName = "unformatted",
            Description = "Don't show formatted output, only print message IDs or message bodies.", ShowInHelpText = true)]
        public bool Unformatted { get; set; } = false;

        [Option(CommandOptionType.SingleOrNoValue, ShortName = "l", LongName = "list",
            Description = "Peek this amount of messages deep and display in a summary. The depth to peek can be provided with --list=50 with allowable values in the range [1..100]. Default = 32", ShowInHelpText = true)]
        public (bool hasValue, int? value) List { get; set; }

        [Option(CommandOptionType.MultipleValue, ShortName = "s", LongName = "sequenceNumber",
            Description = "Peek full message(s) at sequence number(s).", ShowInHelpText = true)]
        public int[] SequenceNumber { get; set; }
        
        [Option(CommandOptionType.SingleValue, ShortName = "o", LongName = "outputPath",
            Description = "Output messages selected through either --sequenceNumber or message ID arguments directly to JSON formed file. Note symbols like tilde need to be expanded by your shell before being passed to this option and using this option with an equal sign (eg. --outputPath=~/myFile) might cause your shell not expand this path properly.", ShowInHelpText = true)]
        public string OutputPath { get; set; }
        
        [Argument(order: 0, Description = "The message IDs to peek and show full message for",
            Name = "messageIds", ShowInHelpText = true)]
        public string[] MessageIds { get; set; }

        public Peek(ITopicService topicService, ILogger<List> logger, IConsole console) : base(logger, console)
        {
            _topicService = topicService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            bool isList = List.hasValue;
            bool isSequenceNumbers = SequenceNumber?.Any() ?? false;
            bool isMessageIds = MessageIds?.Any() ?? false;
            bool isDetailsRequest = isSequenceNumbers || isMessageIds;
            bool outputToFileRequested = !string.IsNullOrWhiteSpace(OutputPath);

            if (isList && isDetailsRequest)
            {
                AnsiConsole.MarkupLine("[darkorange]Please provide details to peek specific messages (--sequenceNumber and/or messageIds arguments) or request a summary with --list. Not both.[/]");
                return await ExitCodesResult.TaskForCommandLineUsageError;
            }
            if(!isList && !isDetailsRequest)
            {
                AnsiConsole.MarkupLine("[darkorange]Please provide details to peek specific messages (--sequenceNumber and/or messageIds arguments) or request a summary with --list.[/]");
                return await ExitCodesResult.TaskForCommandLineUsageError;
            }

            if(outputToFileRequested)
                AnsiConsole.MarkupLine($"[default]Output to be written to path {Path.GetFullPath(OutputPath)} as a JSON object.[/]");
            
            if (isList)
            {
                int listDepth = List.value ?? DefaultListDepth;

                if (listDepth < MinimumListDepth || listDepth > MaximumListDepth)
                {
                    AnsiConsole.MarkupLine($"[darkorange]List depth of {listDepth:N0} is not a valid value. Please provide a list depth for the --list option in the range [[{MinimumListDepth}..{MaximumListDepth}]].[/]");
                    return await ExitCodesResult.TaskForCommandLineUsageError;
                }
            }
            
            
            
            
            
            return await ExitCodesResult.TaskForSuccess;
        }

        private void RenderPlainTopics(GetSubscriptionsForTopicResponse subscriptionResponse)
        {
            foreach (var subscription in subscriptionResponse.Subscriptions)
            {
                AnsiConsole.WriteLine(subscription.Name);
            }
        }

        private void RenderFormattedTopics(GetSubscriptionsForTopicResponse subscriptionResponse)
        {
            AnsiConsole.MarkupLine($"Connection Key: [blue]{ConnectionKey}[/]");

            var table = new Table();

            var nameColumn = new TableColumn("Name");
            var maximumDeliveriesColumn = new TableColumn("Maximum Deliveries");
            var messageTtlColumn = new TableColumn("Message TTL");
            var messageLockDurationColumn = new TableColumn("Message Lock Duration");
            var messageAutoDeleteIdleTimeColumn = new TableColumn("Message Auto-Delete Idle Time");
            var activeMessageColumn = new TableColumn("Active Messages");
            var deadLetterMessageCountColumn = new TableColumn("Dead-Letter Messages");
            var transferMessageCountColumn = new TableColumn("Transferred Messages");
            var scheduledMessageCountColumn = new TableColumn("Scheduled Messages");
            var statusColumn = new TableColumn("Status");
            var forwardToColumn = new TableColumn("Forward To");

            table.AddColumn(nameColumn);
            table.AddColumn(statusColumn);
            table.AddColumn(activeMessageColumn);
            table.AddColumn(deadLetterMessageCountColumn);
            table.AddColumn(transferMessageCountColumn);
            table.AddColumn(scheduledMessageCountColumn);
            table.AddColumn(messageTtlColumn);
            table.AddColumn(messageLockDurationColumn);
            table.AddColumn(messageAutoDeleteIdleTimeColumn);
            table.AddColumn(maximumDeliveriesColumn);
            table.AddColumn(forwardToColumn);

            table.Border = TableBorder.Minimal;

            foreach (var subscription in subscriptionResponse.Subscriptions)
            {
                var nameValue = new Markup($"[blue]{subscription.Name}[/]");
                var statusValue = new Markup($"[{(subscription.Status == "Active" ? "grey" : "darkorange")}]{subscription.Status}[/]");
                var activeMessageValue = new Markup($"[default]{subscription.ActiveMessageCount:N0}[/]");
                var deadLetterValue = new Markup($"[default]{(subscription.DeadLetteringEnabled ? subscription.DeadLetterMessageCount.ToString("N0") : "n/a")}[/]");
                var messageTransferCountValue = new Markup($"[grey]{subscription.TransferMessageCount:N0}[/]");
                var messageScheduleCountValue = new Markup($"[grey]{subscription.ScheduledMessageCount:N0}[/]");
                var messageTtlValue = new Markup($"[grey]{subscription.MessageTimeToLive:c}[/]");
                var messageLockDurationValue = new Markup($"[grey]{subscription.MessageLockDuration:c}[/]");
                var autoDeleteOnIdleForValue = new Markup($"[grey]{subscription.AutoDeleteAfterIdleFor:c}[/]");
                var maximumDeliveryCountValue = new Markup($"[grey]{subscription.MaximumDeliveryCount:N0}[/]");
                var forwardToValue = new Markup($"[grey]{subscription.ForwardMessagesTo}[/]");

                table.AddRow(
                    nameValue,
                    statusValue,
                    activeMessageValue,
                    deadLetterValue,
                    messageTransferCountValue,
                    messageScheduleCountValue,
                    messageTtlValue,
                    messageLockDurationValue,
                    autoDeleteOnIdleForValue,
                    maximumDeliveryCountValue,
                    forwardToValue);
            }

            AnsiConsole.Render(table);
        }

    }
}