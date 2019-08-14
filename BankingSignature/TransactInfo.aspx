<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransactInfo.aspx.cs" Inherits="BankingSignature.TransactInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TransactInfo</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
</head>
<body>
    <form id="form1" runat="server">
        
        <div>        
            <asp:Button ID="Button1" style="float:right" runat="server" Text="Logout"  CssClass="btn btn-secondary" OnClick="Button1_Click"/>
            <center>
            <fieldset style="width:400px;border:solid;border-bottom-left-radius:4px">
                <legend><h3>Welcome MR. 
                <asp:Literal id="txtValueA" runat="server" EnableViewState="false"></asp:Literal>
                 </h3>
                </legend>
                <table border="1" width="100%" height="500px" class="table-hover" >
                    <tr>
                        <th>Acc. No</th>
                        <td>
                            <asp:TextBox ID="txtAcc" runat="server" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Transaction Date</th>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Transaction Type</th>
                        <td>
                            <asp:RadioButtonList ID="rbtType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem>Diposit</asp:ListItem>
                                <asp:ListItem>Withdraw</asp:ListItem>
                                
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th>Amount</th>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Button ID="btnSubmit" style="margin:0 auto;" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn-info"/>
                        </th>
                        <th>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-danger" OnClick="btnCancel_Click"/>
                        </th>
                       
                    </tr>
                </table>
            </fieldset>
            </center>
            
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
</body>
</html>
