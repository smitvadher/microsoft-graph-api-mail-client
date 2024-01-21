namespace MailClientApp.Library.Email
{
    /// <summary>
    /// Represents the body of an email message, including its content type and actual content.
    /// </summary>
    public class EmailBody
    {
        /// <summary>
        /// Gets or sets the content type of the email body.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content of the email body.
        /// </summary>
        public string Content { get; set; }
    }
}
