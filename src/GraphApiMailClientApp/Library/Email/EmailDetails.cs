namespace MailClientApp.Library.Email
{
    /// <summary>
    /// Represents the details of an email, including the email address and associated information.
    /// </summary>
    public class EmailDetails
    {
        /// <summary>
        /// Gets or sets the details of the email address.
        /// </summary>
        public AddressDetails EmailAddress { get; set; }

        /// <summary>
        /// Represents the details of an email address, including the name and address.
        /// </summary>
        public class AddressDetails
        {
            /// <summary>
            /// Gets or sets the name associated with the email address.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the email address.
            /// </summary>
            public string Address { get; set; }
        }
    }
}
