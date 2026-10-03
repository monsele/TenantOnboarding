# PasswordCryptor – sharing with the consuming solution

1. Copy `PasswordCryptor.vb` into the other solution (.NET Framework 4.x; self-contained, no other dependencies).
2. Configure the **same** master key there: env var `TENANT_ENCRYPTION_KEY` (preferred) or appSetting `EncryptionKey`.
   Generate one (PowerShell): `[Convert]::ToBase64String((1..32 | % { Get-Random -Max 256 }) -as [byte[]])`
3. Read the `DatabasePassword` column and call:

```vb
Dim plain As String = Security.PasswordCryptor.Decrypt(row("DatabasePassword").ToString())
```

Algorithm: AES-256-CBC, random IV per encryption, HMAC-SHA256 (encrypt-then-MAC). Wrong key or tampered data throws `CryptographicException`.
Never commit the production key; the dev key in Web.config is for local use only.
