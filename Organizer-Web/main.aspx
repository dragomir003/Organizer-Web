<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="Organizer_Web.Main" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Organizer</title>
    <link rel="stylesheet" href="login.css" />
</head>
<body>
    <form id="formLogin" runat="server">
        <h1>Organizer</h1>
        <asp:TextBox ID="tbUsername" runat="server"></asp:TextBox>
        <div class="buttons">
            <asp:Button ID="btnLogin" runat="server" Text="Uloguj se" CssClass="cs-button" onClick="btnLogin_Click"/>
            <asp:Button ID="btnRegister" runat="server" Text="Registruj se" CssClass="cs-button" OnClick="btnRegister_Click" />
        </div>
        <asp:TextBox ID="tbError" runat="server" Text="" CssClass="error-field" ReadOnly="True"></asp:TextBox>
    </form>
</body>
</html>
