Imports System.Data.SqlClient

Public Class DuplicateTenantException
    Inherits Exception
    Public Sub New(tenantId As String)
        MyBase.New("A tenant with ID '" & tenantId & "' already exists.")
    End Sub
End Class

Public Class TenantRepository
    Private Shared ReadOnly ConnectionString As String =
        ConfigurationManager.ConnectionStrings("TenantDb").ConnectionString

    Private Const SelectColumns As String =
        "TenantID, TenantName, DatabaseName, DatabaseServerName, DatabaseUserId, DatabasePassword, CreatedUtc, UpdatedUtc"

    Public Function GetAll(Optional search As String = Nothing) As List(Of Tenant)
        Dim list As New List(Of Tenant)
        Using cn As New SqlConnection(ConnectionString)
            Using cmd As New SqlCommand()
                cmd.Connection = cn
                Dim sql As String = "SELECT " & SelectColumns & " FROM dbo.Tenants"
                If Not String.IsNullOrWhiteSpace(search) Then
                    sql &= " WHERE TenantID LIKE @s ESCAPE '\' OR TenantName LIKE @s ESCAPE '\'" &
                           " OR DatabaseName LIKE @s ESCAPE '\' OR DatabaseServerName LIKE @s ESCAPE '\'"
                    cmd.Parameters.Add("@s", SqlDbType.NVarChar, 400).Value = "%" & EscapeLike(search.Trim()) & "%"
                End If
                cmd.CommandText = sql & " ORDER BY TenantName"
                cn.Open()
                Using r As SqlDataReader = cmd.ExecuteReader()
                    While r.Read()
                        list.Add(Map(r))
                    End While
                End Using
            End Using
        End Using
        Return list
    End Function

    Public Function GetById(tenantId As String) As Tenant
        Using cn As New SqlConnection(ConnectionString)
            Using cmd As New SqlCommand("SELECT " & SelectColumns & " FROM dbo.Tenants WHERE TenantID = @id", cn)
                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50).Value = tenantId
                cn.Open()
                Using r As SqlDataReader = cmd.ExecuteReader()
                    If r.Read() Then Return Map(r)
                End Using
            End Using
        End Using
        Return Nothing
    End Function

    ''' <summary>Inserts a tenant; plainPassword is encrypted before storage.</summary>
    Public Sub Insert(t As Tenant, plainPassword As String)
        Using cn As New SqlConnection(ConnectionString)
            Using cmd As New SqlCommand(
                "INSERT INTO dbo.Tenants (TenantID, TenantName, DatabaseName, DatabaseServerName, DatabaseUserId, DatabasePassword) " &
                "VALUES (@id, @name, @db, @server, @user, @pwd)", cn)
                AddCommonParameters(cmd, t)
                cmd.Parameters.Add("@pwd", SqlDbType.NVarChar, -1).Value = Security.PasswordCryptor.Encrypt(plainPassword)
                cn.Open()
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As SqlException When ex.Number = 2627 OrElse ex.Number = 2601
                    Throw New DuplicateTenantException(t.TenantID)
                End Try
            End Using
        End Using
    End Sub

    ''' <summary>Updates a tenant. A blank newPlainPassword keeps the stored password.</summary>
    Public Sub Update(t As Tenant, newPlainPassword As String)
        Dim setPwd As Boolean = Not String.IsNullOrEmpty(newPlainPassword)
        Using cn As New SqlConnection(ConnectionString)
            Using cmd As New SqlCommand(
                "UPDATE dbo.Tenants SET TenantName=@name, DatabaseName=@db, DatabaseServerName=@server, DatabaseUserId=@user, " &
                If(setPwd, "DatabasePassword=@pwd, ", "") & "UpdatedUtc=SYSUTCDATETIME() WHERE TenantID=@id", cn)
                AddCommonParameters(cmd, t)
                If setPwd Then
                    cmd.Parameters.Add("@pwd", SqlDbType.NVarChar, -1).Value = Security.PasswordCryptor.Encrypt(newPlainPassword)
                End If
                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Delete(tenantId As String)
        Using cn As New SqlConnection(ConnectionString)
            Using cmd As New SqlCommand("DELETE FROM dbo.Tenants WHERE TenantID = @id", cn)
                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50).Value = tenantId
                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Shared Sub AddCommonParameters(cmd As SqlCommand, t As Tenant)
        cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50).Value = t.TenantID
        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = t.TenantName
        cmd.Parameters.Add("@db", SqlDbType.NVarChar, 128).Value = t.DatabaseName
        cmd.Parameters.Add("@server", SqlDbType.NVarChar, 255).Value = t.DatabaseServerName
        cmd.Parameters.Add("@user", SqlDbType.NVarChar, 128).Value = t.DatabaseUserId
    End Sub

    Private Shared Function Map(r As SqlDataReader) As Tenant
        Return New Tenant With {
            .TenantID = r.GetString(0),
            .TenantName = r.GetString(1),
            .DatabaseName = r.GetString(2),
            .DatabaseServerName = r.GetString(3),
            .DatabaseUserId = r.GetString(4),
            .DatabasePassword = r.GetString(5),
            .CreatedUtc = r.GetDateTime(6),
            .UpdatedUtc = r.GetDateTime(7)
        }
    End Function

    Private Shared Function EscapeLike(s As String) As String
        Return s.Replace("\", "\\").Replace("%", "\%").Replace("_", "\_").Replace("[", "\[")
    End Function
End Class
