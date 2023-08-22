<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SolucionPruebas.Presentacion.MVC.Autenticacion.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log On
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Log On</h2>
    <p>
        Please enter your username and password. <%: Html.ActionLink("Register", "Register") %> if you don't have an account. If you don't remember pass, plaase click <%: Html.ActionLink("RecoverPassword", "RecoverPassword")%>
    </p>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.") %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Empresa)%>
                </div>
                <div class="flow-editor-field">
                <%--<%: Html.DropDownListFor(m => m.IdEmpresa, Model.ListaEmpresas, "-- Selecciona una Empresa --", new { @class = "form-control" })%>--%>
                    <%: Html.DropDownListFor(m => m.IdEmpresa, new SelectList(Model.Empresas, "nIdEmpresa", "sEmpresa"), "-- Seleccione Empresa --", new { @class = "form-control" })%>
                    <%--<select id="Empresa" name="EmpresaTipo"></select>--%>
                    <%: Html.ValidationMessageFor(model => model.Empresa)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.UserName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.UserName) %>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe) %>
                </div>
                
                <p>
                    <input type="submit" value="Log On" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
