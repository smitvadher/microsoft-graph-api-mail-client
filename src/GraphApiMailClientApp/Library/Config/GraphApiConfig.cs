using System.Globalization;

namespace GraphApiMailClientApp.Library.Config
{
    /// <summary>
    /// Description of the configuration of an AzureAD public client application (desktop/mobile application). This should
    /// match the application registration done in the Azure portal
    /// </summary>
    public class GraphApiConfig
    {
        private const string DefaultInstance = "https://login.microsoftonline.com/{0}";
        private const string DefaultApiUrl = "https://graph.microsoft.com/";

        /// <summary>
        /// Instance of Azure AD, for example public Azure or a Sovereign cloud (Azure China, Germany, US government, etc ...)
        /// </summary>
        public string Instance { get; set; } = DefaultInstance;

        /// <summary>
        /// Graph API endpoint, could be public Azure (default) or a Sovereign cloud (US government, etc ...)
        /// </summary>
        public string ApiUrl { get; set; } = DefaultApiUrl;

        /// <summary>
        /// The Tenant is:
        /// - either the tenant ID of the Azure AD tenant in which this application is registered (a guid)
        /// or a domain name associated with the tenant
        /// - or 'organizations' (for a multi-tenant application)
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Guid used by the application to uniquely identify itself to Azure AD
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// URL of the authority
        /// </summary>
        public string Authority => string.Format(CultureInfo.InvariantCulture, Instance, Tenant);

        /// <summary>
        /// Client secret (application password)
        /// </summary>
        /// <remarks>Daemon applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: this property)
        /// or a certificate previously shared with AzureAD during the application registration 
        /// (and identified by the CertificateName property below)
        /// </remarks> 
        public string ClientSecret { get; set; }

        /// <summary>
        /// Email id / object id of email account from azure portal
        /// </summary>
        /// <remarks>GraphApi supports both (User Principal Name(email)/ObjectId).
        /// Both fields are visible in azure portal users details page.</remarks>
        public string ReadEmailsFrom { get; set; }

        public string UsersUrl => $"{BaseUrl}/users/{ReadEmailsFrom}";

        public string MessagesRelativeUrl => $"/users/{ReadEmailsFrom}/messages";

        public string MessagesAbsoluteUrl => $"{BaseUrl}{MessagesRelativeUrl}";

        public string SendUrl => $"{UsersUrl}/sendMail";

        public string BatchUrl => $"{BaseUrl}/$batch";

        #region Private Properties

        private string CleanApiUrl => ApiUrl.TrimEnd('/');

        private string BaseUrl => $"{CleanApiUrl}/v1.0";

        #endregion
    }
}
