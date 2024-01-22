using MailClientApp.Library.Email;

namespace GraphApiMailClientApp.Library.Filters
{
    /// <summary>
    /// Represents a filter condition based on emails created before a specified date and time.
    /// </summary>
    public class LessThanCreateDateTimeFilter : SearchFilter
    {
        /// <summary>
        /// Initializes a new instance of the LessThanCreateDateTimeFilter class.
        /// </summary>
        /// <param name="lessThanCreateDateTime">The date and time for filtering emails created before it.</param>
        public LessThanCreateDateTimeFilter(DateTime lessThanCreateDateTime)
        {
            Value = lessThanCreateDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        protected override string PropertySelector => nameof(Message.CreatedDateTime);
        protected override string Comparer => LtComparer;
        protected override string Value { get; }
    }
}
