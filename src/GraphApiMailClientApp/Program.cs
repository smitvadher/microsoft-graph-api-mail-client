using GraphApiMailClientApp.Library.ApiService;
using GraphApiMailClientApp.Library.Config;
using GraphApiMailClientApp.Library.Filters;
using MailClientApp.Library.Email;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    //.AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var config = new GraphApiConfig();
configuration.GetSection(nameof(GraphApiConfig)).Bind(config);

var emailService = new EmailService(config);

// Example: Get all emails
var searchFilters = new List<SearchFilter>
{
    new SubjectStartsWithFilter("Test"),
    new IsReadFilter(false),
    new IsDraftFilter(false),
    new LessThanCreateDateTimeFilter(DateTime.Now.AddDays(-1))
};
var emails = await emailService.GetEmailsAsync(searchFilters);
Console.WriteLine($"Total Emails: {emails?.Count}");

// Example: Mark an email as read
if (emails?.Count > 0)
{
    var firstEmailId = emails[0].Id;
    await emailService.MarkAsReadAsync(firstEmailId);
    Console.WriteLine($"Marked email with ID {firstEmailId} as read.");
}

// Example: Reply to an email
if (emails?.Count > 1)
{
    var secondEmailId = emails[1].Id;
    var replyText = "This is a sample reply.";
    await emailService.ReplyAsync(replyText, emails[1], false);
    Console.WriteLine($"Replied to email with ID {secondEmailId}.");
}

// Example: Send a new email
var newMessage = new Message
{
    // Set your email properties here
    Subject = "Test Email",
    Body = new EmailBody { Content = "This is a test email body.", ContentType = "text/plain" },
    ToRecipients = new List<EmailDetails>
    {
        new()
        {
            EmailAddress = new EmailDetails.AddressDetails { Name = "Recipient Name", Address = "recipient@example.com" }
        }
    }
};
await emailService.SendAsync(newMessage, saveToSendItems: false);
Console.WriteLine("Sent a new email.");

// Example: Delete emails
await emailService.DeleteEmailsAsync(searchFilters);
Console.WriteLine("Deleted emails based on filter criteria.");