using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CuteServiceBusExplorer.Infrastructure;
using CuteServiceBusExplorer.Interface;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace CuteServiceBusExplorer.Cli.Commands.Topics.Subscriptions
{

    [Command(
        Name = "list",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw,
        OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    public class List : BaseConnectedCommand
    {
        private readonly ITopicService _topicService;

        [Required]
        [Argument(order: 0, Description = "[Required] The name of the Azure Service Bus topic to get subscriptions for",
            Name = "sub", ShowInHelpText = true)]
        public string Topic { get; set; }

        [Option(CommandOptionType.NoValue, ShortName = "u", LongName = "unformatted",
            Description = "Don't show formatted output, only print topic names.", ShowInHelpText = true)]
        public bool Unformatted { get; set; } = false;

        public List(ITopicService topicService, ILogger<List> logger, IConsole console) : base(logger, console)
        {
            _topicService = topicService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            if (string.IsNullOrWhiteSpace(Topic))
            {
                AnsiConsole.MarkupLine("[darkorange]Please provide a topic name as an argument.[/]");
                return await ExitCodesResult.TaskForCommandLineUsageError;
            }

            var subscriptionResponse = await _topicService.GetSubscriptionsForTopic(Topic, ConnectionKey);

            if (subscriptionResponse?.Subscriptions == null)
                return await ExitCodesResult.TaskForSuccess;

            if (Unformatted)
                RenderPlainTopics(subscriptionResponse);
            else
                RenderFormattedTopics(subscriptionResponse);

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
            var createdAtColumn = new TableColumn("Created At");
            var updatedAtColumn = new TableColumn("Updated At");


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
                //var createdAtValue = new Markup($"[grey]{subscription.CreatedAt:s}[/]");
                //var updatedAtValue = new Markup($"[grey]{subscription.UpdatedAt:s}[/]");

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