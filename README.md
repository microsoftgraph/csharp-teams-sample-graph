# Microsoft Teams Graph API Samples

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

As the current Microsoft Teams Graph APIs are only accessible by a tenant admin, to run the app, you'll need to sign in with an account with admin privileges.  Note that in most companies, you might not have these rights, nor the ability to grant yourself these rights, therefore you might benefit from a developer account through our [Office 365 Developer program](https://dev.office.com/devprogram).  

### Registered app

You'll need to register an app through the following process:

1. Sign into the [App Registration Portal](https://apps.dev.microsoft.com) using your personal, work or school account.
2. Choose 'Add an app'.
3. Enter a name for the app, and choose 'Create application'.
4. The registration page displays, listing the properties of your app.
   * Copy the Application Id. This is the unique identifier for your app.
5. Under 'Application Secrets', choose 'Generate New Password'.
   * Copy the password from the 'New password generated' dialog.
6. Under 'Platforms', choose 'Add platform'.
7. Choose 'Web'.
8. Make sure 'Allow Implicit Flow' check box is selected, and enter 'Redirect URI' e.g., http://localhost:55065/.  See appropriate sample for more information on the specific port.
   * The 'Allow Implicit Flow' option enables the hybrid flow. During authentication, this enables the app to receive both sign-in info (the id_token) and artifacts (in this case, an authorization code) that the app can use to obtain an access token.
9. Under 'Microsoft Graph Permissions', Add 'Group.ReadWrite.All' (Read and write all groups) and 'User.ReadWrite.All' (Read and write all users' full profile) as Delegated and Application Permissions.
10. Choose Save.

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
