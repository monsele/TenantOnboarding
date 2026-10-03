<%@ Page Title="Tenant" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.vb" Inherits="TenantOnboarding.Tenants_Edit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="pt-4" style="max-width: 720px;">
        <h1 class="h3 mb-1"><asp:Literal ID="HeadingLiteral" runat="server" /></h1>
        <p class="text-muted">Connection details the platform uses to reach this tenant's database.</p>

        <asp:Panel ID="ErrorPanel" runat="server" Visible="false" CssClass="alert alert-danger" role="alert">
            <asp:Literal ID="ErrorLiteral" runat="server" />
        </asp:Panel>
        <asp:ValidationSummary runat="server" CssClass="alert alert-danger" HeaderText="Please fix the following:" />

        <div class="card card-body">
            <div class="mb-3">
                <label class="form-label" for="<%: TenantIdBox.ClientID %>">Tenant ID</label>
                <asp:TextBox ID="TenantIdBox" runat="server" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="TenantIdBox" ErrorMessage="Tenant ID is required." Display="None" />
            </div>
            <div class="mb-3">
                <label class="form-label" for="<%: TenantNameBox.ClientID %>">Tenant name</label>
                <asp:TextBox ID="TenantNameBox" runat="server" CssClass="form-control" MaxLength="200" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="TenantNameBox" ErrorMessage="Tenant name is required." Display="None" />
            </div>
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label class="form-label" for="<%: DatabaseNameBox.ClientID %>">Database name</label>
                    <asp:TextBox ID="DatabaseNameBox" runat="server" CssClass="form-control" MaxLength="128" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DatabaseNameBox" ErrorMessage="Database name is required." Display="None" />
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label" for="<%: DatabaseServerBox.ClientID %>">Database server name</label>
                    <asp:TextBox ID="DatabaseServerBox" runat="server" CssClass="form-control" MaxLength="255" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DatabaseServerBox" ErrorMessage="Database server name is required." Display="None" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label class="form-label" for="<%: DatabaseUserBox.ClientID %>">Database user ID</label>
                    <asp:TextBox ID="DatabaseUserBox" runat="server" CssClass="form-control" MaxLength="128" autocomplete="off" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DatabaseUserBox" ErrorMessage="Database user ID is required." Display="None" />
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label" for="<%: DatabasePasswordBox.ClientID %>">Database password</label>
                    <asp:TextBox ID="DatabasePasswordBox" runat="server" CssClass="form-control" TextMode="Password" autocomplete="new-password" />
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="DatabasePasswordBox" ErrorMessage="Database password is required." Display="None" />
                    <div class="form-text">Stored encrypted (AES-256).<asp:Literal ID="PasswordHintLiteral" runat="server" /></div>
                </div>
            </div>
            <div class="d-flex gap-2">
                <asp:Button ID="SaveButton" runat="server" Text="Save" CssClass="btn btn-primary" />
                <a class="btn btn-outline-secondary" href="<%: ResolveUrl("~/Tenants") %>">Cancel</a>
            </div>
        </div>
    </main>
</asp:Content>
