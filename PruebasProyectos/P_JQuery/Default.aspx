<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <script src="Bootstrap/js/jquery-2.2.4.min.js" type="text/javascript"></script>
 

    <div class="separadoHora"><span id="ctl00_MainContent_CldFecha_LblAnio" style="top: 0px; left: 0px">*Año:</span></div>

    <select name="ctl00$MainContent$CldFecha$DdlAnio" id="DdlAnio" disabled="disabled" class="aspNetDisabled calendarCombo2" onchange="ValidateYear();" style="width: 80px; display: none;" sb="92028051">
				<option value="2011">2011</option>
				<option value="2012">2012</option>
				<option value="2013">2013</option>
				<option value="2014">2014</option>
				<option value="2015">2015</option>
				<option selected="selected" value="2016">2016</option>

			</select>

			<div id="sbHolder_92028051" class="sbHolder sbHolderDisabled" style="width: 85px;"><a id="sbToggle_92028051" href="#" class="sbToggle"></a><a id="sbSelector_92028051" href="#" class="sbSelector" style="width: 62px;">2016</a><ul id="sbOptions_92028051" class="sbOptions" style="display: none; width: 85px;"><li><a href="#2011" rel="2011">2011</a></li><li><a href="#2012" rel="2012">2012</a></li><li><a href="#2013" rel="2013">2013</a></li><li><a href="#2014" rel="2014">2014</a></li><li><a href="#2015" rel="2015">2015</a></li><li><a href="#2016" rel="2016" class="sbFocus">2016</a></li></ul></div>

   <script type="text/javascript">
       $("document").ready(function () {


           alert($("#DdlAnio").next("div[id^='sbHolder']").find("ul > li> a[href='#2014']").click());

           

       });  


    
    </script>

</body>
</html>
