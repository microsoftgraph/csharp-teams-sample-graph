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
# Microsoft Teams Graph API 示例

此示例含有调用许多 Teams Graph API 的例，包括：

* 获取我的团队
* 获取频道
* 获取应用程序
* 创建频道
* 发布消息
* 创建团队和组
* 添加团队至组
* 添加成员至团队
* 更新团队设置

## 相关示例

* [Microsoft Teams Contoso 航空示例](https://github.com/microsoftgraph/contoso-airlines-teams-sample)
* [Microsoft Teams Graph API 节点版本示例](https://github.com/OfficeDev/microsoft-teams-sample-graph/tree/master/Node/SampleApp)

> 有关 Microsoft Teams 开发应用程序的详细信息，请参阅 Microsoft Teams [开发人员文档](https://msdn.microsoft.com/en-us/microsoft-teams/index)。\**

## 先决条件

### 具有管理员权限的 O365 账户

如果要设置此应用，用户需要是全局管理员，因为只有全局管理员才能许可应用程序使用 Group.ReadWrite.All 等权限。考虑通过使用 [Office 365 开发人员计划](https://dev.office.com/devprogram)创建开发人员账户来创建自己的测试租户。  

### 已注册的应用

需要通过下列流程注册应用程序：

1. 使用工作/学校帐户或 Microsoft 个人帐户登录到 [Azure 门户](https://go.microsoft.com/fwlink/?linkid=2083908)。
2. 如果你的帐户有权访问多个租户，请在右上角选择该帐户，并将门户会话设置为所需的 Azure AD 租户。
3. 选择“**新注册**”。
4. 出现“注册应用程序页”后，输入应用程序的注册信息：
   * **名称** \- 输入一个会显示给应用用户的有意义的应用程序名称。
   * **支持的帐户类型** \- 选择希望应用程序支持的具体帐户。
   * **重定向 URI （可选）** \- 选择 **Web**并为**重定向 URI** 输入 'http://localhost:55065/'。
5. 完成后，选择“**注册**”。
6. Azure AD 会将唯一的应用程序（客户端）ID 分配给应用，同时你会转到应用程序的“概览”页。若要向应用程序添加其他功能，可选择品牌、证书和机密、API 权限等其他配置选项。 

   复制应用程序 ID。这是应用的唯一标识符。
7. 在左窗格的“**管理**”下，单击“**证书和密码**”。在“**客户端密码**”下，单击“**新建客户端密码**”。输入说明和到期日期，然后单击“**添加**”。此操作将创建密码字符串，或应用密码，用于应用程序验证其身份。  

   从新密码复制数值。
8. 在左窗格的“**管理**”下，单击“**身份验证**”。在“**隐式授权**”下，选中“**访问令牌**”和“**ID 令牌**”。在身份验证过程中，这可使应用同时接收登录信息 (id\_token) 以及应用可用来获取访问令牌的项目（在这种情况下，项目为授权代码）。保存所做的更改。
9. 在左窗格的“**管理**”下，单击“**API 权限**”， 然后“**添加新权限**”。选择“**Microsoft Graph**”，然后选择“**委派权限**”。添加 “Group.ReadWrite.All” （读取和写入所有组）和“User.ReadWrite.Al”（读取和写入全部用户的完整配置文件）权限。再次单击“**添加新权限**”，然后单击“**应用程序权限**”。添加 “Group.ReadWrite.All” （读取和写入所有组）和“User.ReadWrite.Al”（读取和写入全部用户的完整配置文件）权限。

查看各项目自述文件了解更多信息。
    
### 生成并运行示例应用

1. 在 Visual Studio 中打开示例解决方案。
2. 从上一节获取应用 ID 和应用密码
3. 创建名为 Web.config 的文件（将其放在 web.config 旁），并添加到 应用 ID 和应用密码中：

```
<?xml version="1.0" encoding="utf-8"?>
  <appSettings >
    <add key="ida:AppId" value="xxxxx"/>
    <add key="ida:AppSecret" value="xxxxx"/>
  </appSettings>
```

4. 使用已注册应用的“ida:RedirectUri”更新 Web 应用程序的 “Web 服务器” 

* 在解决方案资源管理器中，右击要为其指定 Web 服务器的 Web 应用程序项目的名称，然后单击“属性”。
* 在“属性”窗口中单击“Web”选项卡。
* 在“服务器”下，使用已注册应用程序的“ida:RedirectUri”更新“项目 Url”。
* 单击“创建虚拟目录”
* 保存文件。

5. 生成和运行示例。

6. 使用账户登录，并授予请求的权限。

* 注意：需要具有相应的提升的权限才能运行应用（Group.ReadWrite.All 和 User.ReadWrite.All）

7. 选择操作，如“获取我的团队”、“获取频道”、“创建频道”或“发布消息”。

8. 响应信息将在页面底部显示。

## 反馈

欢迎提供反馈信息！[下面介绍如何向我们发送反馈](https://msdn.microsoft.com/en-us/microsoft-teams/feedback)。

## Microsoft 开放源代码行为准则

此项目已采用 [Microsoft 开放源代码行为准则](https://opensource.microsoft.com/codeofconduct/)。有关详细信息，请参阅[行为准则 FAQ](https://opensource.microsoft.com/codeofconduct/faq/)。如有其他任何问题或意见，也可联系 [opencode@microsoft.com](mailto:opencode@microsoft.com)。

## 参与

请阅读“[参与](contributing.md)”，详细了解我们提交拉取请求的流程。

## 许可证

此项目使用 MIT 许可证授权 - 参见“[许可证](LICENSE)”文件了解详细信息。

## 版权信息

版权所有 (c) 2018 Microsoft Corporation。保留所有权利。
