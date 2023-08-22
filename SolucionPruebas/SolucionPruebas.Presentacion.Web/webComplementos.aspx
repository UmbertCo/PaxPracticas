<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webComplementos.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.webComplementos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>Untitled Page</title>

        

    </head>
    <body>
    <script src="Scripts/jquery-2.2.4.js" type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/jquery-2.2.4.js") %>' type="text/javascript"></script>

    <script type="text/javascript">
        var regex = /^(.+?)(\d+)$/i;
        var cloneCount = 1;
        var cloneIndex = $(".clonedInput").length;

        function fnSelectedIndexChanged(sNombreObjeto, sNumeroComplemento) {
            var Id = sNombreObjeto.id;
            var Valor = document.getElementById(Id).value
            PageMethods.DropDownList_SelectedIndexChanged(Id, Valor, sNumeroComplemento);
        }

        function fnTextChanged(sNombreObjeto, sNumeroComplemento) {
            var Id = sNombreObjeto.id;
            var Valor = document.getElementById(Id).value
            PageMethods.TextBox_TextChanged(Id, Valor, sNumeroComplemento);
        }

        function fnAgregar(sNombreObjeto, sNumeroComplemento, sNombrePanel, sNombrePanelPrincipal) {
            var Id = sNombrePanelPrincipal.id;
            //var node = document.getElementById(Id).lastChild;
            var node = $(document.getElementById(Id).lasCthild);
            //var cln = node.cloneNode(true);
            var cln = node.clone(true);

            var Padre = $(document.getElementById(Id));

            cln.appendTo(sNombrePanelPrincipal);
        }



        function fnAgregarDIV(sNombreObjeto, sNumeroComplemento, sNombrePanel, sNombrePanelPrincipal) {
            var HijoId = sNombrePanel.id;
            //var node = document.getElementById(Id).lastChild;
            var node = $(document.getElementById(HijoId));
            //var cln = node.cloneNode(true);
            var cln = node.clone(true);

            var PadreId = sNombrePanelPrincipal.id;

            var NodoPadre = $(document.getElementById(HijoId));

            cln.appendTo(NodoPadre);
        }

        function fnDivClone(sNombreObjeto, sNumeroComplemento, sNombrePanel, sNombrePanelPrincipal) {
            var sIdBoton = sNombreObjeto.id;
            var sIdPanelPadre = sNombrePanelPrincipal.id;
            var sIdHijo = sNombrePanel.id;
            var sNombre = '.' + sIdHijo;
            var nCloneIndex = $(sNombre).length;

            var bBoton = $(document.getElementById(sIdBoton));

            bBoton.parents(sNombre).clone(true)
                    .appendTo('#' + sIdPanelPadre)
                    .attr("id", sIdHijo + "_" + nCloneIndex)
                    .find("*")
                    .each(function () {
                        var id = this.id || "";
                        if (this.id != null && this.id != undefined && this.id != '') {
                            this.id = this.id + '_' + (nCloneIndex);
                        }
                    });
                    cloneIndex++;

                    var bDivClonado = $(document.getElementById(sIdHijo + "_" + nCloneIndex));
                    var sCotenido = 'Texto ejemplo';
                    PageMethods.fnAgregarBoton1(sCotenido, sIdPanelPadre);
        }

//        function fnAgregarBoton(sNombreObjeto, sNumeroComplemento, sNombrePanel, sNombrePanelPrincipal) {
//            var sIdHijo = sNombrePanel.id;
//            var sIdPadre = sNombrePanelPrincipal.id;
//            var sNombre = '.' + sIdHijo;
//            var nCloneIndex = $(sNombre).length;

//            var sDiv = PageMethods.fnAgregarBoton(sIdHijo, sIdPadre, nCloneIndex);


