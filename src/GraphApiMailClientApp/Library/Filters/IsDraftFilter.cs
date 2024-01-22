using MailClientApp.Library.Email;

namespace GraphApiMailClientApp.Library.Filters
{
    /// <summary>
    /// Represents a filter condition based on whether an email is in draft state.
    /// </summary>
    public class IsDraftFilter : SearchFilter
    {
        /// <summary>
        /// Initializes a new instance of the IsDraftFilter class.
        /// </summary>
        /// <param name="isDraft">Indicates whether the email is in draft state.</param>
        public IsDraftFilter(bool isDraft)
        {
            Value = isDraft.ToString().ToLower();
        }

        protected override string PropertySelector => nameof(Message.IsDraft);
        protected override string Comparer => EqComparer;
        protected override string Value { get; }
    }
}
