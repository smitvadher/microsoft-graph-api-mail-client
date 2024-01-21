namespace MailClientApp.Library.Email
{
    /// <summary>
    /// Represents a reply to an email message.
    /// </summary>
    public class Reply
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reply"/> class.
        /// </summary>
        /// <param name="message">Details of the original message to reply to.</param>
        /// <param name="comment">Optional comment to include in the reply.</param>
        public Reply(ReplyRecipients message, string comment)
        {
            Message = message;
            Comment = comment;
        }

        /// <summary>
        /// Gets the optional comment to include in the reply.
        /// <remarks>
        /// NOTE: Specify either a comment or the body property of the message parameter.
        /// Specifying both will return an HTTP 400 Bad Request error.
        /// </remarks>
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Gets the details of the original message to reply to.
        /// </summary>
        public ReplyRecipients Message { get; }
    }
}
