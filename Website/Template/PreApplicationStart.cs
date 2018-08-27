using Microsoft.Owin;
using Owin;
using System.Web.WebPages.Razor;

public class PreApplicationStart
{
    public static void InitializeApplication()
    {
        WebCodeRazorHost.AddGlobalImport("Website.ContentFileHelper");
    }
}