//        }

        function fnAgregarBoton(sNombreObjeto) {
            var sIdDiv = sNombreObjeto.id;

            $.ajax({
                type: "POST",
                url: "webComplementos.aspx/fnAgregarBoton1",
                data: "{ pnNumeroComplementos: '1' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("#" + sIdDiv).append(data.d);

                },
                error: function (requeset, status, error) {
                    alert(error);
                }
            });

        }

        function fnAgregarS(sNombreObjeto, sNumeroComplemento, sNombrePanelPrincipal) {
            var Id = sNombrePanelPrincipal.id;
            var node = document.getElementById(Id).lastChild;
            var cln = node.cloneNode(true);
            document.getElementById(Id).appendChild(cln);
        }

        function fnEliminar(sNombreObjeto, sNumeroComplemento, sNombrePanel, sNombrePanelPrincipal) {
            var Id = sNombreObjeto.id;
            var itm = sNombrePanel;
            var cln = itm.cloneNode(true);
            document.getElementById(sNombrePanelPrincipal).innerHTML = ocument.getElementById(sNombrePanelPrincipal).innerHTML + cln;
        }

        function clone(boton, div) {
            var bBoton = $(document.getElementById(boton.id));
            var bDiv = '#' + div.id;

            bBoton.parents(".clonedInput").clone()
                    .appendTo(bDiv)
                    .attr("id", "clonedInput" + cloneIndex)
                    .find("*")
                    .each(function () {
                        var id = this.id || "";
                        var match = id.match(regex) || [];
                        if (match.length == 3) {
                            this.id = this.id + '_' + (cloneIndex);
                        }
                    })
                    .on('click', 'button.clone', clone)
                    .on('click', 'button.remove', remove);
            cloneIndex++;
        }

        function remove() {
            $(this).parents(".clonedInput").remove();
        }

        $("button.clone").on("click", clone);

        $("button.remove").on("click", remove);

        function mostrarMensaje(objeto) {
            alert(objeto.id)
        }

        </script>

        <form id="form1" runat="server">


            <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True" AjaxFrameworkMode="Enabled" EnablePageMethods="true">
            </asp:ScriptManager>

            <h2>
                Pruebas
            </h2>
            <p>
                <asp:DropDownList ID="ddlComplementos" runat="server">
                    <asp:ListItem Value="1">LeyendasFiscales</asp:ListItem>
                    <asp:ListItem Value="2">Donatarias</asp:ListItem>
                </asp:DropDownList>


                <asp:Panel ID="pnlComplementos" runat="server" BorderColor="Black" 
                BorderStyle="Solid" >
                <asp:Button ID="Button2" runat="server" Text="Button3" />
                <asp:Button ID="Button3" runat="server" Text="Button3" />
                </asp:Panel>

                <asp:UpdatePanel ID="upModalAviso" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Button ID="btnCargarComplemento" runat="server" 
                            onclick="btnCargarComplemento_Click" Text="Cargar complemento" />
                        <asp:Button ID="btnCargarComplementoJS" runat="server" 
                            OnClientClick="fnAgregarBoton(DivCloned)" Text="Cargar complemento JS" />
                
                        <asp:Panel ID="pnlComplementos_Update" runat="server">
                        </asp:Panel>

                        <div id="divComplementosClonados" runat="server">
                        </div>

                        <asp:PlaceHolder ID="phContenedor" runat="server">
                
                        </asp:PlaceHolder>

                        <asp:Button ID="btnValidaDesdeCliente" runat="server" onclick="btnValidaDesdeCliente_Click" Text="Validacion Cliente" />

                        <asp:Button ID="bbtnValidar" runat="server" Text="Validar XSL" 
                            onclick="bbtnValidar_Click" />
                           
                        <asp:Button ID="btnGenerarXML" runat="server" Text="Generar XML" 
                            onclick="btnGenerarXML_Click" style="height: 26px" />

                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
        </form>
        <div id="DivCloned">
            <div id="clonedInput1" class="clonedInput">
                <div>
                    <label for="txtCategory" class="">Learning category <span class="requiredField">*</span></label>
                    <select class="" name="txtCategory[]" id="category1">
                        <option value="">Please select</option>
                    </select>
                </div>
                <div>
                    <label for="txtSubCategory" class="">Sub-category <span class="requiredField">*</span></label>
                    <select class="" name="txtSubCategory[]" id="subcategory1">
                        <option value="">Please select category</option>
                    </select>
                </div>
                <div>
                    <label for="txtSubSubCategory">Sub-sub-category <span class="requiredField">*</span></label>
                    <select name="txtSubSubCategory[]" id="subsubcategory1">
                        <option value="">Please select sub-category</option>
                    </select>
                </div>
                <%--<form id="Form1" runat="server">
                    <asp:TextBox runat="server" id="txtEjemplo1" onchange="mostrarMensaje(this);" />
                </form>--%>
                <div class="actions">
                    <button id="btnAgregar" class="clone" onclick="clone(this, DivCloned);">Clone</button> 
                    <button class="remove">Remove</button>
                </div>
            </div>
        </div>
    </body>
</html>
