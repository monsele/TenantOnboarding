IF OBJECT_ID(N'dbo.Tenants', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tenants (
        TenantID           NVARCHAR(50)  NOT NULL CONSTRAINT PK_Tenants PRIMARY KEY,
        TenantName         NVARCHAR(200) NOT NULL,
        DatabaseName       NVARCHAR(128) NOT NULL,
        DatabaseServerName NVARCHAR(255) NOT NULL,
        DatabaseUserId     NVARCHAR(128) NOT NULL,
        DatabasePassword   NVARCHAR(MAX) NOT NULL, -- AES-256 ciphertext (Base64), see Security/PasswordCryptor.vb
        CreatedUtc         DATETIME2     NOT NULL CONSTRAINT DF_Tenants_Created DEFAULT SYSUTCDATETIME(),
        UpdatedUtc         DATETIME2     NOT NULL CONSTRAINT DF_Tenants_Updated DEFAULT SYSUTCDATETIME()
    );
END
