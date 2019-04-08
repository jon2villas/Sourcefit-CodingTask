<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Management</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous" />
</head>
<body>
    <div class="container">
        <h2>Patient Maintenance</h2>
        <form id="frmPatient" runat="server">
            <div id="divError" class="form-group row" runat="server">
                <div class="col-sm">
                    <p class="text-danger">Patient already exists.</p>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-5">
                    <asp:HiddenField ID="hfPatientId" Value="0" runat="server" />
                    <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
                    <asp:TextBox ID="txtFirstName" class="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-5">
                    <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label>
                    <asp:TextBox ID="txtLastName" class="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm">
                    <asp:Label ID="lblGender" runat="server" Text="Gender"></asp:Label>
                    <asp:DropDownList ID="ddlGender" runat="server" class="form-control">
                        <asp:ListItem>Please Select</asp:ListItem>
                        <asp:ListItem>Male</asp:ListItem>
                        <asp:ListItem>Female</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label>
                    <asp:TextBox ID="txtPhone" class="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                    <asp:TextBox ID="txtEmail" class="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm">
                    <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                    <asp:TextBox ID="txtNotes" class="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm">
                    <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary mr-2" OnClick="btnSave_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" class="btn btn-secondary" OnClick="btnClear_Click" />
                </div>
            </div>
            <asp:Table ID="tblPatient" runat="server" class="table table-striped mt-5">

            </asp:Table>
        </form>
    </div>
</body>
</html>
