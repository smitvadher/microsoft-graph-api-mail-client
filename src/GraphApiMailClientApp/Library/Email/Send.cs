namespace MailClientApp.Library.Email
{
    /// <summary>
    /// Represents a request to send an email message.
    /// </summary>
    public class Send
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Send"/> class.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="saveToSendItems">Indicates whether to save the sent email in the sent items folder.</param>
        public Send(Message message, bool saveToSendItems)
        {
            Message = message;
            SaveToSendItems = saveToSendItems;
        }

        /// <summary>
        /// Gets the message to be sent.
        /// </summary>
        public Message Message { get; set; }

        /// <summary>
        /// Gets a value indicating whether to save the sent email in the sent items folder.
        /// </summary>
        public bool SaveToSendItems { get; set; }
    }
}
