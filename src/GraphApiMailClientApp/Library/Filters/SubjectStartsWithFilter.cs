using MailClientApp.Library.Email;

namespace GraphApiMailClientApp.Library.Filters
{
    /// <summary>
    /// Represents a filter condition based on the starting characters of an email subject.
    /// </summary>
    public class SubjectStartsWithFilter : SearchFilter
    {
        /// <summary>
        /// Initializes a new instance of the SubjectStartsWithFilter class.
        /// </summary>
        /// <param name="subjectStartsWith">The starting characters of the email subject.</param>
        public SubjectStartsWithFilter(string subjectStartsWith)
        {
            Value = subjectStartsWith;
        }

        protected override string PropertySelector => nameof(Message.Subject);
        protected override string Comparer => StartsWithComparer;
        protected override string Value { get; }
    }
}
