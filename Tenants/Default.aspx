<%@ Page Title="Tenants" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="TenantOnboarding.Tenants_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div class="page-head">
            <div>
                <h1 class="page-title">Tenants</h1>
                <p class="page-sub">Businesses onboarded onto the platform and the database each one connects to.</p>
            </div>
            <a class="btn-accent" href="<%: ResolveUrl("~/Tenants/Edit") %>">
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none" stroke="#FFFFFF" stroke-width="2" stroke-linecap="round" aria-hidden="true"><path d="M8 2v12M2 8h12" /></svg>
                Onboard new tenant
            </a>
        </div>

        <asp:Panel ID="MessagePanel" runat="server" Visible="false" CssClass="notice notice-success" role="status">
            <asp:Literal ID="MessageLiteral" runat="server" />
        </asp:Panel>

        <asp:Panel runat="server" DefaultButton="SearchButton" CssClass="search-bar">
            <asp:Label runat="server" AssociatedControlID="SearchBox" CssClass="visually-hidden" Text="Search tenants" />
            <asp:TextBox ID="SearchBox" runat="server" CssClass="field" placeholder="Search by ID, name, database or server" />
            <asp:Button ID="SearchButton" runat="server" Text="Search" CssClass="btn-ghost" />
            <asp:Button ID="ClearButton" runat="server" Text="Clear" CssClass="btn-link-plain" CausesValidation="false" />
        </asp:Panel>

        <div class="table-card">
            <asp:GridView ID="TenantsGrid" runat="server" AutoGenerateColumns="false" DataKeyNames="TenantID"
                CssClass="tenant-table" GridLines="None" BorderWidth="0" UseAccessibleHeader="true"
                EmptyDataText="No tenants found." EmptyDataRowStyle-CssClass="tenant-empty">
                <Columns>
                    <asp:BoundField DataField="TenantID" HeaderText="Tenant ID" ItemStyle-CssClass="mono" />
                    <asp:BoundField DataField="TenantName" HeaderText="Name" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="DatabaseName" HeaderText="Database" ItemStyle-CssClass="mono" />
                    <asp:BoundField DataField="DatabaseServerName" HeaderText="Server" ItemStyle-CssClass="mono" />
                    <asp:BoundField DataField="DatabaseUserId" HeaderText="User" ItemStyle-CssClass="mono" />
                    <asp:TemplateField HeaderText="Password">
                        <ItemTemplate>
                            <span class="pill">
                                <svg width="12" height="12" viewBox="0 0 12 12" fill="none" stroke="#46515F" stroke-width="1.5" stroke-linecap="round" aria-hidden="true"><rect x="2" y="5.5" width="8" height="5" rx="1" /><path d="M4 5.5V4a2 2 0 014 0v1.5" /></svg>
                                Encrypted
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="actions">
                        <ItemTemplate>
                            <a class="btn-link-plain" href="<%# ResolveUrl("~/Tenants/Edit") & "?id=" & Server.UrlEncode(Eval("TenantID").ToString()) %>">Edit</a>
                            <asp:LinkButton runat="server" CommandName="DeleteTenant" CommandArgument='<%# Eval("TenantID") %>'
                                CssClass="btn-link-danger" Text="Delete" CausesValidation="false"
                                OnClientClick="return confirm('Delete this tenant? This cannot be undone.');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <p class="field-hint" style="margin: 12px 4px 0">Passwords are stored encrypted and are never shown in this app.</p>
    </main>
</asp:Content>
