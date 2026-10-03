<%@ Page Title="Tenants" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="TenantOnboarding.Tenants_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="pt-4">
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-3 gap-2">
            <div>
                <h1 class="h3 mb-0">Tenants</h1>
                <p class="text-muted mb-0">Businesses onboarded onto the platform.</p>
            </div>
            <a class="btn btn-primary" href="<%: ResolveUrl("~/Tenants/Edit") %>">+ Onboard new tenant</a>
        </div>

        <asp:Panel ID="MessagePanel" runat="server" Visible="false" CssClass="alert alert-success" role="alert">
            <asp:Literal ID="MessageLiteral" runat="server" />
        </asp:Panel>

        <asp:Panel runat="server" DefaultButton="SearchButton" CssClass="input-group mb-3">
            <asp:TextBox ID="SearchBox" runat="server" CssClass="form-control" placeholder="Search by ID, name, database or server" />
            <asp:Button ID="SearchButton" runat="server" Text="Search" CssClass="btn btn-outline-secondary" />
            <asp:Button ID="ClearButton" runat="server" Text="Clear" CssClass="btn btn-outline-secondary" CausesValidation="false" />
        </asp:Panel>

        <div class="table-responsive">
            <asp:GridView ID="TenantsGrid" runat="server" AutoGenerateColumns="false" DataKeyNames="TenantID"
                CssClass="table table-striped table-hover align-middle" GridLines="None" BorderWidth="0"
                EmptyDataText="No tenants found." EmptyDataRowStyle-CssClass="text-muted">
                <HeaderStyle CssClass="table-dark" />
                <Columns>
                    <asp:BoundField DataField="TenantID" HeaderText="Tenant ID" />
                    <asp:BoundField DataField="TenantName" HeaderText="Name" />
                    <asp:BoundField DataField="DatabaseName" HeaderText="Database" />
                    <asp:BoundField DataField="DatabaseServerName" HeaderText="Server" />
                    <asp:BoundField DataField="DatabaseUserId" HeaderText="User" />
                    <asp:TemplateField HeaderText="Password">
                        <ItemTemplate><span class="text-muted">&bull;&bull;&bull;&bull;&bull;&bull;&bull;&bull; (encrypted)</span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="text-nowrap text-end">
                        <ItemTemplate>
                            <a class="btn btn-sm btn-outline-primary" href="<%# ResolveUrl("~/Tenants/Edit") & "?id=" & Server.UrlEncode(Eval("TenantID").ToString()) %>">Edit</a>
                            <asp:LinkButton runat="server" CommandName="DeleteTenant" CommandArgument='<%# Eval("TenantID") %>'
                                CssClass="btn btn-sm btn-outline-danger" Text="Delete" CausesValidation="false"
                                OnClientClick="return confirm('Delete this tenant? This cannot be undone.');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
