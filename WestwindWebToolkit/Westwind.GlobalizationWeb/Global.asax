<%@ Application Language="C#" %>
<%@ Import Namespace="Westwind.Globalization" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
    }
   
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown        
        
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        // *** Auto switch locale to the browser language and assign $ as currency symbol
        Westwind.Utilities.WebUtils.SetUserLocale("$",true);
    }

public override string GetVaryByCustomString(HttpContext Context, string Custom)
{    
    if (Custom == "Culture")
    {
#if false
        return Context.Request.UserLanguages[0];
        
        // *** NOTE: THIS DOESN'T WORK: It's not set yet!!!
        //return System.Globalization.CultureInfo.CurrentCulture.Name;
        
#else       
        // *** Figure out the supported language from the
        // *** UserLanguage[]                 
        string Lang = GetSupportedLanguage("en,fr,de","en");
        return Lang;
#endif             
    }
    if (Custom == "GZip")
    {
        return WebUtils.IsGZipSupported().ToString();
    }

    return base.GetVaryByCustomString(Context, Custom);
}


    /// <summary>
    /// Checks user languages for one of the supported languages
    /// </summary>
    /// <param name="SupportedLanguages">Two/three letter language codes separated by commas</param>
    /// <returns></returns>
private string GetSupportedLanguage(string SupportedLanguages,string DefaultLanguage)
{
    SupportedLanguages += ",";
    foreach (string Lang in HttpContext.Current.Request.UserLanguages)
    {               
        string[] LangKeys = Lang.Split('-');
        if (LangKeys.Length > 0)
        {
            string SelectedLanguage = LangKeys[0].ToLower();
            
            if (SupportedLanguages.Contains(SelectedLanguage + ","))
                return SelectedLanguage.ToLower();              
        }

    }
    
    // *** Return English as the 
    return DefaultLanguage.ToLower();
}
       
</script>
