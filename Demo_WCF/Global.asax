<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        //Application["UsersLoggedIn"] = new System.Collections.Generic.List<string>();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
        //string userLoggedIn = string.Empty;

        //try
        //{
        //    if (Session["UserLoggedIn"] != null)
        //    {
        //        if (Session["UserLoggedIn"] != string.Empty)
        //        {
        //            if (Session["UserLoggedIn"] == (string)Session["UserLoggedIn"])
        //            {
        //                userLoggedIn = (string)Session["UserLoggedIn"];
        //            }

        //        }
        //    }

        //    //string userLoggedIn = Session["UserLoggedIn"] == null ? string.Empty ? (string)Session["UserLoggedIn"];

        //    if (userLoggedIn.Length > 0)
        //    {
        //        System.Collections.Generic.List<string> d = Application["UsersLoggedIn"] as System.Collections.Generic.List<string>;
        //        if (d != null)
        //        {
        //            lock (d)
        //            {
        //                d.Remove(userLoggedIn);
        //            }
        //        }
        //    }
        //}
        //catch(Exception)
        //{
        //}
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
        //// Code that runs when a session ends. 
        //// Note: The Session_End event is raised only when the sessionstate mode
        //// is set to InProc in the Web.config file. If session mode is set to StateServer 
        //// or SQLServer, the event is not raised.
        
        //string userLoggedIn = string.Empty;

        //try
        //{


        //    if (Session["UserLoggedIn"] != null)
        //    {
        //        if (Session["UserLoggedIn"] != string.Empty)
        //        {
        //            if (Session["UserLoggedIn"] == (string)Session["UserLoggedIn"])
        //            {
        //                userLoggedIn = (string)Session["UserLoggedIn"];
        //            }

        //        }
        //    }

        //    //string userLoggedIn = Session["UserLoggedIn"] == null ? string.Empty ? (string)Session["UserLoggedIn"];

        //    if (userLoggedIn.Length > 0)
        //    {
        //        System.Collections.Generic.List<string> d = Application["UsersLoggedIn"] as System.Collections.Generic.List<string>;
        //        if (d != null)
        //        {
        //            lock (d)
        //            {
        //                d.Remove(userLoggedIn);
        //            }
        //        }
        //    }
        //}
        //catch (Exception)
        //{
        //}   
    }
       
</script>
