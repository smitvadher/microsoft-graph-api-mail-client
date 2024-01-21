namespace MailClientApp.Library.Email
{
    /// <summary>
    /// Represents a request to mark an email message as read or unread.
    /// </summary>
    public class ReadMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadMessage"/> class.
        /// </summary>
        /// <param name="markAsRead">Indicates whether to mark the email as read.</param>
        public ReadMessage(bool markAsRead)
        {
            IsRead = markAsRead;
        }

        /// <summary>
        /// Gets a value indicating whether the email should be marked as read.
        /// </summary>
        public bool IsRead { get; }
    }
}
