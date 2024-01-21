namespace GraphApiMailClientApp.Library.Exceptions
{
    /// <summary>
    /// Exception thrown for errors related to Microsoft Graph API operations.
    /// </summary>
    [Serializable]
    public class GraphApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphApiException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public GraphApiException(string message) : base(message)
        {
        }
    }
}
