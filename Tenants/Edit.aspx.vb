Public Class Tenants_Edit
    Inherits Page

    Private ReadOnly _repo As New TenantRepository()

    Private ReadOnly Property EditingId As String
        Get
            Return Request.QueryString("id")
        End Get
    End Property

    Private ReadOnly Property IsEdit As Boolean
        Get
            Return Not String.IsNullOrEmpty(EditingId)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If IsPostBack Then Return

        If IsEdit Then
            Dim t As Tenant = Nothing
            Try
                t = _repo.GetById(EditingId)
            Catch ex As Exception
                ShowError("Could not load tenant: " & ex.Message)
            End Try
            If t Is Nothing Then
                If Not ErrorPanel.Visible Then ShowError("Tenant not found.")
                SaveButton.Enabled = False
                Return
            End If
            HeadingLiteral.Text = "Edit tenant"
            TenantIdBox.Text = t.TenantID
            TenantIdBox.ReadOnly = True
            TenantNameBox.Text = t.TenantName
            DatabaseNameBox.Text = t.DatabaseName
            DatabaseServerBox.Text = t.DatabaseServerName
            DatabaseUserBox.Text = t.DatabaseUserId
            PasswordRequired.Enabled = False
            PasswordHintLiteral.Text = " Leave blank to keep the current password."
        Else
            HeadingLiteral.Text = "Onboard new tenant"
        End If
    End Sub

    Protected Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If Not Page.IsValid Then Return

        Dim t As New Tenant With {
            .TenantID = If(IsEdit, EditingId, TenantIdBox.Text.Trim()),
            .TenantName = TenantNameBox.Text.Trim(),
            .DatabaseName = DatabaseNameBox.Text.Trim(),
            .DatabaseServerName = DatabaseServerBox.Text.Trim(),
            .DatabaseUserId = DatabaseUserBox.Text.Trim()
        }

        Try
            If IsEdit Then
                _repo.Update(t, DatabasePasswordBox.Text)
                Response.Redirect("~/Tenants?msg=" & Server.UrlEncode("Tenant updated."))
            Else
                _repo.Insert(t, DatabasePasswordBox.Text)
                Response.Redirect("~/Tenants?msg=" & Server.UrlEncode("Tenant onboarded."))
            End If
        Catch ex As DuplicateTenantException
            ShowError(ex.Message)
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ShowError("Could not save tenant: " & ex.Message)
        End Try
    End Sub

    Private Sub ShowError(text As String)
        ErrorPanel.Visible = True
        ErrorLiteral.Text = Server.HtmlEncode(text)
    End Sub
End Class
