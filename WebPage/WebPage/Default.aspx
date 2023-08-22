<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true"  validateRequest="false" CodeBehind="Default.aspx.cs" Inherits="PAXActividades.WebForm7" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-top:60px;" align="center">
    <h1>
        Bienvenido(a)<br />
        <asp:Label ID="lblUsuario" runat="server"></asp:Label>
    </h1>
    <img src="Imagenes/user.jpg" alt="usuario" width="99" height="156" />
    </div>
</asp:Content>
