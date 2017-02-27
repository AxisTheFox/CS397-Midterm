<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FoxBraydonMidterm.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Log In</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
            <h1>Log In</h1>

            <p>Username
                &nbsp&nbsp&nbsp
                <asp:TextBox ID="usernameTextBox" runat="server"></asp:TextBox></p>

            <p>Password
                &nbsp&nbsp&nbsp
                <asp:TextBox ID="passwordTextBox" runat="server" TextMode="Password"></asp:TextBox></p>

            <p>
                <asp:Button ID="loginButton" runat="server" Text="Log In" OnClick="loginButton_Click" /></p>

            <p>
                <asp:Label ID="loginErrorLabel" runat="server" Text="" ForeColor="Red"></asp:Label></p>

        </div>
    </form>
</body>
</html>
