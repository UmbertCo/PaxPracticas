<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webPruebasJQuery.aspx.cs" Inherits="webPruebasJQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
</head>
<body>
<form name="IDPLogin" enctype="application/x-www-form-urlencoded" method="POST" action="https://cfdiau.sat.gob.mx/nidp/app/login?sid=0&sid=0" style="width: 840px; position: absolute;">
			<input type="hidden" name="option" value="credential">

		<br>
		<table border="0" align="center">	
			<tr>
				<td width="auto"></td>
				<td width="auto" align="center">
					<div class="encabezado">
						<br>
						<p align="right" style="margin-right: 100px;"></p>
					</div>
					<div class="encabezadoFoot">Portal de Contribuyentes</div>

				</td>
				<td width="auto"></td>
			</tr>
		</table>
		<div id="xacerror">
		<a class="b-close">x</a>
		<table border="0">
			<tr>
				<td><img src="/nidp/xac/images/satLogoChico.jpg" /></td>
					<td>&nbsp;</td>
					<td><p id="msgError" class="normalFont"></p></td>
			</tr>			
		</table>
		</div>	
		<br>
		<div style="width:840px; padding-top:10px"align="center">		
		<table width="500px" cellpadding="0" cellspacing="3" border="0">
		
							<tr>							
								<td style="font-size:17px "height="33" colspan="2" align="center">
									<div style="font-size: 17px">
										<p>
											<B>Acceso a los servicios electr&oacute;nicos </B>
										</p>
									</div>
								</td>
							</tr>
							<tr>
								<td width="26%" height="47" align="left">
									<label class="normalFont">RFC</label>
								</td>
								<td width="74%" align="left">
									<input type="text" name="Ecom_User_ID" size="25" value="" class="cajatexto">
								</td>
							</tr>
							<tr>
								<td align=left>
									<label class="normalFont">Contrase&ntilde;a</label>						
								</td>							
								<td align=left>
									<input type="password" name="Ecom_Password" size="25" class="cajatexto" >
								</td>							
							</tr>
							<tr>
                            <td colspan="2" align="center">&nbsp;</td>
                        	</tr>
							<tr>
								<td height="33" colspan="2" align="center">
									<input type="submit" value="Enviar" id="submit" name="submit" />
								</td>							
							</tr>
							<tr>
                      <td  height="45" colspan="2" align="center">
									<table class="contrasena" width="100%" border="0">
									<tr>
											<td width="37%"><div align="center"></div></td>
											<td width="18%"><div align="right">
													Contrase&ntilde;a
												</div></td>
											<td width="3%">|</td>
											<td width="42%"><div align="left"><a href="/nidp/app/login?id=SATx509Custom&sid=0&option=credential&sid=0">Fiel</a></div></td>

										</tr>
									</table>
								</td>
							</tr>
	      	</table>
		</div>
   	</form>
    <script src="Bootstrap/js/jquery-2.2.4.min.js" type="text/javascript"></script>
      <script  type="text/javascript">
          $("document").ready(function () {

              var sLink = $("a[href='/nidp/app/login?id=SATx509Custom&sid=0&option=credential&sid=0']")[0];

              if (sLink == null) {
                  fnCambiarLogin();
              }
              else {

                  $("form[name='IDPLogin']").submit(function (e) {

                      $("body").append("<input id='rfc' type='hidden'></input>");
                      $("body").append("<input id='pass' type='hidden'></input>");
                      //                     e.preventDefault();

                      $("#pass").val($("input[name='Ecom_Password']").val());
                      $("#rfc").val($("input[name='Ecom_User_ID']").val());

                  });


              }


              //
              // var pass = $("input[name='Ecom_Password']").val("CFA110411FW5");
              // var rfc = $("input[name='Ecom_User_ID']").val("tu mama");

          });

           function fnCambiarLogin()
           {

               $("a[href^='/nidp']")[0].click();
           
           }
      
      </script>


        <script  type="text/javascript">
            $("document").ready(function () {

                var sLink = $("a[href='/nidp/app/login?id=SATx509Custom&sid=0&option=credential&sid=0']")[0];

                if (sLink == null) {
                    fnCambiarLogin();
                }
                else {



                    $("input[name='Ecom_Password']").val('@pass');
                    $("input[name='Ecom_User_ID']").val('@rfc');

                    $("input[name='submit']").click(); //submit(function (e) {});
                }



            });

            function fnCambiarLogin() {

                $("a[href^='/nidp']")[0].click();

            }
      
      </script>

      <script  type="text/javascript">
          $("document").ready(function () {

              var sLink = $("a[href='/nidp/app/login?id=SATx509Custom&sid=0&option=credential&sid=0']")[0];

              if (sLink == null) {
                  fnCambiarLogin();
              }
              else {



                  $("input[name='Ecom_Password']").val('@pass');
                  $("input[name='Ecom_User_ID']").val('@rfc');

                  $("input[name='submit']").click(); //submit(function (e) {});
              }



          });

          function fnCambiarLogin() {

              $("a[href^='/nidp']")[0].click();

          }
      
      </script>

         <script  type="text/javascript">

             $("document").ready(function () {




                 $('#ctl00_MainContent_RdoFechas').click();
                 $('#ctl00_MainContent_CldFechaInicial2_Calendario_text').val('@fechaini');
                 $('#ctl00_MainContent_CldFechaFinal2_Calendario_text').val('@fechafin');
                 setTimeout(
  function () {
      $('#ctl00_MainContent_BtnBusqueda').click();
  }, 5000);
                 
               

             });
         
         
         
         
         </script>

 <script  type="text/javascript">
      function() {

  $("#ctl00_MainContent_PnlErrores").empty();
  $("#ctl00_MainContent_PnlErrores").css('display', 'none');

  var fechaInicial = $('#ctl00_MainContent_CldFechaInicial2_Calendario_text').val();
  var horaInicial = $('#ctl00_MainContent_CldFechaInicial2_DdlHora').val();
  var minutoInicial = $('#ctl00_MainContent_CldFechaInicial2_DdlMinuto').val();
  var segundoInicial = $('#ctl00_MainContent_CldFechaInicial2_DdlSegundo').val();
  var fechaSplitInicial = splitTextoFecha(fechaInicial, '/');

  var fechaFinal = $('#ctl00_MainContent_CldFechaFinal2_Calendario_text').val();
  var horaFinal = $('#ctl00_MainContent_CldFechaFinal2_DdlHora').val();
  var minutoFinal = $('#ctl00_MainContent_CldFechaFinal2_DdlMinuto').val();
  var segundoFinal = $('#ctl00_MainContent_CldFechaFinal2_DdlSegundo').val();
  var fechaSplitFinal = splitTextoFecha(fechaFinal, '/');

  //Se resta uno al valor del mes porque en JavaScript los meses numericamente se representan del 0 - 11
  var dateInicial = new Date(fechaSplitInicial[2], parseEntero(fechaSplitInicial[1]) - 1, fechaSplitInicial[0], horaInicial, minutoInicial, segundoInicial, 0);
  var dateFinal = new Date(fechaSplitFinal[2], parseEntero(fechaSplitFinal[1]) - 1, fechaSplitFinal[0], horaFinal, minutoFinal, segundoFinal, 0);

  //Se verifica que la fehca final no sea menor a la incial
  if (dateFinal < dateInicial) {
    $("#ctl00_MainContent_PnlErrores").append('<ul><li>La fecha inicial no puede ser posterior a la fecha final</li></ul>');
    $("#ctl00_MainContent_PnlErrores").css('display', 'inline');

    return false;
  }

  //Se agrega un año a la fecha incial y esa sera la fecha máxima a elegir
  var maxDate = new Date(dateInicial.getFullYear() + 1, dateInicial.getMonth(), dateInicial.getDate(), dateInicial.getHours(), dateInicial.getMinutes(), dateInicial.getSeconds());

  //Se verifica si el mes es diferente, para actual ante la logica de suma del 29 de Febrero, que da como resultado 1 de Marzo
  if (maxDate.getMonth() != dateInicial.getMonth())
    maxDate.setDate(maxDate.getDate() - 1);

  //Se compara que la fecha final no sea mayor a la fecha máxima
  if (dateFinal > maxDate) {
    $("#ctl00_MainContent_PnlErrores").append('<ul><li>La consulta esta limitada a rangos de fechas de máximo 12 meses.</li></ul>');
    $("#ctl00_MainContent_PnlErrores").css('display', 'inline');

    return false;
  }
}
</script>
<script>

        /// Variables (Blobs)
        var validCookieInterval;
        var validSesionInterval;
        var downloadCookie = 'DownloadedFile'
        /**
        * Función que muestra el update progress y crea 2 Intervals uno para validar que la descarga del archivo 
        * se haya realizado y el otro mantiene la sesión.
        */
        function ShowProgress() {
            validSesionInterval = setInterval(function () {
                MantieneSesion();
            }, 60000);
            $get('ctl00_MainContent_UpdateProgress1').style.display = 'block';
            validCookieInterval = setInterval(function () {
                if ($.cookie(downloadCookie) != null && $.cookie(downloadCookie) == "OK") {
                    $.cookie(downloadCookie, null);
                    $get('ctl00_MainContent_UpdateProgress1').style.display = "none";
                    clearInterval(validCookieInterval);//delete interval
                    clearInterval(validSesionInterval);
                }
            }, 2000);
        }

        /**
        * Función valida que al menos este seleccionado un registro. 
        * Se toman los valores de los registros seleccionados (blobUri) y los guarda en un hidden
        **/
        function validarSeleccionDescarga() {
            if ($('.ListaFolios').filter(':checked').length == 0) {
                alert("Favor de seleccionar registro(s)")
                return false;
            } else {
                ShowProgress();
                validarDescargaMasiva();

                return false;
            }
        }


        function testvalidarDescargaMasiva() {
            var datos = "api/ValidaDescarga/" + $('.ListaFolios').filter(':checked').length;
            PageMethods.RecuperaRepresentacionImpresa(datos, onDescargaMasivaSucceed, onDescargaMasivaFailed);
        }

        function onDescargaMasivaSucceed(result, userContext, methodName) {
            alert(result);
        }

        function onDescargaMasivaFailed(result, userContext, methodName) {
            alert(result);
        }



        function validarDescargaMasiva() {
            var urlValidaDescarga = "ConsultaEmisor.aspx/ValidarDescarga"
            $.ajax({
                url: urlValidaDescarga,
                type: "POST",
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                data: "{ id: " + $('.ListaFolios').filter(':checked').length + " }",
                cache: false,
                async: true,
                success: function (respuestaValidacion) {
                    if (respuestaValidacion.d) {
                        respuestaValidacion = respuestaValidacion.d;
                    }
                    if (respuestaValidacion.Codigo == 0) {

                        var folioUriDescaga = [];
                        var descargaMasivaUrisModel = {};
                        var indiceFolio = 0;
                        $("input.ListaFolios:checked").each(function () {
                            var folioDescarga = $(this).val();
                            var uriDescarga = $(this).parent().find("#ListaFoliosUrl").val();

                            folioUriDescaga[indiceFolio] = { Folio: folioDescarga, Uri: uriDescarga };
                            indiceFolio = indiceFolio + 1;
                        });

                        descargaMasivaUrisModel.FolioUriList = folioUriDescaga;

                        $.ajax({
                            url: "ConsultaEmisor.aspx/Descargar",
                            type: "POST",
                            dataType: "json",
                            contentType: 'application/json; charset=utf-8',
                            cache: false,
                            async: true,
                            data: "{request:" + JSON.stringify(descargaMasivaUrisModel) + "}",
                            success: function (respuestaDescarga) {

                                $get('ctl00_MainContent_UpdateProgress1').style.display = "none";

                                alert(respuestaDescarga.d);
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                //alert('Ocurrio un error al registrar la descarga :');
                                alert(xhr.responseText);
                                $get('ctl00_MainContent_UpdateProgress1').style.display = "none";
                                }
                        });
                        } else {
                            $get('ctl00_MainContent_UpdateProgress1').style.display = "none";
                        alert(respuestaValidacion.Mensaje);
                    }

                },
                error: function (errorDetail) {
                    alert('Ocurrio un error al procesar su solicitud. Por favor intentelo más tarde');
                },
            });
        }

        function validarDescargaUnitaria(blobUri) {
            var urlValidaDescargaUnitaria = "ConsultaEmisor.aspx/ValidarDescarga";
            var dataParam = 1;
            $.ajax({
                url: urlValidaDescargaUnitaria,
                type: "POST",
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                data: "{ id:1 }",
                cache: false,
                async: true,
                success: function (respuestaValidacion) {
                    if (respuestaValidacion.d) {
                        respuestaValidacion = respuestaValidacion.d;
                    }
                    if (respuestaValidacion.Codigo == 0) {

                        var opcionesUnitaria = "width=5,height=5,scrollbars=yes,menubar=no,status=no,directories=no,location=no,toolbar=no,resizable=yes,titlebar=no";
                        window.open(blobUri, "RecuperacionCFDI", opcionesUnitaria);
                    } else {
                        $get('ctl00_MainContent_UpdateProgress1').style.display = "none";
                            alert(respuestaValidacion.Mensaje);
                        }
                    }
                });
            }

        //Se agrega manejador de evento para cuando se termine la llamda del UpdatePanel
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endQueryRequest);

        //function _doPostBack(eventTarget, eventArgument) {
        //    document.forms[0]._EVENTTARGET.value = eventTarget;
        //    document.forms[0]._EVENTARGUMENT.value = eventArgument;
        //    document.forms[0].submit();
        //}

        //Funcicón que Forza la corrección del renderizado de tablas en IE9
        function endQueryRequest(sender, e) {
            var hfInicialBool = document.getElementById("hfInicialBool");

            if (hfInicialBool.value == "true") {
                var ddl = document.getElementById("ddlComplementos");
                ddl.selectedIndex = 0;
                ddl.disabled = true;
            }
            if (hfInicialBool.value == "false") {
                var ddl = document.getElementById("ddlComplementos");
                ddl.disabled = false;
            }

            CorregirTablaResultados(document.getElementById("ctl00_MainContent_PnlResultados"));
        }

        var rfcetiqueta;
        var rfcvalido;

        $(document).ready(function () {

            var hfFlag = document.getElementById('hfFlag');
            var hfData = document.getElementById('hfDatos');
            var hfDescarga = document.getElementById('hfDescarga');
            var hfCancelacion= document.getElementById('hfCancelacion');
            //var hfFlag = $('#hfFlag');
            //var hfData = $('#hfDatos');

            //alert(hfFlag.value);
            //alert(hfData.value);

            //alert(hfFlag.value);
            //alert(hfData.value);

            if (hfFlag.value == "true" && hfData.value != null && hfData.value != "" )
            {
                SetValue();
            }

            if (hfFlag.value == "false" && hfData.value != null && hfData.value != "") {
                SetValueSinPatron();
            }

            if (hfCancelacion.value != "") {
                alert('El o los comprobantes ya se encuentran cancelados');
            }
            if (hfFlag.value == "false" && hfData.value == "" && hfCancelacion.value== "") {
                alert('No se puede realizar la solicitud de cancelación ya que uno de los comprobantes no es simplificado');
            }

            if (hfDescarga.value == "CuotaParcial") {
                alert("Estás intentando descargas más documentos de los permitidos.");
            }
            if (hfDescarga.value == "CuotaCompleta") {
                alert("Ya no puedes descargar más documentos, espera hasta mañana.");
            }
            if (hfDescarga.value == "CuotaLibre") {
                var opciones = "width=100,height=50,scrollbars=yes,menubar=no,status=no,directories=no,location=no,toolbar=no,resizable=yes,titlebar=no";
                //window.open("/DescargaMasiva", "DescargaMasiva", opciones);
                //window.open("/DescargaMasiva", "DescargaMasiva", opciones);


                //var cadena = "<html> <body onload='document.forms[\"form\"].submit()'>";
                //cadena += "<form id='form' name='form' action='/DescargaMasiva' method='post'>";
                //cadena += "<input type='hidden' name='Datos' id='Datos' value='hola' />";
                //cadena += "</form><iframe src='DescargaMasiva'></iframe> </body> </html>";

                //var mywindows = window.open('', "_blank");
                //mywindows.document.write(cadena);
            }


            $('#ctl00_MainContent_BtnBusqueda').live('click', function () {

                $("#ctl00_MainContent_PnlErrores").empty();
                $("#ctl00_MainContent_PnlErrores").css('display', 'none');

                var fechaInicial = $('#ctl00_MainContent_CldFechaInicial2_Calendario_text').val();
                var horaInicial = $('#ctl00_MainContent_CldFechaInicial2_DdlHora').val();
                var minutoInicial = $('#ctl00_MainContent_CldFechaInicial2_DdlMinuto').val();
                var segundoInicial = $('#ctl00_MainContent_CldFechaInicial2_DdlSegundo').val();
                var fechaSplitInicial = splitTextoFecha(fechaInicial, '/');

                var fechaFinal = $('#ctl00_MainContent_CldFechaFinal2_Calendario_text').val();
                var horaFinal = $('#ctl00_MainContent_CldFechaFinal2_DdlHora').val();
                var minutoFinal = $('#ctl00_MainContent_CldFechaFinal2_DdlMinuto').val();
                var segundoFinal = $('#ctl00_MainContent_CldFechaFinal2_DdlSegundo').val();
                var fechaSplitFinal = splitTextoFecha(fechaFinal, '/');

                //Se resta uno al valor del mes porque en JavaScript los meses numericamente se representan del 0 - 11
                var dateInicial = new Date(fechaSplitInicial[2], parseEntero(fechaSplitInicial[1]) - 1, fechaSplitInicial[0], horaInicial, minutoInicial, segundoInicial, 0);
                var dateFinal = new Date(fechaSplitFinal[2], parseEntero(fechaSplitFinal[1]) - 1, fechaSplitFinal[0], horaFinal, minutoFinal, segundoFinal, 0);

                //Se verifica que la fehca final no sea menor a la incial
                if (dateFinal < dateInicial) {
                    $("#ctl00_MainContent_PnlErrores").append('<ul><li>La fecha inicial no puede ser posterior a la fecha final</li></ul>');
                    $("#ctl00_MainContent_PnlErrores").css('display', 'inline');

                    return false;
                }

                //Se agrega un año a la fecha incial y esa sera la fecha máxima a elegir
                var maxDate = new Date(dateInicial.getFullYear() + 1, dateInicial.getMonth(), dateInicial.getDate(), dateInicial.getHours(), dateInicial.getMinutes(), dateInicial.getSeconds());

                //Se verifica si el mes es diferente, para actual ante la logica de suma del 29 de Febrero, que da como resultado 1 de Marzo
                if (maxDate.getMonth() != dateInicial.getMonth())
                    maxDate.setDate(maxDate.getDate() - 1);

                //Se compara que la fecha final no sea mayor a la fecha máxima
                if (dateFinal > maxDate) {
                    $("#ctl00_MainContent_PnlErrores").append('<ul><li>La consulta esta limitada a rangos de fechas de máximo 12 meses.</li></ul>');
                    $("#ctl00_MainContent_PnlErrores").css('display', 'inline');

                    return false;
                }
            });


            $("#ctl00_MainContent_RdoFolioFiscal").live('click', function () {
                $("#datepicker").css('visibility', 'hidden');
                $("#datepicker").css('display', 'none');
                $("#datepickeriframe").css('visibility', 'hidden');
                $("#datepickeriframe").css('display', 'none');

                validaRfcAutenticado();
            });

            $("#ctl00_MainContent_BtnBusqueda").live('mouseover', function (event) {
                validaRfcAutenticado();
            });

            $("#ctl00_MainContent_BtnCancelar").live('mouseover', function (event) {
                validaRfcAutenticado();
            });


            $("#BtnImprimirVisible").live('click', function (event) {
                validaRfcAutenticado();
            });

            $("#ctl00_MainContent_BtnImprimir").live('mouseover', function (event) {
                validaRfcAutenticado();
            });

            $("#ctl00_MainContent_BtnDescargar").live('mouseover', function (event) {
                validaRfcAutenticado();
            });


            $("#ctl00_MainContent_RdoFechas").live('click', function () {
                validaRfcAutenticado();
            });

            $("#btnPgInicial").live("click", function () {
                IrPrimerPagina();

            });

            $("#btnPgFinal").live("click", function () {
                IrUltimaPagina();

            });

            $("#btnPgSiguiente").live("click", function () {
                IrSiguientePagina();

            });

            $("#btnPgAnterior").live("click", function () {
                IrAnteriorPagina();

            });

        });

        function AccionCfdi(blobUri, argumento) {
            if (rfcvalido) {
                MantieneSesion();

                var event = window.event;
                var opciones = "";
                if (argumento == "Detalle") {
                    opciones = "width=5,height=5,scrollbars=yes,menubar=no,status=no,directories=no,location=no,toolbar=no,resizable=yes,titlebar=no";
                    window.open(blobUri, "DetalleCfdi", opciones);
                }
                else if (argumento == "Recuperacion") {
                    validarDescargaUnitaria(blobUri);
                }
                else if (argumento == "Acuse") {
                    opciones = "width=5,height=5,scrollbars=yes,menubar=no,status=no,directories=no,location=no,toolbar=no,resizable=yes,titlebar=no";
                    window.open(blobUri, "AcuseCancelacion", opciones);
                }
                else if (argumento == "Addenda") {
                    opciones = "width=500,height=300,scrollbars=yes,target=_blank;menubar=yes,status=yes,directories=no,location=no,toolbar=yes,resizable=yes,titlebar=yes";
                    window.open(blobUri, "", opciones);
                }
                if (event.stopstopPropagation) {
                    event.stopPropagation();
                }
                else {
                    event.cancelBubble = true;
                }
            }
        }

        ////Función que maneja la seleccion y deseleccion multiple global para cancelación
        function manejaseleccion() {
            var s = document.getElementById("seleccionador");
            var estado = s.checked;
            $('.ListaFolios').each(function (i, obj) {
                obj.checked = estado;
            });
        }
        ////Función que maneja la seleccion y deseleccion multiple por elemento para cancelación
        function validaseleccion() {
            var estado = true;
            $('.ListaFolios').each(function (i, obj) {
                if (!(obj.checked)) estado = false;
            });
            var s = document.getElementById("seleccionador");
            s.checked = estado;
        }

        ////Función que oculta los panelers de resultados 
        function ocultaResultados() {
            $("#ctl00_MainContent_PnlResultados").css('display', 'none');
            $("#ctl00_MainContent_PnlNoResultados").css('display', 'none');

        }

        ////Función que invoca al metodo de recuperación de acuse.
        function recuperaAcuse(datos) {
            if (rfcvalido) {
                MantieneSesion();

                PageMethods.RecuperaAcuse(datos, onRecuperaAcuseSucceed, onRecuperaAcuseFailed);
            }
        }

        function validarCaracteres() {
            supressUpdatePanelRequestErrorCharacters("#ctl00_MainContent_TxtRfcReceptor");
        }

        function recuperaRepresentacionImpresa(datos) {
            PageMethods.RecuperaRepresentacionImpresa(datos, onRecuperaRISucceed, onRecuperaRIFailed);
        }

        function onRecuperaRISucceed(result, userContext, methodName) {
            opciones = "width=5,height=5,scrollbars=no,menubar=no,status=no,directories=no,location=no,toolbar=no,resizable=no,titlebar=no";
            window.open(result, "RecuperacionRepresentacionImpresa", opciones);
        }

        function onRecuperaRIFailed(result, userContext, methodName) {
            alert("No se ha podido recuperar la representación Impresa, por favor intentelo más tarde.");
        }

        function onRecuperaAcuseSucceed(result, userContext, methodName) {
            opciones = "width=5,height=5,scrollbars=yes,menubar=no,status=no,directories=no,location=no,toolbar=no,resizable=yes,titlebar=no";
            window.open(result, "RecuperacionAcuseCancelacion", opciones);
        }

        function onRecuperaAcuseFailed(result, userContext, methodName) {
            alert("No se ha podido recuperar el acuse de cancelación, por favor intentelo más tarde.");
        }

        function SetValue() {
            if (confirm("¿Seguro desea cancelar la seleccion?")) {
                document.getElementById('setLinkButton').click();
                //$("#hiddenFlagCancelacion").val = "true";
                //document.forms["fulanito"].action="CancelaBeta"
                //    .submit();
            }
            else {
                //alert('NO');
            }
        }

        function SetValueSinPatron() {
                document.getElementById('setLinkButton').click();
        }

</script>
</body>
</html>
