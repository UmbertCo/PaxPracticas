<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preview.aspx.cs" Inherits="PAXPlantillaNomina12CFDI33.Preview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
   <asp:HyperLink ID="lnkPDF" runat="server" NavigateUrl="~/ReportOutput.aspx/MySaveAsFileName" Target="_blank">View PDF</asp:HyperLink>
       
    </div>
    </form>
</body>
</html>
