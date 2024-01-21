namespace MailClientApp.Library.Email
{
    /// <summary>
    /// Represents an email message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
            ReplyTo = new List<EmailDetails>();
            ToRecipients = new List<EmailDetails>();
        }

        /// <summary>
        /// Gets or sets the unique identifier of the message.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body of the email message.
        /// </summary>
        public EmailBody Body { get; set; }

        /// <summary>
        /// Gets or sets the details of the sender of the email.
        /// </summary>
        public EmailDetails Sender { get; set; }

        /// <summary>
        /// Gets or sets the details of the intended recipient of the email.
        /// </summary>
        public EmailDetails From { get; set; }

        /// <summary>
        /// Gets or sets the list of email details for reply recipients.
        /// </summary>
        public IList<EmailDetails> ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the list of email details for primary recipients.
        /// </summary>
        public IList<EmailDetails> ToRecipients { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email has been read.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email is a draft.
        /// </summary>
        public bool IsDraft { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the email message was created.
        /// </summary>
        public DateTimeOffset CreatedDateTime { get; set; }

        /// <summary>
        /// List of all property names of Message class.
        /// </summary>
        // NOTE: If any json ignore/readonly property is set below properties should exclude those names.
        public static readonly IReadOnlyList<string> GraphApiMessageProps = typeof(Message).GetProperties().Select(p => p.Name).ToList();
    }
}
