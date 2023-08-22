<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webEntityFramework.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.webEntityFramework" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <asp:DropDownList ID="ddlListaDepartamentos" runat="server" AutoPostBack="True" 
        onselectedindexchanged="ddlListaDepartamentos_SelectedIndexChanged">
    </asp:DropDownList>
    <asp:gridview ID="gvCursos" runat="server" DataKeyNames="CourseID" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="CourseID" HeaderText="CourseID" ReadOnly="True" SortExpression="CourseID" />
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
            <asp:BoundField DataField="Credits" HeaderText="Credits" SortExpression="Credits" />
        </Columns>
    </asp:gridview>
    </form>
</body>
</html>
