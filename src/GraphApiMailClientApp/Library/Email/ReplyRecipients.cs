namespace MailClientApp.Library.Email
{
    /// <summary>
    /// Represents the recipients for a reply to an email message.
    /// </summary>
    public class ReplyRecipients
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyRecipients"/> class.
        /// </summary>
        /// <param name="originalMessage">The original message to reply to.</param>
        /// <param name="replyToAll">Indicates whether the reply is to all recipients.</param>
        public ReplyRecipients(Message originalMessage, bool replyToAll)
        {
            ToRecipients = new List<EmailDetails>();

            //don't send ToRecipients when replying to all
            if (originalMessage != null && !replyToAll)
            {
                if (originalMessage.ReplyTo.Any())
                    ToRecipients = originalMessage.ReplyTo;

                else if (originalMessage.From != null)
                    ToRecipients.Add(originalMessage.From);

                else if (originalMessage.Sender != null)
                    ToRecipients.Add(originalMessage.Sender);
            }
        }

        /// <summary>
        /// Gets the list of email details for the recipients of the reply.
        /// </summary>
        public IList<EmailDetails> ToRecipients { get; }
    }
}
