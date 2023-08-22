<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .page
        {
            background-position: center center;
            width: 970px; /* background-color: #fff;*/
            margin: 20px auto 0px auto;
            border: 5px;
            position: static;
            background-image: url('~/Imagenes/entradauach.png');
            background-repeat: no-repeat;
            background-attachment: scroll;
        }
        .style4
        {
            width: 965px;
            height: 630px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <center>
        <p>
            <img alt="" class="style4" src="Imagenes/entradauach.png" /></p>
    </center>
</asp:Content>
