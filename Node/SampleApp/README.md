# Node.js sample app for Microsoft Teams APIs in Microsoft Graph

This sample app, written in Node.js, shows a web site calling the Microsoft Graph Team APIs.  

## Prerequisites
The minimum prerequisites to run this sample are:
* The latest update of Visual Studio. You can download the community version [here](http://www.visualstudio.com) for free.
* An Office 365 account with access to Microsoft Teams, with [sideloading enabled](https://msdn.microsoft.com/en-us/microsoft-teams/setup).
* An account with the [appropriate rights](../README.md) to register and run the samples.

## Register the application:
Please see the project's [Read Me file](../../README.md) for more details.  Note that for the Node sample, you should set your redirect URI to be: http://localhost:55065/login

## Build and run the sample app
1. In app.js, update the process.env variables with your app's ID and secret and 'https://localhost:55065' as the hostname.
2. Run “node app.js”.  
3. Go to http://localhost:55065/login in your browser.
4. Sign in with your account, and grant the requested permissions.
    * Note you'll need to have appropriate elevated rights to run the app (Group.ReadWrite.All and User.ReadWrite.All)
5. Choose operation, such as 'Get My Teams', 'Get Channels', 'Create Channel' or 'Post Message'.
6. Response information is displayed at the bottom of the page.

