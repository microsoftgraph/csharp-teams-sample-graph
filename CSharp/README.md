# C# sample app for Microsoft Teams APIs in Microsoft Graph

This sample app, written in C#, shows a web site calling the Microsoft Graph Team APIs.  

## Prerequisites
The minimum prerequisites to run this sample are:
* The latest update of Visual Studio. You can download the community version [here](http://www.visualstudio.com) for free.
* An Office 365 account with access to Microsoft Teams, with [sideloading enabled](https://msdn.microsoft.com/en-us/microsoft-teams/setup).
* An account with the [appropriate rights](./README.md) to register and run the samples.

## Register the application:
Please see the project's [Read Me file](./README.md) for more details.  

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
5. Sign in with your account, and grant the requested permissions.
    * Note you'll need to have appropriate elevated rights to run the app (Group.ReadWrite.All and User.ReadWrite.All)
6. Choose operation, such as 'Get My Teams', 'Get Channels', 'Create Channel' or 'Post Message'.
7. Response information is displayed at the bottom of the page.
