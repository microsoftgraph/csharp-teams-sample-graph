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
# Ejemplos de la API de Microsoft Teams Graph

Este ejemplo tiene llamadas a muchas de las API de Teams Graph, entre las que se incluyen:

* Obtener mis equipos
* Obtener canales
* Obtener aplicaciones
* Crear canales
* Publicar mensaje
* Crear equipo y grupo
* Agrear equipo al grupo
* Agregar miembro al equipo
* Actualizar la configuración del equipo

## Ejemplos relacionados

* [Ejemplo de Contoso Airlines para Microsoft Teams](https://github.com/microsoftgraph/contoso-airlines-teams-sample)
* [Versión para Node de los ejemplos de la API de Microsoft Teams Graph](https://github.com/OfficeDev/microsoft-teams-sample-graph/tree/master/Node/SampleApp)

> Para obtener más información sobre cómo desarrollar aplicaciones para Microsoft Teams, consulte la [documentación para desarrolladores](https://msdn.microsoft.com/en-us/microsoft-teams/index).\**

## Requisitos previos

### Cuenta de O365 con privilegios de administrador

Para configurar esta aplicación, debe ser un administrador global, porque solo ellos pueden aceptar las aplicaciones que usan permisos como Group.ReadWrite.All. Considere la posibilidad de crear su propio espacio de prueba al crear una cuenta de desarrollador mediante nuestro [Programa de desarrolladores de Office 365](https://dev.office.com/devprogram).  

### Aplicación registrada

Tendrá que registrar una aplicación a través del proceso siguiente:

1. Inicie sesión en [Microsoft Azure Portal](https://go.microsoft.com/fwlink/?linkid=2083908) con una cuenta personal, profesional o educativa de Microsoft.
2. Si la cuenta le proporciona acceso a más de un inquilino, haga clic en la cuenta en la esquina superior derecha y establezca la sesión del portal en el inquilino de Azure AD que desee.
3. Haga clic en **Nuevo registro**.
4. Cuando aparezca la página Registrar una aplicación, escriba la información de registro:
   * **Nombre**: escriba un nombre significativo que se mostrará a los usuarios de la aplicación.
   * **Tipos de cuenta compatibles**: seleccione qué cuentas desea que la aplicación admita.
   * **URI de redirección (opcional)**: seleccione **Web** y escriba 'http://localhost:55065/' como valor de **URI de redirección**.
5. Cuando termine, seleccione **Registrar**.
6. Azure AD le asigna un id. de aplicación (cliente) único a la aplicación y lo lleva a la página de información general de la misma. Para agregar funcionalidades adicionales a la aplicación, puede seleccionar otras opciones de configuración, como personalización de marca, certificados y secretos, permisos de API y mucho más. 

   Copie el Id. de aplicación. Este es el identificador único de su aplicación.
7. En **Administrar** en el panel izquierdo, haga clic en **Certificados y secretos**. En **Secretos de cliente**, haga clic en **Nuevo secreto de cliente**. Escriba una descripción y una fecha de expiración y, después, haga clic en **Agregar**. Se crea una cadena secreta o una contraseña que la aplicación usa para demostrar su identidad.  

   Copie el valor del nuevo secreto.
8. En **Administrar** en el panel izquierdo, haga clic en **Autenticación**. En **Concesión implícita**, marque **Tokens de acceso** y **Tokens de id.**. Durante la autenticación, esto permite que la aplicación reciba la información de inicio de sesión (id\_token) y artefactos (en este caso, un código de autorización) que la aplicación puede usar para obtener un token de acceso. Guarde los cambios.
9. En **Administrar** en el panel izquierdo, haga clic en **Permisos de API** y luego en **Agregar un permiso**. Seleccione **Microsoft Graph** y luego **Permisos delegados**. Agregue los permisos "Group.ReadWrite.All (Read and write all groups)" y "User.ReadWrite.All (Read and write all users' full profile)". Vuelva a hacer clic en **Agregar un permiso** y luego en **Permisos de la aplicación**. Agregue los permisos "Group.ReadWrite.All (Read and write all groups)" y "User.ReadWrite.All (Read and write all users' full profile)".

Para obtener más información, consulte los archivos Léame de los proyectos individuales.
    
### Compilar y ejecutar el ejemplo

1. Abra la solución de ejemplo en Visual Studio.
2. Obtenga el id. y el secreto de la aplicación de la sección anterior.
3. Cree un archivo con el nombre Web.config.secrets (colóquelo al lado de Web.config) y agregue el id. y el secreto de la aplicación:

```
<?xml version="1.0" encoding="utf-8"?>
  <appSettings >
    <add key="ida:AppId" value="xxxxx"/>
    <add key="ida:AppSecret" value="xxxxx"/>
  </appSettings>
```

4. Actualice el "Servidor web" de la aplicación web con el "ida:RedirectUri" de la aplicación registrada. 

* En el Explorador de soluciones, haga clic con el botón secundario en el nombre del proyecto de aplicación web para el que desea especificar un servidor web y, a continuación, haga clic en "Propiedades".
* En la ventana "Propiedades", haga clic en la pestaña "Web".
* En "Servidores", actualice la "Dirección URL del proyecto" con el "ida:RedirectUri" de la aplicación registrada.
* Haga clic en "Crear directorio virtual".
* Guarde el archivo.

5. Compile y ejecute el ejemplo.

6. Inicie sesión con su cuenta y conceda los permisos solicitados.

* Tenga en cuenta que deberá tener los permisos apropiados para ejecutar la aplicación (Group.ReadWrite.All y User.ReadWrite.All).

7. Elija una operación, como "Obtener mis equipos", "Obtener canales", "Crear canal" o "Publicar mensaje".

8. La información de la respuesta se muestra en la parte inferior de la página.

## Comentarios

Agradecemos sus comentarios. [Siga este procedimiento para enviarnos sus comentarios](https://msdn.microsoft.com/en-us/microsoft-teams/feedback).

## Código de conducta de código abierto de Microsoft

Este proyecto ha adoptado el [Código de conducta de código abierto de Microsoft](https://opensource.microsoft.com/codeofconduct/). Para obtener más información, vea [Preguntas frecuentes sobre el código de conducta](https://opensource.microsoft.com/codeofconduct/faq/) o póngase en contacto con [opencode@microsoft.com](mailto:opencode@microsoft.com) si tiene otras preguntas o comentarios.

## Contribuciones

Lea [Contribuciones](contributing.md) para obtener más información sobre nuestro código de conducta y sobre el proceso de envío de solicitudes de incorporación de cambios.

## Licencia

Este proyecto está publicado bajo la licencia MIT. Consulte el archivo [Licencia](LICENSE) para obtener más información.

## Derechos de autor

Copyright (c) 2018 Microsoft Corporation. Todos los derechos reservados.
