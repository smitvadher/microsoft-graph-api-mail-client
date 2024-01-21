using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace GraphApiMailClientApp.Library.ApiService
{
    /// <summary>
    /// Utility class for making HTTP requests to a API, specifically designed for interactions with the Microsoft Graph API.
    /// Provides methods for common HTTP operations such as GET, POST, PATCH, and DELETE.
    /// </summary>
    public static class ApiRequestHelper
    {
        #region Fields

        private const string ApplicationJson = "application/json";
        private static readonly HttpMethod Patch = new("PATCH");

        #endregion

        #region Private methods

        /// <summary>
        /// Sends a PATCH request asynchronously using an HttpClient, given a request URI and content.
        /// </summary>
        private static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            return client.SendAsync(new HttpRequestMessage(Patch, requestUri)
            {
                Content = content
            });
        }

        /// <summary>
        /// Creates and configures an HttpClient with the necessary headers, including authorization using an access token.
        /// </summary>
        private static HttpClient GetHttpClient(string accessToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return httpClient;
        }

        /// <summary>
        /// Converts a generic request model into StringContent with JSON serialization and proper content type headers.
        /// </summary>
        private static StringContent GetStringContent<T>(T requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            var contentString = new StringContent(json, Encoding.UTF8, ApplicationJson);
            contentString.Headers.ContentType = new MediaTypeHeaderValue(ApplicationJson);

            return contentString;
        }

        /// <summary>
        /// Handles failure responses by constructing informative error messages, including details about the request and response.
        /// </summary>
        private static async Task HandleFailureResponseAsync(HttpResponseMessage response, string requestUrl, object requestModel)
        {
            // NOTE: If response.Code is 403 and response.content.code is Authorization_RequestDenied
            // is because the tenant admin has not granted consent for the application to call the Web API
            var content = await response.Content.ReadAsStringAsync();

            var messages = new List<string>
            {
                $"Failed to call the Graph API: {(int)response.StatusCode}-{response.ReasonPhrase}",
                $"URL: {requestUrl}"
            };

            if (requestModel != null)
                messages.Add($"RequestData: {JsonConvert.SerializeObject(requestModel, Formatting.Indented)}");
            messages.Add($"Response: {content}");

            throw new HttpRequestException(string.Join(Environment.NewLine, messages));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an asynchronous HTTP GET request
        /// Deserializes the response content into a specified type T.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response content into.</typeparam>
        /// <param name="requestUri">The URI of the resource to request.</param>
        /// <param name="accessToken">The access token for authorization.</param>
        /// <returns>The deserialized response content.</returns>
        public static async Task<T> GetAsync<T>(string requestUri, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) return default;

            var httpClient = GetHttpClient(accessToken);
            var response = await httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                await HandleFailureResponseAsync(response, requestUri, null);

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Performs an asynchronous HTTP POST request
        /// </summary>
        /// <param name="requestUri">The URI of the resource to request.</param>
        /// <param name="accessToken">The access token for authorization.</param>
        /// <param name="requestModel">The data to include in the request body.</param>
        public static async Task PostAsync(string requestUri, string accessToken, object requestModel)
        {
            if (string.IsNullOrEmpty(accessToken)) return;
            var httpClient = GetHttpClient(accessToken);

            var requestContent = GetStringContent(requestModel);
            var response = await httpClient.PostAsync(requestUri, requestContent);

            if (!response.IsSuccessStatusCode)
                await HandleFailureResponseAsync(response, requestUri, requestModel);
        }

        /// <summary>
        /// Performs an asynchronous HTTP PATCH request
        /// </summary>
        /// <param name="requestUri">The URI of the resource to request.</param>
        /// <param name="accessToken">The access token for authorization.</param>
        /// <param name="requestModel">The data to include in the request body.</param>
        public static async Task PatchAsync(string requestUri, string accessToken, object requestModel)
        {
            if (string.IsNullOrEmpty(accessToken)) return;
            var httpClient = GetHttpClient(accessToken);

            var requestContent = GetStringContent(requestModel);
            var response = await httpClient.PatchAsync(requestUri, requestContent);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                await HandleFailureResponseAsync(response, requestUri, requestModel);
        }

        /// <summary>
        /// Performs an asynchronous HTTP DELETE request
        /// </summary>
        /// <param name="requestUri">The URI of the resource to request.</param>
        /// <param name="accessToken">The access token for authorization.</param>
        public static async Task DeleteAsync(string requestUri, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) return;
            var httpClient = GetHttpClient(accessToken);

            var response = await httpClient.DeleteAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                await HandleFailureResponseAsync(response, requestUri, null);
        }

        #endregion
    }
}
