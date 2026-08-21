CREATE DATABASE Agenda;
go 
USE Agenda;

CREATE TABLE Contactos
(
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(150) NOT NULL UNIQUE,
    Telefono VARCHAR(20) NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
 

    fecha_creacion DATETIME DEFAULT GETDATE(),
    usuario_creacion VARCHAR(100) NOT NULL,
    fecha_modificacion DATETIME NULL,
    usuario_modificacion VARCHAR(100) NULL
);
GO

CREATE PROCEDURE sp_InsertarContacto
    @Nombre VARCHAR(100),
    @Correo VARCHAR(150),
    @Telefono VARCHAR(20) = NULL,
    @usuario_creacion VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
 
    BEGIN TRY
 
        -- Validar que el nombre no esté vacío
        IF @Nombre IS NULL OR LTRIM(RTRIM(@Nombre)) = ''
        BEGIN
            THROW 50001, 'El nombre es obligatorio.', 1;
        END;
 
        -- Validar que el correo no esté vacío
        IF @Correo IS NULL OR LTRIM(RTRIM(@Correo)) = ''
        BEGIN
            THROW 50002, 'El correo es obligatorio.', 1;
        END;
 
        -- Validar que el correo no esté duplicado
        IF EXISTS (
            SELECT 1
            FROM Contactos
            WHERE Correo = @Correo
        )
        BEGIN
            THROW 50003, 'El correo ya se encuentra registrado.', 1;
        END;
 
        -- Insertar el contacto
        INSERT INTO Contactos
        (
            Nombre,
            Correo,
            Telefono,
            usuario_creacion
        )
        VALUES
        (
            @Nombre,
            @Correo,
            @Telefono,
            @usuario_creacion
        );
 
        -- Retornar el ID creado
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS ID;
 
    END TRY
 
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

------Ejemplo
 
EXEC sp_InsertarContacto
    @Nombre = 'Paula Garbanzo',
    @Correo = 'paula@gmail.com',
    @Telefono = '88888888',
    @usuario_creacion = 'Paula';

	SELECT * FROM Contactos;

	