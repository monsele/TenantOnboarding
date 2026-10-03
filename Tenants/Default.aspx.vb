Public Class Tenants_Default
    Inherits Page

    Private ReadOnly _repo As New TenantRepository()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim msg As String = Request.QueryString("msg")
            If Not String.IsNullOrEmpty(msg) Then ShowMessage(msg, True)
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        Try
            TenantsGrid.DataSource = _repo.GetAll(SearchBox.Text)
            TenantsGrid.DataBind()
        Catch ex As Exception
            ShowMessage("Could not load tenants: " & ex.Message, False)
        End Try
    End Sub

    Private Sub ShowMessage(text As String, success As Boolean)
        MessagePanel.Visible = True
        MessagePanel.CssClass = If(success, "alert alert-success", "alert alert-danger")
        MessageLiteral.Text = Server.HtmlEncode(text)
    End Sub

    Protected Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        BindGrid()
    End Sub

    Protected Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        SearchBox.Text = String.Empty
        BindGrid()
    End Sub

    Protected Sub TenantsGrid_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles TenantsGrid.RowCommand
        If e.CommandName = "DeleteTenant" Then
            Try
                _repo.Delete(Convert.ToString(e.CommandArgument))
                ShowMessage("Tenant deleted.", True)
            Catch ex As Exception
                ShowMessage("Could not delete tenant: " & ex.Message, False)
            End Try
            BindGrid()
        End If
    End Sub
End Class
