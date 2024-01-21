namespace GraphApiMailClientApp.Library.ApiService
{
    /// <summary>
    /// Represents a batch of HTTP requests to be processed together. The batch can include multiple individual requests.
    /// </summary>
    public class BatchRequest
    {
        private const int MaxBatchSize = 20;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchRequest"/> class with a collection of individual requests.
        /// </summary>
        /// <param name="requests">The collection of individual requests within the batch.</param>
        public BatchRequest(IEnumerable<Request> requests)
        {
            Requests = requests;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collection of individual requests within the batch.
        /// </summary>
        public IEnumerable<Request> Requests { get; }

        #endregion

        #region Nested Classes

        /// <summary>
        /// Represents the available HTTP methods for individual requests within the batch.
        /// </summary>
        public enum Methods
        {
            Delete
        }

        /// <summary>
        /// Represents an individual request within the batch.
        /// </summary>
        public class Request
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Request"/> class.
            /// </summary>
            /// <param name="id">A correlation value to associate individual responses with requests.</param>
            /// <param name="method">The HTTP method.</param>
            /// <param name="url">The relative resource URL the individual request would typically be sent to.</param>
            public Request(int id, Methods method, string url)
            {
                Id = id.ToString();
                Method = method.ToString().ToUpper();
                Url = url;
            }

            /// <summary>
            /// Gets a correlation value to associate individual responses with requests.
            /// </summary>
            public string Id { get; }

            /// <summary>
            /// Gets the HTTP method.
            /// </summary>
            public string Method { get; }

            /// <summary>
            /// Gets the relative resource URL the individual request would typically be sent to.
            /// </summary>
            public string Url { get; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a collection of batch requests from a sequence of individual requests, considering a maximum batch size.
        /// </summary>
        /// <param name="allRequests">The sequence of individual requests to be grouped into batches.</param>
        /// <returns>A collection of batch requests.</returns>
        public static IEnumerable<BatchRequest> CreateBatchRequests(IEnumerable<Request> allRequests)
        {
            return GetMaxBatchList(allRequests).Select(requests => new BatchRequest(requests));
        }

        #endregion

        #region Private methods

        private static IEnumerable<IEnumerable<T>> GetMaxBatchList<T>(IEnumerable<T> source)
        {
            T[] batch = null;
            var count = 0;

            foreach (var item in source)
            {
                batch ??= new T[MaxBatchSize];

                batch[count++] = item;

                if (count != MaxBatchSize)
                    continue;

                yield return batch;

                batch = null;
                count = 0;
            }

            // last batch with all remaining elements
            if (batch == null || count <= 0) yield break;

            Array.Resize(ref batch, count);
            yield return batch;
        }

        #endregion
    }
}
