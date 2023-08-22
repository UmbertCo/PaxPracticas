<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8" />
<title>Menú Desplegable Vertical y CSS3</title>
<link rel="stylesheet" type="text/css" href="style.css" />
<!--[if lt IE 9]>
<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
</head>
<body>
   <nav>
      <li class="parent"><a href="#">Cristalab</a>
         <ul>
            <li><a href="#">Blogs</a></li>
            <li><a href="#">Foros</a></li>
            <li><a href="#">Tutoriales</a></li>
            <li ><a href="#">Cursos</a></li>
         </ul>
      </li>
      <li><a href="#">Mejorando.la</a></li>
      <li><a href="#">L4C</a></li>
      <li class="parent"><a href="#">Mas de Cristalab</a>
         <ul>
            <li><a href="#">Publica un tutorial</a></li>
            <li><a href="#">¿Qué es Cristalab?</a></li>
            <li><a href="#">Anime</a></li>
         </ul>
      </li>
   </nav>
</body>
</html>
</asp:Content>

