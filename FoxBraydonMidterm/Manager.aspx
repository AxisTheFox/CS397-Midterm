<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manager.aspx.cs" Inherits="FoxBraydonMidterm.Manager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manager Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <h1>Manager Page</h1>

        <h2>Assign an open bug to a developer</h2>

        <p>Select a bug:<br />
            <asp:DropDownList ID="bugDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="bugDropDownList_SelectedIndexChanged" /></p>

        <p><strong>
            <asp:Label ID="bugIdLabel" runat="server" Text=""></asp:Label>
            &nbsp&nbsp&nbsp
            <asp:Label ID="enteredByLabel" runat="server" Text=""></asp:Label>
            &nbsp&nbsp&nbsp
            <asp:Label ID="bugPriorityLabel" runat="server" Text=""></asp:Label>
        </strong></p>

        <p><strong>
            <asp:Label ID="bugSubjectLabel" runat="server" Text=""></asp:Label></strong>
            <br />
            <asp:Label ID="bugDescriptionLabel" runat="server" Text=""></asp:Label>
        </p>

        <p>Select a developer to assign this bug to:<br />
            <asp:DropDownList ID="developerDropDownList" runat="server"></asp:DropDownList></p>

        <p>
            <asp:Button ID="assignButton" runat="server" Text="Assign" OnClick="assignButton_Click" /></p>

    </div>
    </form>
</body>
</html>
