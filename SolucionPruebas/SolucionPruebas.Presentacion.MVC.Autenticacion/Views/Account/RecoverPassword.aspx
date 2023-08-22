<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SolucionPruebas.Presentacion.MVC.Autenticacion.Models.RecoverPasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RecoverPasswordView
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Recover Password</h2>
    <p>
        Please enter your email.
    </p>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Recuperación de la contraseña sin exito. Favor de revisar los datos capturados. ") %>

        <div>
            <fieldset>
                <legend>Account Information</legend>
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Email)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email)%>
                    <%: Html.ValidationMessageFor(m => m.Email)%>
                </div>
                <p>
                    <input type="submit" value="Recover Password" />
                </p>
            </fieldset>
        </div>

    <% } %>

</asp:Content>
