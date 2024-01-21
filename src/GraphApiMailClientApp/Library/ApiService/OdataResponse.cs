namespace GraphApiMailClientApp.Library.ApiService
{
    /// <summary>
    /// Represents the response structure for OData queries, including a collection of values of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data contained in the response.</typeparam>
    public class OdataResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdataResponse{T}"/> class.
        /// </summary>
        public OdataResponse()
        {
            Value = new List<T>();
        }

        /// <summary>
        /// Gets or sets the collection of values in the response.
        /// </summary>
        public IList<T> Value { get; set; }
    }
}
