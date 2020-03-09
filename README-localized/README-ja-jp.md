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
# Microsoft Teams Graph API サンプル

このサンプルでは、以下の Teams Graph API を呼び出す例を示します。

* チームを取得する
* チャネルを取得する
* アプリを取得する
* チャネルを作成する
* メッセージを投稿する
* チームとグループを作成する
* チームをグループに追加する
* メンバーをチームに追加する
* チーム設定を変更する

## 関連するサンプル

* [Microsoft Teams を使った Contoso Airlines サンプル](https://github.com/microsoftgraph/contoso-airlines-teams-sample)
* [Microsoft Teams Graph API の Node サンプル](https://github.com/OfficeDev/microsoft-teams-sample-graph/tree/master/Node/SampleApp)

> Microsoft Teams のアプリ開発の詳細については、Microsoft Teams [開発者向けドキュメント](https://msdn.microsoft.com/en-us/microsoft-teams/index)をご覧ください。

## 前提条件

### 管理者特権のある O365 アカウント

このアプリを設定するには、グローバル管理者である必要があります。グローバル管理者のみが Group.ReadWrite.All などのアクセス許可を使用するアプリに同意できるためです。[Office 365 開発者プログラム](https://dev.office.com/devprogram)の開発者アカウントを作成して、自分自身のテスト テナントを作成することを検討してください。  

### 登録済みのアプリ

次の手順に従ってアプリを登録する必要があります。

1. 職場または学校のアカウントか、個人の Microsoft アカウントを使用して、[Azure portal](https://go.microsoft.com/fwlink/?linkid=2083908)にサインインします。
2. ご利用のアカウントで複数のテナントにアクセスできる場合は、右上でアカウントを選択し、ポータルのセッションを目的の Azure AD テナントに設定します。
3. [**新規登録**] を選択します。
4. [アプリケーションの登録ページ] が表示されたら、以下のアプリケーションの登録情報を入力します。
   * **名前** \- アプリのユーザーに表示されるわかりやすいアプリケーション名を入力します。
   * **サポートされているアカウントの種類** \- アプリケーションでサポートするアカウントを選択します。
   * **リダイレクト URI (オプション)** \- [**Web**] を選択し、[**リダイレクト URI**] に "http://localhost:55065/" を入力します。
5. 終了したら、[**登録**] を選択します。
6. Azure AD によりアプリに一意のアプリケーション (クライアント) ID が割り当てられ、アプリケーションの [概要] ページに移動します。アプリケーションにさらに機能を追加するには、ブランディング、証明書と秘密情報、API アクセス許可など、その他の構成オプションを選択できます。 

   アプリケーション ID をコピーします。これは、アプリの一意識別子です。
7. 左側のペインの [**管理**] で、[**証明書とシークレット**] をクリックします。[**クライアント シークレット**] で、[**新しいクライアント シークレット**] をクリックします。説明と有効期限を入力し、[**追加**] をクリックします。これにより、アプリケーションが ID を証明するために使用する秘密の文字列、またはアプリケーション パスワードが作成されます。  

   新しいシークレットの値をコピーします。
8. 左側のペインの [**管理**] で、 [**認証**] をクリックします。[**暗黙の付与**] で、[**アクセス トークン**] と [**ID トークン**] を確認します。認証時に、アクセス トークンを取得するためにアプリが使用できるサインイン情報 (id\_token) と成果物 (この場合は、認証コード) の両方をアプリで受信できるようになります。変更内容を保存します。
9. 左側のペインの [**管理**] で、[**API のアクセス許可**] をクリックして、[**アクセス許可の追加**] をクリックします。[**Microsoft Graph**] を選び、[**委任されたアクセス許可**] を選びます。"Group.ReadWrite.All" (すべてのグループの読み取りと書き込み) アクセス許可と "User.ReadWrite.All" (すべてのユーザーの完全なプロファイルの読み取りと書き込み) アクセス許可を追加します。[**アクセス許可の追加**] をもう一度クリックし、[**アプリケーションのアクセス許可**] をクリックします。"Group.ReadWrite.All" (すべてのグループの読み取りと書き込み) アクセス許可と "User.ReadWrite.All" (すべてのユーザーの完全なプロファイルの読み取りと書き込み) アクセス許可を追加します。

詳細については、各プロジェクトの Readme を参照してください。
    
### サンプル アプリのビルドと実行

1. サンプル ソリューションを Visual Studio で開きます。
2. 前のセクションを参照して アプリ ID と アプリ シークレットを取得します。
3. Web.config と同じ場所に Web.config.secrets という名前のファイルを作成し、アプリ ID とアプリ シークレットを追加します。

```
<?xml version="1.0" encoding="utf-8"?>
  <appSettings >
    <add key="ida:AppId" value="xxxxx"/>
    <add key="ida:AppSecret" value="xxxxx"/>
  </appSettings>
```

4. 登録済みアプリの "ida:RedirectUri" と同じ値で、Web アプリケーションの "Web サーバー" を更新します。 

* ソリューション エクスプローラーで、Web サーバーを指定する Web アプリケーション プロジェクトの名前を右クリックし、[プロパティ] をクリックします。
* [プロパティ] ウィンドウで、[Web] タブをクリックします。
* [サーバー] で、登録されているアプリの "ida: RedirectUri" と同じ値で "Project Url" を更新します。
* [仮想ディレクトリの作成] をクリックします。
* ファイルを保存します。

5. サンプルをビルドして実行します。

6. 自分のアカウントでサインインし、要求されたアクセス許可を付与します。

* アプリケーションを実行するには、適切な管理者権限 (Group.ReadWrite.All と User.ReadWrite.All) を持っている必要があります。

7. [チームを取得する]、[チャネルを取得する]、[チャネルを作成する]、[メッセージを投稿する] などの操作を選びます。

8. 応答情報は、ページの下部に表示されます。

## フィードバック

フィードバックをお寄せください。[フィードバックの送信方法はこちらです](https://msdn.microsoft.com/en-us/microsoft-teams/feedback)。

## Microsoft オープン ソース倫理規定

このプロジェクトでは、[Microsoft Open Source Code of Conduct (Microsoft オープン ソース倫理規定)](https://opensource.microsoft.com/codeofconduct/) が採用されています。詳細については、「[Code of Conduct の FAQ (倫理規定の FAQ)](https://opensource.microsoft.com/codeofconduct/faq/)」を参照してください。また、その他の質問やコメントがあれば、[opencode@microsoft.com](mailto:opencode@microsoft.com) までお問い合わせください。

## 投稿

プル要求を送信するプロセスの詳細については、「[投稿](contributing.md)」をお読みください。

## ライセンス

このプロジェクトは、MIT ライセンスの下でライセンスされています。詳細については、[ライセンス](LICENSE) ファイルを参照してください。

## 著作権

Copyright (c) 2018 Microsoft Corporation.All rights reserved.
