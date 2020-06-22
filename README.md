---
page_type: sample
products:
- office-teams
- ms-graph
languages:
- csharp
extensions:
  contentType: samples
  technologies:
  - Microsoft Graph 
  services:
  - Microsoft Teams
  createdDate: 5/30/2017 6:00:04 PM
---
# Microsoft Teams Graph API Samples

**This project is being archived and replaced with the [Microsoft Graph snippets sample for ASP.NET Core 3.1](https://github.com/microsoftgraph/aspnet-snippets-sample). As part of the archival process, we're closing all open issues and pull requests.**

**You can continue to use this sample "as-is", but it won't be maintained moving forward. We apologize for any inconvenience.**

This sample has example calls to many of the Teams Graph APIs, including:

* Get My Teams
* Get Channels
* Get Apps
* Create Channel
* Post Message
* Create Team and Group
* Add Team to Group
* Add Member to team
* Update Team settings

## Related samples

* [Contoso Airlines sample for Microsoft Teams](https://github.com/microsoftgraph/contoso-airlines-teams-sample)
* [Node version of the Microsoft Teams Graph API Samples](https://github.com/OfficeDev/microsoft-teams-sample-graph/tree/master/Node/SampleApp)

> For more information on developing apps for Microsoft Teams, please review the Microsoft Teams [developer documentation](https://msdn.microsoft.com/en-us/microsoft-teams/index).**

## Prerequisites

### O365 Account with Admin privileges

To set this app up, you'll need to be a global admin, because only global admins can consent to apps using permissions such as Group.ReadWrite.All. Consider creating your own test tenant by creating a developer account with our [Office 365 Developer program](https://dev.office.com/devprogram).  

### Registered app

You'll need to register an app through the following process:

1. Sign in to the [Azure portal](https://go.microsoft.com/fwlink/?linkid=2083908) using either a work or school account or a personal Microsoft account.
2. If your account gives you access to more than one tenant, select your account in the top right corner, and set your portal session to the Azure AD tenant that you want.
3. Select **New registration**.
4. When the Register an application page appears, enter your application's registration information:
   * **Name** - Enter a meaningful application name that will be displayed to users of the app.
   * **Supported account types** - Select which accounts you would like your application to support.
   * **Redirect URI (optional)** - Select **Web** and enter 'http://localhost:55065/' for the **Redirect URI**.
5. When finished, select **Register**.
6. Azure AD assigns a unique application (client) ID to your app, and you're taken to your application's Overview page. To add additional capabilities to your application, you can select other configuration options including branding, certificates and secrets, API permissions, and more. 

   Copy the Application Id. This is the unique identifier for your app.
7. Under **Manage** on the left-hand pane, click **Certificates & secrets**.  Under **Client secrets**, click **New client secret**.  Enter a description and an expiration, then click **Add**.  This creates a secret string, or application password, which the application uses to prove it's identity.  

   Copy the value from the new secret.
8. Under **Manage** on the left-hand pane, click **Authentication**. Under **Implicit grant**, check **Access tokens** and **ID tokens**. During authentication, this enables the app to receive both sign-in info (the id_token) and artifacts (in this case, an authorization code) that the app can use to obtain an access token. Save your changes.
9. Under **Manage** on the left-hand pane, click **API permissions** and then **Add a new permission**. Select **Microsoft Graph** and then **Delegated permissions**. Add 'Group.ReadWrite.All' (Read and write all groups) and 'User.ReadWrite.All' (Read and write all users' full profile) permissions. Click **Add a new permission** again then **Application permissions**. Add 'Group.ReadWrite.All' (Read and write all groups) and 'User.ReadWrite.All' (Read and write all users' full profile) permissions.

See the individual project readmes for more information.
    
### Build and run the sample app

1. Open the sample solution in Visual Studio.
2. Get your appid & app secret from the previous section
3. Create a file named Web.config.secrets (put it next to Web.config), and add in your appid and app secret:

```
<?xml version="1.0" encoding="utf-8"?>
  <appSettings >
    <add key="ida:AppId" value="xxxxx"/>
    <add key="ida:AppSecret" value="xxxxx"/>
  </appSettings>
```

4. Update the 'Web Server' of your web application with the 'ida:RedirectUri' of your registered app 

* In Solution Explorer, right-click the name of the Web application project for which you want to specify a Web server, and then click 'Properties'.
* In the 'Properties' window, click the 'Web' tab.
* Under 'Servers', update the 'Project Url' with the 'ida:RedirectUri' of your registered app.
* Click 'Create Virtual Directory'
* Save the file.

5. Build and run the sample.

6. Sign in with your account, and grant the requested permissions.

* Note you'll need to have appropriate elevated rights to run the app (Group.ReadWrite.All and User.ReadWrite.All)

7. Choose operation, such as 'Get My Teams', 'Get Channels', 'Create Channel' or 'Post Message'.

8. Response information is displayed at the bottom of the page.

## Feedback

We welcome your feedback! [Here's how to send us yours](https://msdn.microsoft.com/en-us/microsoft-teams/feedback).

## Microsoft Open Source Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Contributing

Please read [Contributing](contributing.md) for details on the process for submitting pull requests to us.

## License

This project is licensed under the MIT License - see the [License](LICENSE) file for details.

## Copyright

Copyright (c) 2018 Microsoft Corporation. All rights reserved.
