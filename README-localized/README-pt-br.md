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
# Exemplos de API do Graph no Microsoft Teams

Este exemplo tem chamadas de exemplo para várias APIs do Graph Teams, incluindo:

* Obter minhas equipes	
* Obter canais
* Obter aplicativos
* Criar canal
* Postar uma mensagem
* Criar equipe ou grupo
* Adicionar equipe ao grupo
* Adicionar membro à equipe
* Alterar configurações da equipe

## Exemplos afins

* [Exemplo da companhia aérea Contoso para Microsoft Teams](https://github.com/microsoftgraph/contoso-airlines-teams-sample)
* [ Exemplos da versão node da API do Graph no Microsoft Teams](https://github.com/OfficeDev/microsoft-teams-sample-graph/tree/master/Node/SampleApp)

> Para mais informações sobre o desenvolvimento de aplicativos para o Microsoft Teams, confira a[ documentação para desenvolvedores](https://msdn.microsoft.com/en-us/microsoft-teams/index)do Microsoft Teams.\**

## Pré-requisitos

### Conta do O365 com privilégios de administrador

Para configurar esse aplicativo, você precisar ser um administrador global, pois somente administradores podem dar consentimento para aplicativos que usem permissões do tipo Group.ReadWrite.All. Considere a criação de seu próprio locatário de teste por meio da criação de uma conta de desenvolvedor com o nosso [programa para Desenvolvedores do Office 365](https://dev.office.com/devprogram).  

### Aplicativo registrado

Será necessário registrá-lo seguindo os procedimentos abaixo:

1. Entre no [portal do Azure](https://go.microsoft.com/fwlink/?linkid=2083908)usando uma conta corporativa, de estudante ou uma conta Microsoft pessoal.
2. Se sua conta fornecer acesso a mais de um locatário, selecione sua conta no canto superior direito e defina sua sessão do portal para o locatário desejado do Azure AD.
3. Selecione **Novo registro**.
4. Quando a página Registrar um aplicativo for exibida, insira as informações de registro do aplicativo:
   * **Nome** \- Insira um nome de aplicativo relevante o qual será exibido para os um do aplicativo.
   * **Tipos de contas com suporte** \- Selecione quais as contas para as quais você gostaria que o seu aplicativo oferecesse suporte.
   * **URI de redirecionamento (opcional)** \- Selecione **Web** e insira 'http://localhost:55065/' como **URI de redirecionamento**.
5. Quando terminar, selecione **Registrar**.
6. O Azure AD atribui uma ID exclusiva do aplicativo (cliente) para seu aplicativo, e você é redirecionado para a página Visão geral desse aplicativo. Para adicionar mais recursos ao seu aplicativo, você pode selecionar outras opções de configuração, incluindo identidade visual, certificados e segredos, permissões de API e muito mais. 

   Copie a ID do Aplicativo. Esse é o identificador exclusivo do aplicativo.
7. Em**Gerenciar** no painel à esquerda, clique em **Certificados e segredos**. Em **Segredos do cliente**, clique em **Novo segredo do cliente**. Insira uma descrição e uma expiração, em seguida, clique em **Adicionar**. Isso cria uma cadeia de caracteres secreta ou uma senha do aplicativo, usado por ele para provar sua identidade.  

   Copie o valor do novo segredo.
8. Em**Gerenciar**, no painel à esquerda, clique em **Autenticação**. Em **Concessão implícita**, marque **Tokens de acesso** e **Tokens de ID**. Durante a autenticação, isso permite que o aplicativo receba informações de entrada (o id\_token) e artefatos (neste caso, um código de autorização) que o aplicativo pode usar para obter um token de acesso. Salve suas alterações.
9. Em **Gerenciar**, no painel à esquerda, clique em ** Permissões da API** e, em seguida, em **Adicione uma nova permissão**. Selecione **Microsoft Graph** e **Permissões delegadas**. Adicione as permissões 'Group.ReadWrite.All' (ler e gravar todos os grupos) e 'User.ReadWrite.All (ler e gravar todos os perfis de todos os usuários). Clique em **Adicionar uma nova permissão** novamente e, em seguida, clique em **Permissões de aplicativo**. Adicione as permissões 'Group.ReadWrite.All' (ler e gravar todos os grupos) e 'User.ReadWrite.All (ler e gravar todos os perfis de todos os usuários).

Confira o arquivo readme individual do projeto para saber mais.
    
### Crie e execute o aplicativo de exemplo

1. Abra a solução de exemplo no Visual Studio.
2. Obter o ID e o segredo do aplicativo definidos na seção anterior
3. Crie um arquivo chamando Web.config.secrets (coloque-o próximo a Web.config) e adicione o AppID e o segredo do aplicativo:

```
<?xml version="1.0" encoding="utf-8"?>
  <appSettings >
    <add key="ida:AppId" value="xxxxx"/>
    <add key="ida:AppSecret" value="xxxxx"/>
  </appSettings>
```

4. Atualize o "servidor Web" do seu aplicativo Web com a 'ida:RedirectUri' do seu aplicativo registrado 

* No Gerenciador de soluções, clique com o botão direito do mouse no nome do projeto do aplicativo Web para o qual você deseja especificar um servidor Web e, em seguida, clique em 'Propriedades'.
* Na janela 'Propriedades', clique na guia 'Web'.
* Em 'Servidores', atualize 'Project Url' com o 'ida:RedirectUri' do seu aplicativo registrado.
* Clique em 'Criar diretório virtual'
* Salve o arquivo.

5. Crie e execute o aplicativo de exemplo.

6. Entre com sua conta e conceda as permissões solicitadas.

* Observe que é preciso ter direitos elevados para executar o aplicativo (permissões de Group.ReadWrite.All e de User.ReadWrite.All)

7. Escolha uma operação, como 'Obter minha equipes', 'Obter canais', 'Criar canal' ou 'postar mensagem'.

8. As informações de resposta são exibidas na parte inferior da página.

## Avaliações

Seus comentários serão muito bem-vindos. [Veja aqui como nos enviar comentários](https://msdn.microsoft.com/en-us/microsoft-teams/feedback).

## Código de Conduta de Código Aberto da Microsoft

Este projeto adotou o [Código de Conduta de Código Aberto da Microsoft](https://opensource.microsoft.com/codeofconduct/).  Para saber mais, confira as [Perguntas frequentes sobre o Código de Conduta](https://opensource.microsoft.com/codeofconduct/faq/) ou entre em contato pelo [opencode@microsoft.com](mailto:opencode@microsoft.com) se tiver outras dúvidas ou comentários.

## Colaboração

Leia [Colaboração](contributing.md) para obter detalhes sobre o nosso código de conduta e o processo de envio para nós de solicitações pull.

## Licença

Este projeto está licenciado sob a Licença MIT, confira o arquivo [Licença](LICENSE) para obter detalhes.

## Direitos autorais

Copyright (c) 2018 Microsoft Corporation. Todos os direitos reservados.
