Public Class Tenant
    Public Property TenantID As String
    Public Property TenantName As String
    Public Property DatabaseName As String
    Public Property DatabaseServerName As String
    Public Property DatabaseUserId As String
    ''' <summary>Encrypted (Base64) password as stored in the database.</summary>
    Public Property DatabasePassword As String
    Public Property CreatedUtc As DateTime
    Public Property UpdatedUtc As DateTime
End Class
