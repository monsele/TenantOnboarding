<%@ Page Title="Tenant" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.vb" Inherits="TenantOnboarding.Tenants_Edit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <a class="back-link" href="<%: ResolveUrl("~/Tenants") %>">
            <svg width="14" height="14" viewBox="0 0 14 14" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M9 2L4 7l5 5" /></svg>
            All tenants
        </a>
        <h1 class="page-title" style="margin-top: 8px"><asp:Literal ID="HeadingLiteral" runat="server" /></h1>
        <p class="page-sub" style="margin-bottom: 24px">Connection details the platform uses to reach this tenant's database.</p>

        <asp:Panel ID="ErrorPanel" runat="server" Visible="false" CssClass="notice notice-danger" role="alert" style="max-width: 760px">
            <asp:Literal ID="ErrorLiteral" runat="server" />
        </asp:Panel>
        <asp:ValidationSummary runat="server" CssClass="notice notice-danger" HeaderText="Please fix the following:" style="max-width: 760px; display: block" />

        <div class="form-card">
            <fieldset class="form-section">
                <legend>Tenant</legend>
                <div class="form-grid">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="TenantIdBox" CssClass="field-label" Text="Tenant ID" />
                        <asp:TextBox ID="TenantIdBox" runat="server" CssClass="field mono" MaxLength="50" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TenantIdBox" ErrorMessage="Tenant ID is required." Display="None" />
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="TenantNameBox" CssClass="field-label" Text="Tenant name" />
                        <asp:TextBox ID="TenantNameBox" runat="server" CssClass="field" MaxLength="200" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TenantNameBox" ErrorMessage="Tenant name is required." Display="None" />
                    </div>
                </div>
            </fieldset>

            <fieldset class="form-section">
                <legend>Database connection</legend>
                <div class="form-grid">
                    <div>
                        <asp:Label runat="server" AssociatedControlID="DatabaseNameBox" CssClass="field-label" Text="Database name" />
                        <asp:TextBox ID="DatabaseNameBox" runat="server" CssClass="field mono" MaxLength="128" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="DatabaseNameBox" ErrorMessage="Database name is required." Display="None" />
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="DatabaseServerBox" CssClass="field-label" Text="Database server name" />
                        <asp:TextBox ID="DatabaseServerBox" runat="server" CssClass="field mono" MaxLength="255" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="DatabaseServerBox" ErrorMessage="Database server name is required." Display="None" />
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="DatabaseUserBox" CssClass="field-label" Text="Database user ID" />
                        <asp:TextBox ID="DatabaseUserBox" runat="server" CssClass="field mono" MaxLength="128" autocomplete="off" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="DatabaseUserBox" ErrorMessage="Database user ID is required." Display="None" />
                    </div>
                    <div>
                        <asp:Label runat="server" AssociatedControlID="DatabasePasswordBox" CssClass="field-label" Text="Database password" />
                        <asp:TextBox ID="DatabasePasswordBox" runat="server" CssClass="field" TextMode="Password" autocomplete="new-password" />
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="DatabasePasswordBox" ErrorMessage="Database password is required." Display="None" />
                        <p class="field-hint">Stored encrypted (AES-256).<asp:Literal ID="PasswordHintLiteral" runat="server" /></p>
                    </div>
                </div>
            </fieldset>

            <div class="form-actions">
                <asp:Button ID="SaveButton" runat="server" Text="Save tenant" CssClass="btn-accent" />
                <a class="btn-ghost" href="<%: ResolveUrl("~/Tenants") %>">Cancel</a>
            </div>
        </div>
    </main>
</asp:Content>
