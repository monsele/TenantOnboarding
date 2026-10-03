Imports System.Security.Cryptography

Namespace Security
    ''' <summary>
    ''' Reversible password protection shared by the Tenant Onboarding app (Encrypt) and
    ''' any consuming solution (Decrypt). Copy this single file into the other solution and
    ''' configure the SAME master key there.
    '''
    ''' Format: Base64( version(1) | IV(16) | AES-256-CBC ciphertext | HMAC-SHA256(32) )
    ''' Master key: Base64 of 32 random bytes, taken from the TENANT_ENCRYPTION_KEY environment
    ''' variable, falling back to the "EncryptionKey" appSetting.
    ''' Generate a key:  [Convert]::ToBase64String((1..32 | % { Get-Random -Max 256 }) -as [byte[]])
    ''' </summary>
    Public NotInheritable Class PasswordCryptor
        Private Const FormatVersion As Byte = 1
        Private Const IvLength As Integer = 16
        Private Const MacLength As Integer = 32
        Private Const KeyEnvVar As String = "TENANT_ENCRYPTION_KEY"
        Private Const KeySettingName As String = "EncryptionKey"

        Private Sub New()
        End Sub

        Public Shared Function Encrypt(plainText As String) As String
            If plainText Is Nothing Then Throw New ArgumentNullException("plainText")

            Dim encKey As Byte() = Nothing
            Dim macKey As Byte() = Nothing
            DeriveKeys(encKey, macKey)

            Using aes As Aes = Aes.Create()
                aes.KeySize = 256
                aes.Mode = CipherMode.CBC
                aes.Padding = PaddingMode.PKCS7
                aes.Key = encKey
                aes.GenerateIV()

                Dim cipher As Byte()
                Using enc As ICryptoTransform = aes.CreateEncryptor()
                    Dim data As Byte() = Encoding.UTF8.GetBytes(plainText)
                    cipher = enc.TransformFinalBlock(data, 0, data.Length)
                End Using

                Dim payload(1 + IvLength + cipher.Length - 1) As Byte
                payload(0) = FormatVersion
                Buffer.BlockCopy(aes.IV, 0, payload, 1, IvLength)
                Buffer.BlockCopy(cipher, 0, payload, 1 + IvLength, cipher.Length)

                Dim mac As Byte() = ComputeMac(macKey, payload, payload.Length)
                Dim result(payload.Length + MacLength - 1) As Byte
                Buffer.BlockCopy(payload, 0, result, 0, payload.Length)
                Buffer.BlockCopy(mac, 0, result, payload.Length, MacLength)
                Return Convert.ToBase64String(result)
            End Using
        End Function

        Public Shared Function Decrypt(cipherText As String) As String
            If String.IsNullOrEmpty(cipherText) Then Throw New ArgumentNullException("cipherText")

            Dim all As Byte()
            Try
                all = Convert.FromBase64String(cipherText)
            Catch ex As FormatException
                Throw New CryptographicException("Cipher text is not valid Base64.", ex)
            End Try

            If all.Length < 1 + IvLength + 16 + MacLength OrElse all(0) <> FormatVersion Then
                Throw New CryptographicException("Cipher text has an unrecognised format.")
            End If

            Dim encKey As Byte() = Nothing
            Dim macKey As Byte() = Nothing
            DeriveKeys(encKey, macKey)

            Dim payloadLength As Integer = all.Length - MacLength
            Dim expected As Byte() = ComputeMac(macKey, all, payloadLength)
            Dim actual(MacLength - 1) As Byte
            Buffer.BlockCopy(all, payloadLength, actual, 0, MacLength)
            If Not ConstantTimeEquals(expected, actual) Then
                Throw New CryptographicException("Cipher text failed integrity check (wrong key or tampered data).")
            End If

            Dim iv(IvLength - 1) As Byte
            Buffer.BlockCopy(all, 1, iv, 0, IvLength)
            Dim cipherLength As Integer = payloadLength - 1 - IvLength

            Using aes As Aes = Aes.Create()
                aes.KeySize = 256
                aes.Mode = CipherMode.CBC
                aes.Padding = PaddingMode.PKCS7
                aes.Key = encKey
                aes.IV = iv
                Using dec As ICryptoTransform = aes.CreateDecryptor()
                    Dim plain As Byte() = dec.TransformFinalBlock(all, 1 + IvLength, cipherLength)
                    Return Encoding.UTF8.GetString(plain)
                End Using
            End Using
        End Function

        Private Shared Sub DeriveKeys(ByRef encKey As Byte(), ByRef macKey As Byte())
            Dim master As Byte() = LoadMasterKey()
            Using h As New HMACSHA256(master)
                encKey = h.ComputeHash(Encoding.UTF8.GetBytes("enc"))
                macKey = h.ComputeHash(Encoding.UTF8.GetBytes("mac"))
            End Using
        End Sub

        Private Shared Function LoadMasterKey() As Byte()
            Dim b64 As String = Environment.GetEnvironmentVariable(KeyEnvVar)
            If String.IsNullOrWhiteSpace(b64) Then
                b64 = System.Configuration.ConfigurationManager.AppSettings(KeySettingName)
            End If
            If String.IsNullOrWhiteSpace(b64) Then
                Throw New InvalidOperationException("Encryption key not configured. Set the " & KeyEnvVar &
                    " environment variable or the '" & KeySettingName & "' appSetting (Base64, 32 bytes).")
            End If

            Dim key As Byte()
            Try
                key = Convert.FromBase64String(b64.Trim())
            Catch ex As FormatException
                Throw New InvalidOperationException("Encryption key must be Base64 encoded.", ex)
            End Try
            If key.Length < 32 Then
                Throw New InvalidOperationException("Encryption key must be at least 32 bytes (256 bits).")
            End If
            Return key
        End Function

        Private Shared Function ComputeMac(macKey As Byte(), data As Byte(), count As Integer) As Byte()
            Using h As New HMACSHA256(macKey)
                Return h.ComputeHash(data, 0, count)
            End Using
        End Function

        Private Shared Function ConstantTimeEquals(a As Byte(), b As Byte()) As Boolean
            If a.Length <> b.Length Then Return False
            Dim diff As Integer = 0
            For i As Integer = 0 To a.Length - 1
                diff = diff Or (a(i) Xor b(i))
            Next
            Return diff = 0
        End Function
    End Class
End Namespace
