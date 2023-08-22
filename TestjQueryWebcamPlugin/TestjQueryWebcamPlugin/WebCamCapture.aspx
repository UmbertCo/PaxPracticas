<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebCamCapture.aspx.cs" Inherits="TestjQueryWebcamPlugin.WebCamCapture" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Web capture</title>

    <style type="text/css">

        #profile_pic_wrapper
        {
            position: relative;
            border: #ccc solid 1px;
            width: 120px;
            height: 120px;
            border: none;
        } 

       #profile_pic_wrapper a
        {
            position: absolute;
            display: block;
            top: 30;
            right: 0;
            line-height: 20px;
            padding: 5px;
            color: #fff;
            background-color: #333;
            width: 110px;
            text-decoration: underline;
            text-align: center;
            z-index: 100;
            text-decoration: none;
            font-family: Verdana;
            font-size: 10px;
        }


        #profile_pic_wrapper:hover a
        {
            position: absolute;
            display: block;
            text-decoration: none;
            font-family: Verdana;
            font-size: 10px;
        }
 

        #profile_pic_wrapper:hover a:hover
        {
            text-decoration: none;
            font-family: Verdana;
            font-size: 10px;
        }

        .profile_pic

        {

            width: 120px;
            height: 120px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
       <div style="float: left; padding-left: 35px;" id="profile_pic_wrapper">
            <asp:Image ID="img" Width="120" Height="120" runat="server"  />
            <asp:LinkButton ID="Linkbutton1" runat="server" onclick="Linkbutton1_Click" >Change  Photo</asp:LinkButton>
     
       </div>

                <asp:LinkButton ID="Linkbutton2" runat="server" onclick="Linkbutton2_Click">Decode QR</asp:LinkButton>
       <asp:TextBox ID="txtDecoderType" runat="server" Width="249px"></asp:TextBox>
       <asp:TextBox ID="txtDecoderContent" runat="server" Width="436px"></asp:TextBox>
    </form>
</body>
</html>
