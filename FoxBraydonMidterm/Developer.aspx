<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Developer.aspx.cs" Inherits="FoxBraydonMidterm.Developer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <h1>Developer Page</h1>

        <p>Bugs assigned to you:<br />
            <asp:DropDownList ID="assignedBugsDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="assignedBugsDropDownList_SelectedIndexChanged"></asp:DropDownList></p>

        <p><strong>
            <asp:Label ID="bugSubjectLabel" runat="server" Text=""></asp:Label></strong>
            <br />
            <asp:Label ID="bugDescriptionLabel" runat="server" Text=""></asp:Label>
        </p>

        <p>Changes made to fix this bug:
            <br />
            <asp:TextBox ID="changesTextBox" runat="server" Width="40%"></asp:TextBox>
        </p>

        <p>
            <asp:Button ID="fixedButton" runat="server" Text="Fixed" OnClick="fixedButton_Click" /></p>

        <p>
            <asp:Label ID="successLabel" runat="server" Text=""></asp:Label></p>

    </div>
    </form>
</body>
</html>
