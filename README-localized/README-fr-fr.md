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
# Exemples de l’API Microsoft Teams Graph

Cet exemple contient des exemples d’appels vers de nombreuses API Teams Graph, notamment :

* Obtenir mes équipes
* Obtenir des canaux
* Obtenir des applications
* Créer un canal
* Envoyer un message
* Créer une équipe et un groupe
* Ajouter une équipe au groupe
* Ajouter un membre à l’équipe
* Mettre à jour les paramètres d’une équipe

## Exemples connexes

* [Exemple de Contoso Airlines pour Microsoft Teams](https://github.com/microsoftgraph/contoso-airlines-teams-sample)
* [Version du nœud des exemples de l’API Microsoft Teams Graph](https://github.com/OfficeDev/microsoft-teams-sample-graph/tree/master/Node/SampleApp)

> Pour plus d’informations sur le développement d’applications pour Microsoft Teams, veuillez consulter la [documentation des développeurs](https://msdn.microsoft.com/en-us/microsoft-teams/index) de Microsoft Teams.\**

## Conditions préalables

### Compte O365 avec des privilèges d’administrateur

Pour configurer cette application, vous devez être administrateur global, car seuls les administrateurs globaux peuvent donner leur consentement pour des applications à l’aide d’autorisations telles que Group.ReadWrite.All. Envisagez de créer votre propre client de test en créant un compte de développeur avec notre [programme Office 365 Développeur](https://dev.office.com/devprogram).  

### Application inscrite

Vous devez enregistrer une application en procédant comme suit :

1. Connectez-vous au [portail Microsoft Azure](https://go.microsoft.com/fwlink/?linkid=2083908) à l’aide d’un compte professionnel ou scolaire, ou d’un compte Microsoft personnel.
2. Si votre compte vous donne accès à plusieurs locataires, sélectionnez votre compte en haut à droite et définissez votre session de portail sur le locataire Azure AD souhaité.
3. Sélectionnez **Nouvelle inscription**.
4. Lorsque la page Inscrire une application s’affiche, saisissez les informations d’inscription de votre application :
   * **Nom** : saisissez un nom d’application cohérent qui s’affichera pour les utilisateurs de l’application.
   * **Types de compte pris en charge** : sélectionnez les comptes à prendre en charge par l’application.
   * **URI de redirection (facultatif)** : sélectionnez **Web** et entrez 'http://localhost:55065/ » pour l’**URI de redirection**.
5. Lorsque vous avez terminé, sélectionnez **Inscrire**.
6. Azure AD attribue un ID d’application unique (client) à votre application, et vous êtes redirigé vers la page Vue d’ensemble de votre application. Pour ajouter des fonctionnalités supplémentaires à votre application, vous pouvez sélectionner d’autres options de configuration, dont la personnalisation, les certificats et les secrets, les autorisations API, et plus encore. 

   Copiez l’ID de l’application. Il s’agit de l’identificateur unique de votre application.
7. Sous **Gérer** dans le volet gauche, cliquez sur **Certificats et clés secrètes**. Sous **Clés secrètes client**, cliquez sur **Nouvelle clé secrète client**. Entrez une description et une date d’expiration, puis cliquez sur **Ajouter**. Cette opération crée une chaîne secrète ou un mot de passe utilisé par l’application pour prouver son identité.  

   Copiez la valeur de la nouvelle clé secrète.
8. Sous **Gérer** dans le volet gauche, cliquez sur **Authentification**. Sous **Octroi implicite**, vérifiez **Jetons d’accès** et **Jetons d’ID**. Lors de l’authentification, cela permet à l’application de recevoir les informations de connexion (id\_token) et les artefacts (dans ce cas, un code d’autorisation) qui servent à obtenir un jeton d’accès. Enregistrez vos modifications.
9. Sous **Gérer** dans le volet gauche, cliquez sur **Autorisations de l’API**, puis sur **Ajouter une nouvelle autorisation**. Sélectionnez **Microsoft Graph**, puis **Autorisations déléguées**. Ajoutez les autorisations 'Group.ReadWrite.All' (lire et écrire tous les groupes) et 'User.ReadWrite.All' (lire et écrire le profil complet de tous les utilisateurs). Cliquez de nouveau sur **Ajouter une nouvelle autorisation**, puis sur **Autorisations d’application**. Ajoutez les autorisations 'Group.ReadWrite.All' (lire et écrire tous les groupes) et 'User.ReadWrite.All' (lire et écrire le profil complet de tous les utilisateurs).

Pour plus d’informations, consultez les fichiers readme du projet en question.
    
### Créer et exécuter l’exemple d’application

1. Ouvrez l’exemple de solution dans Visual Studio.
2. Procurez-vous votre ID d’application et la clé secrète de l’application à partir de la section précédente.
3. Créez un fichier nommé Web.config.secrets (placez-le en regard de Web.config), puis ajoutez votre ID et votre clé secrète d’application :

```
<?xml version="1.0" encoding="utf-8"?>
  <appSettings >
    <add key="ida:AppId" value="xxxxx"/>
    <add key="ida:AppSecret" value="xxxxx"/>
  </appSettings>
```

4. Mettez à jour le 'serveur Web' de votre application Web avec l’option 'Ida:RedirectUri' de votre application inscrite. 

* Dans l’Explorateur de solutions, cliquez avec le bouton droit sur le nom du projet d’application Web pour lequel vous voulez spécifier un serveur Web, puis cliquez sur 'Propriétés'.
* Dans la fenêtre 'Propriétés', cliquez sur l’onglet 'Web'.
* Sous 'Serveurs', mettez à jour l’URL du projet avec l'option 'Ida:RedirectUri' de votre application inscrite.
* Cliquez sur 'Créer un répertoire virtuel'.
* Enregistrez le fichier.

5. Créez et exécutez l’exemple.

6. Connectez-vous à votre compte, puis accordez les autorisations demandées.

* Remarque : vous devez disposer des droits élevés appropriés pour exécuter l’application (Group.ReadWrite.All et User.ReadWrite.All)

7. Sélectionnez l’opération, par exemple, 'Obtenir mes équipes', 'Obtenir des canaux', 'Créer un canal' ou 'Envoyer un message'.

8. Les informations de réponse s’affichent en bas de la page.

## Commentaires

Vos commentaires sont les bienvenus ! [Pour nous envoyer les vôtres, procédez comme suit](https://msdn.microsoft.com/en-us/microsoft-teams/feedback).

## Code de conduite Open Source de Microsoft

Ce projet a adopté le [Code de conduite Open Source de Microsoft](https://opensource.microsoft.com/codeofconduct/). Pour en savoir plus, reportez-vous à la [FAQ relative au code de conduite](https://opensource.microsoft.com/codeofconduct/faq/) ou contactez [opencode@microsoft.com](mailto:opencode@microsoft.com) pour toute question ou tout commentaire.

## Contribution

Consultez [Contribution](contributing.md) pour en savoir plus sur le processus de soumission des demandes de réception.

## Licence

Ce projet a fait l’objet d’une licence en vertu de la licence MIT. Consultez le fichier de [Licence](LICENSE) pour plus de détails.

## Copyright

Copyright (c) 2018 Microsoft Corporation. Tous droits réservés.
