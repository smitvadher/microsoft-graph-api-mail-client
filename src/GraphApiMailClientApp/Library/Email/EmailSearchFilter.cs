namespace GraphApiMailClientApp.Library.Email
{
    /// <summary>
    /// Represents a filter for searching emails based on specified criteria.
    /// </summary>
    public class EmailSearchFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSearchFilter"/> class.
        /// </summary>
        /// <param name="subjectStartsWith">The prefix for filtering emails by subject.</param>
        /// <param name="isRead">Indicates whether the email has been read.</param>
        /// <param name="lessThanCreateDateTime">Filters emails created before the specified date and time.</param>
        public EmailSearchFilter(string subjectStartsWith = null, bool? isRead = null, DateTime? lessThanCreateDateTime = null)
        {
            SubjectStartsWith = subjectStartsWith;
            IsRead = isRead;
            LessThanCreateDateTime = lessThanCreateDateTime;
            HasFilterValues = !string.IsNullOrEmpty(SubjectStartsWith) ||
                              IsRead.HasValue ||
                              LessThanCreateDateTime.HasValue;
        }

        /// <summary>
        /// Gets the prefix for filtering emails by subject.
        /// </summary>
        public string SubjectStartsWith { get; }

        /// <summary>
        /// Gets a value indicating whether the email has been read.
        /// </summary>
        public bool? IsRead { get; }

        /// <summary>
        /// Gets the date and time for filtering emails created before the specified value.
        /// </summary>
        public DateTime? LessThanCreateDateTime { get; }

        /// <summary>
        /// Gets a value indicating whether the filter has any non-null values.
        /// </summary>
        public bool HasFilterValues { get; }
    }
}