<%@ Page Language="C#" Theme="Alitas" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Bienvenida.aspx.cs" Inherits="Account_Bienvenida" Title="Bienvenido" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style2
        {
            height: 23px;
        }
        .style3
        {
            width: 572px;
            height: 432px;
        }
    </style>
    <script runat="server">   
        public void Page_PreInit()
        {
            if (Session["theme"] == null)
            {
                this.Theme = "Alitas";
            }
            else
            {
                this.Theme = Convert.ToString(Session["theme"]);
            }
            // Sets the Theme for the page.
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1"  Runat="Server">
<link href="Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
 <div style="width: 935px; height: 500px;">
    <center style="width: 940px">
        <div class="title">
            <center>
                <h2>
                    <asp:Image SkinID="imagenEntrada" runat="server" />
                </h2>
            </center>
        </div>
    </center>
    </div>
</asp:Content>
