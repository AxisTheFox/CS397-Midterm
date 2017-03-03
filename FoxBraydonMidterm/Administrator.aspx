<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Administrator.aspx.cs" Inherits="FoxBraydonMidterm.Administrator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <h1>Administrator Page</h1>

        <h3>Add a new user:</h3>

        <p>User Type:&nbsp&nbsp&nbsp
            <asp:DropDownList ID="userTypeDropDownList" runat="server">
                <asp:ListItem>Administrator</asp:ListItem>
                <asp:ListItem>Developer</asp:ListItem>
                <asp:ListItem>Manager</asp:ListItem>
                <asp:ListItem>Tester</asp:ListItem>
            </asp:DropDownList></p>

        <p>User's Name:&nbsp&nbsp&nbsp
            <asp:TextBox ID="nameTextBox" runat="server"></asp:TextBox></p>

        <p>Login Username:&nbsp&nbsp&nbsp
            <asp:TextBox ID="loginUsernameTextBox" runat="server"></asp:TextBox></p>

        <p>Login Password:&nbsp&nbsp&nbsp
            <asp:TextBox ID="passwordTextBox" runat="server" TextMode="Password"></asp:TextBox></p>

        <p>
            <asp:Button ID="createUserButton" runat="server" Text="Create User" OnClick="createUserButton_Click" /></p>

        <p>
            <asp:Label ID="resultLabel" runat="server" Text=""></asp:Label></p>

    </div>
    </form>
</body>
</html>
