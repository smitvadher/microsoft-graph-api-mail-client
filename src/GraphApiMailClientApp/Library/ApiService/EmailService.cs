using GraphApiMailClientApp.Library.Config;
using GraphApiMailClientApp.Library.Exceptions;
using GraphApiMailClientApp.Library.Filters;
using MailClientApp.Library.Email;
using Microsoft.Identity.Client;

namespace GraphApiMailClientApp.Library.ApiService
{
    /// <summary>
    /// Service for interacting with Microsoft Graph API to manage emails.
    /// </summary>
    public class EmailService
    {
        #region Fields

        private readonly GraphApiConfig _graphApiConfig;
        private readonly string[] _tokenApiScopes;
        private IConfidentialClientApplication _app;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="graphApiConfig">The configuration for Microsoft Graph API.</param>
        public EmailService(GraphApiConfig graphApiConfig)
        {
            ArgumentNullException.ThrowIfNull(graphApiConfig);

            var errorMessages = new List<string>();

            if (string.IsNullOrEmpty(graphApiConfig.Tenant))
                errorMessages.Add("Please provide Tenant(Directory ID) from azure portal in config.");

            if (string.IsNullOrEmpty(graphApiConfig.ClientId))
                errorMessages.Add("Please provide ClientId(Application ID) from azure portal in config.");

            if (string.IsNullOrEmpty(graphApiConfig.ClientSecret))
                errorMessages.Add("Please provide ClientSecret from azure portal in config.");

            if (string.IsNullOrEmpty(graphApiConfig.ReadEmailsFrom))
                errorMessages.Add("Please provide Email address to ReadEmailsFrom in config.");

            if (errorMessages.Count != 0)
                throw new ConfigException(errorMessages);

            _tokenApiScopes = new[] { $"{graphApiConfig.ApiUrl}.default" };
            _graphApiConfig = graphApiConfig;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Builds and returns a ConfidentialClientApplication for authentication.
        /// </summary>
        private IConfidentialClientApplication BuildConfidentialClientApplication()
        {
            if (_app != null) return _app;

            _app = ConfidentialClientApplicationBuilder.Create(_graphApiConfig.ClientId)
                .WithClientSecret(_graphApiConfig.ClientSecret)
                .WithAuthority(new Uri(_graphApiConfig.Authority))
                .Build();

            return _app;
        }

        /// <summary>
        /// Acquires and caches a token for authentication.
        /// </summary>
        private async Task<AuthenticationResult> AcquireAccessTokenAsync()
        {
            var app = BuildConfidentialClientApplication();

            try
            {
                return await app.AcquireTokenForClient(_tokenApiScopes).ExecuteAsync();
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope.
                // Mitigation: change the scope to be as expected
                throw new GraphApiException("Graph API scope provided is not supported");
            }
            catch (Exception ex)
            {
                throw new GraphApiException($"Exception occurred while acquiring the access token for GraphAPI.{Environment.NewLine}{ex.GetBaseException()}");
            }
        }

        /// <summary>
        /// Constructs the URL for retrieving emails based on the provided search filter and selected properties.
        /// </summary>
        private string GetEmailsUrl(IEnumerable<SearchFilter> searchFilters, params string[] propertiesToSelect)
        {
            var url = _graphApiConfig.MessagesAbsoluteUrl;
            url += $"?$select={string.Join(",", propertiesToSelect)}";
            url += $"&$top={999999}";
            url += $"&$filter={SearchFilter.GetConditions(searchFilters)}";

            return url;
        }

        private string GetEmailsUrl(IEnumerable<SearchFilter> searchFilters)
        {
            return GetEmailsUrl(searchFilters, Message.GraphApiMessageProps.ToArray());
        }

        private string GetReplyUrl(Message originalMessage, bool replyAll)
        {
            var url = _graphApiConfig.MessagesAbsoluteUrl;
            url += $"/{originalMessage.Id}/";
            url += replyAll ? "replyAll" : "reply";
            return url;
        }

        private string GetSendUrl()
        {
            var url = _graphApiConfig.SendUrl;
            return url;
        }

        private string GetSingleMessageUrl(string id)
        {
            var url = _graphApiConfig.MessagesAbsoluteUrl;
            url += $"/{id}";
            return url;
        }

        private string GetSingleMessageRelativeUrl(string id)
        {
            var url = _graphApiConfig.MessagesRelativeUrl;
            url += $"/{id}";
            return url;
        }

        #endregion

        /// <summary>
        /// Get all emails based on specified filter criteria.
        /// </summary>
        /// <remarks>
        /// Requires one of the following permissions: [Mail.ReadBasic.All, Mail.Read, Mail.ReadWrite].
        /// For more information, refer to the <see href="https://docs.microsoft.com/en-us/graph/permissions-reference#mail-permissions"/>
        /// </remarks>
        public async Task<IList<Message>> GetEmailsAsync(IEnumerable<SearchFilter> searchFilters)
        {
            var tokenResponse = await AcquireAccessTokenAsync();

            var emails = await ApiRequestHelper.GetAsync<OdataResponse<Message>>(GetEmailsUrl(searchFilters), tokenResponse?.AccessToken);

            return emails?.Value;
        }

        /// <summary>
        /// Marks the selected email as read.
        /// </summary>
        /// <remarks>
        /// Requires one of the following permissions: [Mail.ReadWrite].
        /// For more information, refer to the <see href="https://docs.microsoft.com/en-us/graph/permissions-reference#mail-permissions"/>
        /// </remarks>
        public async Task MarkAsReadAsync(string id)
        {
            var tokenResponse = await AcquireAccessTokenAsync();

            var readRequest = new ReadMessage(true);

            await ApiRequestHelper.PatchAsync(GetSingleMessageUrl(id), tokenResponse?.AccessToken, readRequest);
        }

        /// <summary>
        /// Reply to the selected email.
        /// </summary>
        /// <remarks>
        /// Requires one of the following permissions: [Mail.Send].
        /// For more information, refer to the <see href="https://docs.microsoft.com/en-us/graph/permissions-reference#mail-permissions"/>
        /// </remarks>
        public async Task ReplyAsync(string text, Message message, bool replyToAll)
        {
            var tokenResponse = await AcquireAccessTokenAsync();

            var replyRequest = new Reply(new ReplyRecipients(message, replyToAll), text);

            await ApiRequestHelper.PostAsync(GetReplyUrl(message, replyToAll), tokenResponse?.AccessToken, replyRequest);
        }

        /// <summary>
        /// Sends an email to selected recipients.
        /// </summary>
        /// <remarks>
        /// Requires one of the following permissions: [Mail.Send].
        /// For more information, refer to the <see href="https://docs.microsoft.com/en-us/graph/permissions-reference#mail-permissions"/>
        /// </remarks>
        public async Task SendAsync(Message message, bool saveToSendItems = false)
        {
            var tokenResponse = await AcquireAccessTokenAsync();

            var replyRequest = new Send(message, saveToSendItems);

            await ApiRequestHelper.PostAsync(GetSendUrl(), tokenResponse?.AccessToken, replyRequest);
        }

        /// <summary>
        /// Deletes the selected emails.
        /// </summary>
        /// <remarks>
        /// Requires one of the following permissions: [Mail.ReadWrite].
        /// For more information, refer to the <see href="https://docs.microsoft.com/en-us/graph/permissions-reference#mail-permissions"/>
        /// </remarks>
        public async Task DeleteEmailsAsync(IEnumerable<SearchFilter> searchFilters)
        {
            var tokenResponse = await AcquireAccessTokenAsync();
            var url = GetEmailsUrl(searchFilters, nameof(Message.Id));
            var messageToDelete = await ApiRequestHelper.GetAsync<OdataResponse<Message>>(url, tokenResponse?.AccessToken);

            if (messageToDelete == null || !messageToDelete.Value.Any()) return;

            var allRequests = messageToDelete.Value.Select((m, i) =>
                new BatchRequest.Request(i, BatchRequest.Methods.Delete, GetSingleMessageRelativeUrl(m.Id)));

            foreach (var batchRequest in BatchRequest.CreateBatchRequests(allRequests))
                await ApiRequestHelper.PostAsync(_graphApiConfig.BatchUrl, tokenResponse?.AccessToken, batchRequest);
        }
    }
}
