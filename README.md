# Microsoft Teams Graph API Samples
Thank you for your interest in the Microsoft Teams developer platform.  This repo contains sample apps that use the Microsoft Teams APIs in Microsoft Graph.


## Sample highlights 

This sample shows how you can use the new beta Microsoft Teams Graph APIs to:
* Query all teams
* Query channels for a team
* Send a message to a channel

The sample is provided in two flavors:
* [Node](https://github.com/OfficeDev/microsoft-teams-sample-graph/tree/master/Node)
* [C#](https://github.com/OfficeDev/microsoft-teams-sample-graph/tree/master/CSharp)

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
9. Under 'Microsoft Graph Permissions', Add 'Group.ReadWrite.All' and 'User.ReadWrite.All' as Delegated and Application Permissions.
10. Choose Save.

See the individual project readmes for more information.
    
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
Copyright (c) 2017 Microsoft Corporation. All rights reserved.

