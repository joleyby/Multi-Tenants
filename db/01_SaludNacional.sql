IF DB_ID(N'SaludNacional') IS NULL CREATE DATABASE SaludNacional;
GO
USE SaludNacional;
GO


IF OBJECT_ID(N'dbo.Provincias', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Provincias (
        Id            INT IDENTITY(1,1) CONSTRAINT PK_Provincias PRIMARY KEY,
        Nombre        NVARCHAR(100) NOT NULL,
        Codigo        NVARCHAR(10)  NOT NULL CONSTRAINT UQ_Provincias_Codigo UNIQUE,
        Estado        TINYINT       NOT NULL CONSTRAINT DF_Provincias_Estado DEFAULT 1,
        FechaCreacion DATETIME2     NOT NULL CONSTRAINT DF_Provincias_Fecha DEFAULT SYSDATETIME(),
        CONSTRAINT CK_Provincias_Estado CHECK (Estado IN (0, 1))
    );
END
GO

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios (
        Id              INT IDENTITY(1,1) CONSTRAINT PK_Usuarios PRIMARY KEY,
        NombreCompleto  NVARCHAR(150) NOT NULL,
        NombreUsuario   NVARCHAR(50)  NOT NULL CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE,
        HashContraseña  NVARCHAR(256) NOT NULL,
        Rol             TINYINT       NOT NULL,
        ProvinciaId     INT           NULL CONSTRAINT FK_Usuarios_Provincias REFERENCES dbo.Provincias(Id),
        Estado          TINYINT       NOT NULL CONSTRAINT DF_Usuarios_Estado DEFAULT 1,
        FechaCreacion   DATETIME2     NOT NULL CONSTRAINT DF_Usuarios_Fecha DEFAULT SYSDATETIME(),
        CONSTRAINT CK_Usuarios_Rol CHECK (Rol IN (1, 2, 3)),
        CONSTRAINT CK_Usuarios_Estado CHECK (Estado IN (0, 1)),
        CONSTRAINT CK_Usuarios_Provincia CHECK (
            (Rol = 1 AND ProvinciaId IS NULL) OR
            (Rol IN (2, 3) AND ProvinciaId IS NOT NULL))
    );
END
GO

IF OBJECT_ID(N'dbo.Tipos_Establecimientos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tipos_Establecimientos (
        Id            INT IDENTITY(1,1) CONSTRAINT PK_Tipos_Establecimientos PRIMARY KEY,
        Nombre        NVARCHAR(100) NOT NULL CONSTRAINT UQ_Tipos_Nombre UNIQUE,
        Descripcion   NVARCHAR(255) NULL,
        Estado        TINYINT       NOT NULL CONSTRAINT DF_Tipos_Estado DEFAULT 1,
        FechaCreacion DATETIME2     NOT NULL CONSTRAINT DF_Tipos_Fecha DEFAULT SYSDATETIME(),
        CONSTRAINT CK_Tipos_Estado CHECK (Estado IN (0, 1))
    );
END
GO

IF OBJECT_ID(N'dbo.Establecimientos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Establecimientos (
        Id                    INT IDENTITY(1,1) CONSTRAINT PK_Establecimientos PRIMARY KEY,
        ProvinciaId           INT           NOT NULL CONSTRAINT FK_Establecimientos_Provincias REFERENCES dbo.Provincias(Id),
        TipoEstablecimientoId INT           NOT NULL CONSTRAINT FK_Establecimientos_Tipos REFERENCES dbo.Tipos_Establecimientos(Id),
        Codigo                NVARCHAR(20)  NOT NULL,
        Nombre                NVARCHAR(150) NOT NULL,
        Direccion             NVARCHAR(255) NOT NULL,
        Telefono              NVARCHAR(30)  NULL,
        Correo                NVARCHAR(150) NULL,
        Estado                TINYINT       NOT NULL CONSTRAINT DF_Establecimientos_Estado DEFAULT 1,
        FechaCreacion         DATETIME2     NOT NULL CONSTRAINT DF_Establecimientos_Fecha DEFAULT SYSDATETIME(),
        FechaActualizacion    DATETIME2     NULL,
        CONSTRAINT CK_Establecimientos_Estado CHECK (Estado IN (0, 1)),
        CONSTRAINT UQ_Establecimientos_Provincia_Codigo UNIQUE (ProvinciaId, Codigo)
    );
END
GO

IF OBJECT_ID(N'dbo.Bitacora', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bitacora (
        Id            BIGINT IDENTITY(1,1) CONSTRAINT PK_Bitacora PRIMARY KEY,
        FechaHora     DATETIME2     NOT NULL CONSTRAINT DF_Bitacora_Fecha DEFAULT SYSDATETIME(),
        UsuarioId     INT           NULL,
        NombreUsuario NVARCHAR(50)  NOT NULL,
        ProvinciaId   INT           NULL CONSTRAINT FK_Bitacora_Provincias REFERENCES dbo.Provincias(Id),
        Operacion     TINYINT       NOT NULL CONSTRAINT CK_Bitacora_Operacion CHECK (Operacion BETWEEN 1 AND 5),
        Entidad       NVARCHAR(50)  NOT NULL,
        EntidadId     INT           NULL,
        Descripcion   NVARCHAR(500) NOT NULL,
        Exito         BIT           NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Bitacora_Provincia_Fecha')
    CREATE INDEX IX_Bitacora_Provincia_Fecha ON dbo.Bitacora (ProvinciaId, FechaHora DESC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Establecimientos_ProvinciaId')
    CREATE INDEX IX_Establecimientos_ProvinciaId
        ON dbo.Establecimientos (ProvinciaId)
        INCLUDE (Nombre, Codigo, Estado);
GO

/* ---------- Datos Pruebas ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Provincias WHERE Codigo = N'SPM')
    INSERT INTO dbo.Provincias (Nombre, Codigo) VALUES (N'San Pedro de Macorís', N'SPM');
IF NOT EXISTS (SELECT 1 FROM dbo.Provincias WHERE Codigo = N'SDO')
    INSERT INTO dbo.Provincias (Nombre, Codigo) VALUES (N'Santo Domingo', N'SDO');
IF NOT EXISTS (SELECT 1 FROM dbo.Provincias WHERE Codigo = N'STG')
    INSERT INTO dbo.Provincias (Nombre, Codigo) VALUES (N'Santiago', N'STG');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Tipos_Establecimientos WHERE Nombre = N'Hospital')
    INSERT INTO dbo.Tipos_Establecimientos (Nombre, Descripcion) VALUES (N'Hospital', N'Hospital general o especializado');
IF NOT EXISTS (SELECT 1 FROM dbo.Tipos_Establecimientos WHERE Nombre = N'Centro de salud')
    INSERT INTO dbo.Tipos_Establecimientos (Nombre, Descripcion) VALUES (N'Centro de salud', N'Centro de atención primaria');
IF NOT EXISTS (SELECT 1 FROM dbo.Tipos_Establecimientos WHERE Nombre = N'Clínica')
    INSERT INTO dbo.Tipos_Establecimientos (Nombre, Descripcion) VALUES (N'Clínica', N'Clínica de atención ambulatoria o especializada');
IF NOT EXISTS (SELECT 1 FROM dbo.Tipos_Establecimientos WHERE Nombre = N'Dispensario médico')
    INSERT INTO dbo.Tipos_Establecimientos (Nombre, Descripcion) VALUES (N'Dispensario médico', N'Puesto de atención básica');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'admin')
    INSERT INTO dbo.Usuarios (NombreCompleto, NombreUsuario, HashContraseña, Rol, ProvinciaId) VALUES
        (N'Administrador Nacional', N'admin',
         N'PBKDF2-SHA256$100000$lII6/DtWwV1bNSGtLsiWNQ==$rb+rU/3cUqv0U3x9yAf5lZU/RRKM4H6NIV6NYffdkMo=',
         1, NULL);
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'spm.admin')
    INSERT INTO dbo.Usuarios (NombreCompleto, NombreUsuario, HashContraseña, Rol, ProvinciaId) VALUES
        (N'Administrador San Pedro de Macorís', N'spm.admin',
         N'PBKDF2-SHA256$100000$YYfw22Of7QUTAMXFF/q5Bg==$hgYWR9etGBD/q2YSaGPziyZxi8ijDJjI65i+vTXanrM=',
         2, (SELECT Id FROM dbo.Provincias WHERE Codigo = N'SPM'));
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'spm.operador')
    INSERT INTO dbo.Usuarios (NombreCompleto, NombreUsuario, HashContraseña, Rol, ProvinciaId) VALUES
        (N'Operador San Pedro de Macorís', N'spm.operador',
         N'PBKDF2-SHA256$100000$iKZJx2bw7JJqM3fO0AGW/A==$XRWoNIFlMOk2MKLYx73e7qTCwbQVZS7zp7nTv/RmZvQ=',
         3, (SELECT Id FROM dbo.Provincias WHERE Codigo = N'SPM'));
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'sdo.operador')
    INSERT INTO dbo.Usuarios (NombreCompleto, NombreUsuario, HashContraseña, Rol, ProvinciaId) VALUES
        (N'Operador Santo Domingo', N'sdo.operador',
         N'PBKDF2-SHA256$100000$mOkI/FruNDtDtbNdX8yYJA==$QjaKuQhs0gMWeoDd9prMGe+e6f5jTHn7qjVLtFcHMsE=',
         3, (SELECT Id FROM dbo.Provincias WHERE Codigo = N'SDO'));
GO

