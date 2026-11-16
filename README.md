# Salud Nacional - Multi-Tenant por provincia

Solución .NET 10 (WinForms + SQL Server) para centralizar los establecimientos de salud por provincia.

## Puesta en marcha
1. Ejecutar `db/01_SaludNacional.sql` en SQL Server (crea la base `SaludNacional` y datos semilla).
2. Ajustar la cadena de conexión en `Presentacion/appsettings.json` si su servidor no es `localhost`.
3. Abrir `Multi-Tenants.slnx` en Visual Studio o usar `dotnet build`.
4. Ejecutar el proyecto `Presentacion`.

## Cuentas de prueba (contraseña: Pruebas@)
| Usuario | Rol | Acceso |
|---|---|---|
| admin | Administrador nacional | Consulta de todas las provincias (solo lectura) |
| spm.admin | Administrador provincial | San Pedro de Macorís |
| spm.operador | Operador provincial | San Pedro de Macorís |
| sdo.operador | Operador provincial | Santo Domingo |

Cambie las contraseñas antes de usar el sistema con datos reales.

## Arquitectura
- **Entidades**: modelos de dominio, enums, `UsuarioContexto` y `TenantContexto` (aislamiento por provincia).
- **Seguridad**: hash de contraseñas PBKDF2-SHA256.
- **Datos**: repositorios ADO.NET (Microsoft.Data.SqlClient). Cada consulta de provincia incluye el filtro `ProvinciaId`.
- **Negocio**: servicios con reglas de negocio y validaciones.
- **TransferenciaDatos**: DTOs entre capas.
- **Presentacion**: formularios WinForms con diseñador (`*.Designer.cs`).
- **Pruebas.Unitarias**: xUnit con repositorios falsos.

