using MailClientApp.Library.Email;

namespace GraphApiMailClientApp.Library.Filters
{
    /// <summary>
    /// Represents a filter condition based on the read status of an email.
    /// </summary>
    public class IsReadFilter : SearchFilter
    {
        /// <summary>
        /// Initializes a new instance of the IsReadFilter class.
        /// </summary>
        /// <param name="isRead">Indicates whether the email is read or unread.</param>
        public IsReadFilter(bool isRead)
        {
            Value = isRead.ToString().ToLower();
        }

        protected override string PropertySelector => nameof(Message.IsRead);
        protected override string Comparer => EqComparer;
        protected override string Value { get; }
    }
}
