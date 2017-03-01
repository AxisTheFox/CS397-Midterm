<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tester.aspx.cs" Inherits="FoxBraydonMidterm.Tester" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tester Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <h1>Tester Page</h1>

        <h2>Enter a new bug</h2>

        <p>Priority:<br />
            <asp:DropDownList ID="priorityDropDownList" runat="server" AutoPostBack="True">
                <asp:ListItem>Low</asp:ListItem>
                <asp:ListItem>Medium</asp:ListItem>
                <asp:ListItem>High</asp:ListItem>
                </asp:DropDownList></p>

        <p>Subject:<br />
            <asp:TextBox ID="subjectTextBox" runat="server" Width="40%"></asp:TextBox></p>

        <p>Description:<br />
            <asp:TextBox ID="descriptionTextBox" runat="server" Width="40%"></asp:TextBox></p>

        <p>
            <asp:Button ID="submitButton" runat="server" Text="Submit Bug" OnClick="submitButton_Click" /></p>

        <h3>
            <asp:Label ID="submissionResultLabel" runat="server" Text=""></asp:Label></h3>

    </div>
    </form>
</body>
</html>
