<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="PAXRecepcionProveedores.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        <asp:Label ID="lblInicioSesion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblInicioSesion %>" />
    </h2>
    <p>
        <%--<asp:Label ID="lblInicioSesion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblInicioSesion %>" />
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">Register</asp:HyperLink> if you don't have an account.--%>
    </p>
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
        <LayoutTemplate>
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="LoginUserValidationGroup"/>
            <div class="accountInfo">
                <fieldset class="login">
                    <legend><asp:Literal ID="ltInfoCuenta"  runat="server" Text ="<%$ Resources:resCorpusCFDIEs, lblInfoCuenta %>"></asp:Literal></legend>
                    <p>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text ="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                        <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                             CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>"
                              ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>"
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text ="<%$ Resources:resCorpusCFDIEs,lblContrasenia %>"></asp:Label>
                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                             CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida  %>"
                              ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida  %>" 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                   <%-- <p>
                        <asp:CheckBox ID="RememberMe" runat="server"/>
                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Keep me logged in</asp:Label>
                    </p>--%>
                </fieldset>
                <p class="submitButton">
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="<%$ Resources:resCorpusCFDIEs,lblEntrar %>" ValidationGroup="LoginUserValidationGroup"/>
                </p>
            </div>
        </LayoutTemplate>
    </asp:Login>
</asp:Content>
