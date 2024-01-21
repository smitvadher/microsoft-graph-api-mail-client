# Graph API Mail Client App

The **Graph API Mail Client App** is an automation library for .NET that facilitates interactions with Microsoft Graph API for email management. It enables developers to perform various email-related tasks, such as retrieving emails, marking emails as read, replying to emails, sending new emails, and deleting emails based on specified criteria.

## Getting Started

Before using the Graph API Mail Client App, you need to register an application in the Azure portal and obtain the necessary credentials. Follow the steps below:

### 1. Register Your Application

1. Go to the [Azure portal](https://portal.azure.com/).
2. Navigate to the **Azure Active Directory** section.
3. Select **App registrations** and click on **New registration**.
4. Provide a meaningful name for your application, choose the appropriate supported account types, and set the redirect URI if required.
5. Click **Register** to create the application.

### 2. Generate Client Secret

1. In the application details page, go to the **Certificates & Secrets** tab.
2. Under the **Client secrets** section, click **New client secret**.
3. Enter a description, choose an expiration, and click **Add**. Ensure to copy and securely store the generated value, as it won't be visible again.

### 3. Configure Permissions

Assign the necessary permissions to your application based on the operations you intend to perform with the Graph API Mail Client App. Commonly required permissions include:

- **Mail.Read**: Required for retrieving emails.
- **Mail.ReadWrite**: Required for marking emails as read, replying to emails, and deleting emails.
- **Mail.Send**: Required for sending new emails.
- **Mail.ReadBasic.All**: Provides read access to a user's basic email. Note: Use cautiously due to broad access.

Navigate to the application details page in the Azure portal, go to the **API permissions** tab, and add the required permissions.

[NOTE](https://learn.microsoft.com/en-us/entra/identity-platform/scenario-daemon-acquire-token?source=recommendations&tabs=idweb#did-you-forget-to-provide-admin-consent-daemon-apps-need-it "Did you forget to provide admin consent? Daemon apps need it!"): If you get an Insufficient privileges to complete the operation error when you call the API, the tenant administrator needs to grant permissions to the application. For guidance on how to grant admin consent for your application, see step 4 in [Quickstart: Acquire a token and call Microsoft Graph in a .NET Core console app](https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-console-app-netcore-acquire-token#step-4-admin-consent).

## Configuration

To use the Graph API Mail Client App, configure the `GraphApiConfig` class in the `appsettings.json` file. The configuration includes the following parameters:

- **Instance**: The Azure AD instance URL, typically set to "https://login.microsoftonline.com/{0}".
  
- **ApiUrl**: The Graph API endpoint URL, defaulting to "https://graph.microsoft.com/".

- **Tenant**: The tenant ID or domain name associated with the Azure AD tenant. It can also be set to 'organizations' for a multi-tenant application.

- **ClientId**: The unique identifier for the application in Azure AD.

- **Authority**: The URL of the authority, constructed using the Instance and Tenant.

- **ClientSecret**: The client secret (application password) used for authentication.

- **ReadEmailsFrom**: The email ID or object ID of the email account from the Azure portal.

For example:

```json
{
  "GraphApiConfig": {
    "Instance": "https://login.microsoftonline.com/{0}",
    "ApiUrl": "https://graph.microsoft.com/",
    "Tenant": "your-tenant-id",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "ReadEmailsFrom": "readfrom@example.com"
  }
}
```

## Usage
See the `Program.cs` file for usage examples and integrate the Graph API Mail Client App into your .NET project for seamless email automation with Microsoft Graph API.