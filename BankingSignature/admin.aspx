<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="BankingSignature.admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Panel</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
</head>
<body style="text-align:center;background-color:gainsboro" >
    
    <h1 style="font-size:50px;margin-bottom:30px">Key Generation Panel</h1>
    
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="Button3" style="float:right" runat="server" Text="Logout" OnClick="Button3_Click" CssClass="btn btn-secondary" />
        </div>
         <div style="margin-bottom:10px;margin-left:60px">
            <asp:TextBox ID="userId" runat="server" placeholder="Enter the user id" TextMode="Number" ></asp:TextBox>
        </div>
        <div style="text-align:center">
            <asp:Button ID="Button1" runat="server" Text="Insert Keys" OnClick="Button1_Click" CssClass="btn btn-info"/>
            <asp:Button ID="Button2" runat="server" Text="Regenarate Keys" OnClick="Button2_Click" CssClass="btn btn-info"/>
        </div>
        

        <h1 style="font-size:50px;margin-bottom:30px;margin-top:50px;background-color:cadetblue">Notification Panel</h1>
        <asp:Label ID="Label1" runat="server"></asp:Label>

        <asp:GridView ID="GridView1" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False" CssClass="table table-light" OnRowCommand="GridView1_RowCommand">
            <Columns>
                <asp:BoundField DataField="user_id" HeaderText="User Id" />
                <asp:BoundField DataField="file" HeaderText="File" />
                <%--<asp:BoundField DataField="user name" HeaderText="User Name" />
                <asp:BoundField DataField="acc. num" HeaderText="Account Number" />
                --%>
                <asp:ButtonField Text="Validate" CommandName="validate" />
            </Columns>

        </asp:GridView>
    </form>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
</body>
</html>
