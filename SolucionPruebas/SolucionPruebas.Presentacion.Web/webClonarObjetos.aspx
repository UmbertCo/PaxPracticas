<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webClonarObjetos.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.webClonarObjetos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<style type="text/css">
    .BotonEstiloAgregar
    {
        background-image: url('../mnu_images/boton_entrar.png');
        background-repeat: no-repeat;
        background-position: center center;
        background-color: #FFFFFF;
        border-color: #FFFFFF;
        border-style: none;
        height: 32px;
        width: 120px;
        color: #FFFFFF;
        font-family: 'Century Gothic';
        font-size: 11px;
    }
    .BotonEstiloEliminar
    {
        background-image: url('../mnu_images/botonGris.png');
        background-repeat: no-repeat;
        background-position: center center;
        background-color: #FFFFFF;
        border-color: #FFFFFF;
        border-style: none;
        height: 32px;
        width: 120px;
        color: #FFFFFF;
        font-family: 'Century Gothic';
        font-size: 11px;
    }
</style>
    <script src="Scripts/jquery-2.2.4.js" type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/jquery-2.2.4.js") %>' type="text/javascript"></script>

    <script type="text/javascript">

        var regex = /^(.+?)(\d+)$/i;
        var cloneIndex = $(".clonedInput").length;
        var sXMLLayout = "";

        function mostrarMensaje(objeto) { 
            alert(objeto.id)
        }

        function fnBuscarElementos() {

            var cElementos = document.body.getElementsByTagName("*");

            if (cElementos == null) {
            
                return;
            }

            var sXMLLayout = "";
            var sNombreNodo = "";

            for (var i = 0; i < cElementos.length; i++) {

                var sTipoElemento = cElementos[i].tagName.toLowerCase();
                var sNombreElemento = cElementos[i].id;
                
                var sNodo = "";

                if (sNombreElemento != null & sNombreElemento != "" & sNombreElemento != undefined) {

                    if (sTipoElemento == "div") {

                        var sComplemento = $("#" + sNombreElemento).attr("complemento");
                        var sNodo = $("#" + sNombreElemento).attr("nodo");

                        if (sComplemento != null & sComplemento != undefined & sComplemento != "") {

                            if (sXMLLayout != null & sXMLLayout != undefined & sXMLLayout != "") {
                                sXMLLayout = sXMLLayout + "/>\n<comp tipo=\"" + sComplemento + "\"";
                            }
                            else {
                                sXMLLayout = sXMLLayout + "<comp tipo=\"" + sComplemento + "\" />\n";
                            }
                            

                        }
                        else if (sNodo != null & sNodo != undefined & sNodo != "") {

                            if (sNombreNodo != "") {

                                sXMLLayout = sXMLLayout + " />\n"
                                sXMLLayout = sXMLLayout + "<" +  sNodo;
                            }
                            else {

                                sXMLLayout = sXMLLayout + "<" + sNodo;

                            }
                            
                            sNombreNodo = sNodo;
                        }
                        else 
                        { 
                        
                        }
                    }
                    else if (sTipoElemento == "input") {

                        var sAtributo = $("#" + sNombreElemento).attr("atributo");

                        if (sAtributo != null & sAtributo != undefined & sAtributo != "") {

                            var sValor = cElementos[i].value;

                            if (sValor != null & sValor != undefined & sValor != "") {

                                sXMLLayout = sXMLLayout + " " + sAtributo + "=\"" + sValor + "\"";

                            }
                        }
                    }
                    else if (sTipoElemento == "select") {

                        var sAtributo = $("#" + sNombreElemento).attr("atributo");

                        if (sAtributo != null & sAtributo != undefined & sAtributo != "") {

                            var sValor = cElementos[i].value;

                            if (sValor != null & sValor != undefined & sValor != "") {

                                sXMLLayout = sXMLLayout + " " + sAtributo + "=\"" + sValor + "\"";

                            }
                        }
                    }
                    else 
                    {

                    }
                }
            }

            if (sXMLLayout != null & sXMLLayout != undefined & sXMLLayout != "") {

                sXMLLayout = "<Datos>\n" + sXMLLayout + " />\n</Datos>";
            }

            if (sXMLLayout != null & sXMLLayout != undefined & sXMLLayout != "") {

                alert(sXMLLayout);
            }
            else
            {
                alert("Complemento no generado");
            }
        }
       

        function clone(boton) {

            var bBoton = $(document.getElementById(boton.id));

            bBoton.parents(".clonedInput").clone()
                    .appendTo("body")
                    .attr("id", "clonedInput" + cloneIndex)
                    .find("*")
                    .each(function () {
                        var id = this.id || "";
                        var match = id.match(regex) || [];
                        if (match.length == 3) {
                            this.id = match[1] + (cloneIndex);
                        }
                    });
//                    .on('click', 'button.clone', clone)
//                    .on('click', 'button.remove', remove);
            cloneIndex++;
        }

        function clone2(boton, div) {

            var bDiv = $(document.getElementById(div.id));
            var bBoton = $(document.getElementById(boton.id));

            var bDivClonado = bDiv.clone();
            bDivClonado.appendTo("body")
                    .attr("id", "clonedInput" + cloneIndex)
                    .find("*")
                    .each(function () {
                        var id = this.id || "";
                        var match = id.match(regex) || [];
                        if (match.length == 3) {
                            this.id = match[1] + (cloneIndex);
                        }
                    });
            cloneIndex++;

        }

        function fnDivClone(sNombreObjeto, sNumeroComplemento, sNombrePanel, sNombrePanelPrincipal) {

            var sIdBoton = sNombreObjeto.id;
            var bBoton = $(document.getElementById(sIdBoton));
//            var bDivClonar = sNombrePanel.id;
            var bDivPadre = bBoton.parent().parent().parent().parent();

            var sIdPanelPadre = bDivPadre[0].id;
            var sIdHijo = sNombrePanel.id;

            var sNombre = '.' + sIdHijo;
            var nCloneIndex = $(sNombre).length;

            bBoton.parents(sNombre).clone(true)
                    .appendTo('#' + sIdPanelPadre)
                    .attr("id", sIdHijo + "_Clone_" + nCloneIndex)
                    .find("*")
                    .each(function () {
                        var id = this.id || "";
                        if (this.id != null && this.id != undefined && this.id != '') {
                            this.id = this.id + '_' + (nCloneIndex);
                        }
                    });
            cloneIndex++;
        }

        function fnEliminar(sNombreObjeto) {
            var sIdBoton = sNombreObjeto.id;
            var bBoton = $(document.getElementById(sIdBoton));
            var bDiv = bBoton.parent().parent().parent();
            var sNombre = '.' + bDiv[0].id;
            var nCloneIndex = $(sNombre).length;

            if (nCloneIndex > 1) {

                bDiv.remove();

            }            
        }

        var IdDiv = '';

        function clone3(boton, div) {
            IdDiv = div.id;
            var bBoton = $(document.getElementById(boton.id));

            $.ajax({
                type: "POST",
                url: "webComplementos.aspx/fnAgregarBoton1",
                data: "{ pnNumeroComplementos: '1' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var bDiv = $(document.getElementById(div.id));
//                    alert(1);
//                    alert(data.d);
                    $("#" + IdDiv).append(data.d);

                },
                error: function (requeset, status, error) {
                    alert(error);
                }
            });

        }

        function clone4(boton, div) {

            var sIdBoton = boton.id;
            var bBoton = $(document.getElementById(sIdBoton));

            var bPadre = bBoton.parent().parent().parent();
        }

        function remove() {
            $(this).parents(".clonedInput").remove();
        }

        $("button.clone").on("click", clone);

        $("button.remove").on("click", remove);
    </script>

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
            <select name="txtSubSubCategory[]" id="subsubcategory1" onchange="mostrarMensaje(this);">
                <option value="">Please select sub-category</option>
                <option value="2">Segundo valor</option>
            </select>

            <form runat="server">
                <asp:TextBox runat="server" id="txtEjemplo_1" onchange="mostrarMensaje(this);"  />
                <input id="txtEjemplo3" runat="server" type="text" value="1.1" onchange="mostrarMensaje(this);"  />
                <input id="txtEjemplo4" runat="server" type="text" onchange="mostrarMensaje(this);"/>1.1
                <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="txtEjemplo3"
                    ErrorMessage="User ID is required."> *
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat=server 
                    ControlToValidate="txtEjemplo3" 
                    ErrorMessage="ID must be 6-10 letters." 
                    ValidationExpression="[a-zA-Z]{6,10}" />
                <asp:ValidationSummary runat="server" 
                    HeaderText="There were errors on the page:" />
                 
            </form>

            <input id="txtEjemplo2"  type="text" onchange="mostrarMensaje(this);" />
            
        </div>
        <div class="actions">
            <button id="btnAgregar" class="clone" onclick="clone3(this, clonedInput1);" >Agregar</button> 
            <button id="btnClonar" class="clone" onclick="clone4(this, clonedInput1);" >Clonar</button> 
            <button class="remove">Remove</button>
        </div>
    </div>

    <button id="btnBuscar" onclick="fnBuscarElementos();" style="display:block">btnBuscar</button> 
    

</body>
</html>
