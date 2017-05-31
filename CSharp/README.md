# C# sample app for Microsoft Teams APIs in Microsoft Graph

## Register the application:
1. Sign into the [App Registration Portal](https://apps.dev.microsoft.com) using either your personal or work or school account.
2. Choose 'Add an app'.
3. Enter a name for the app, and choose 'Create application'.
4. The registration page displays, listing the properties of your app.
   * Copy the Application Id. This is the unique identifier for your app.
5. Under 'Application Secrets', choose 'Generate New Password'.
   * Copy the password from the 'New password generated' dialog.
6. Under 'Platforms', choose 'Add platform'.
7. Choose 'Web'.
8. Make sure 'Allow Implicit Flow' check box is selected, and enter 'Redirect URI' e.g., https://localhost:55065/.
   * The 'Allow Implicit Flow' option enables the hybrid flow. During authentication, this enables the app to receive both sign-in info (the id_token) and artifacts (in this case, an authorization code) that the app can use to obtain an access token.
9. Under 'Microsoft Graph Permissions', Add 'Group.ReadWrite.All' and 'User.ReadWrite.All' as Delegated and Application Permissions.
10. Choose Save.

## Build and run the sample app
1.  Open the sample solution in Visual Studio.
2.  In the Web.config file in the root directory, replace the 'ida:AppId', 'ida:AppSecret' and 'ida:RedirectUri' placeholder values with the values of your registered app.
3.  Update the 'Web Server' of your web application with the 'ida:RedirectUri' of your registered app 
    * In Solution Explorer, right-click the name of the Web application project for which you want to specify a Web server, and then click 'Properties'.
    * In the 'Properties' window, click the 'Web' tab.
    * Under 'Servers', update the 'Project Url' with the 'ida:RedirectUri' of your registered app.
    * Click 'Create Virtual Directory'
    * Save the file.
4. Build and run the sample.
5. Sign in with your personal account (MSA) or your work or school account, and grant the requested permissions.
6. Choose operation, such as 'Get My Teams', 'Get Channels', 'Create Channel' or 'Post Message'.
7. Response information is displayed at the bottom of the page.
