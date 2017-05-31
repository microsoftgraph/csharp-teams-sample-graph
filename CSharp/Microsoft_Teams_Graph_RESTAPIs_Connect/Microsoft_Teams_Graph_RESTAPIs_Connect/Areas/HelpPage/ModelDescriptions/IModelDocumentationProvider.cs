using System;
using System.Reflection;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}