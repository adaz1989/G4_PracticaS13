
CREATE PROCEDURE SP_ObtenerCompras
AS
BEGIN	
	SELECT Id_Compra AS IdCompra, Descripcion, Precio, Saldo, Estado
	FROM dbo.Principal
	ORDER BY 
		CASE Estado 
				WHEN 'Pendiente' THEN 0
				WHEN 'Cancelado' THEN 1
		END
END
GO

----------------------------------------------------------------------------------------------
CREATE PROCEDURE SP_ObtenerComprasPendientes
AS
BEGIN
	SELECT Id_Compra AS IdCompra, Descripcion, Precio, Saldo, Estado
	FROM dbo.Principal
	WHERE Estado = 'Pendiente'
END 
GO

----------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE SP_RegistrarAbono
	@IdCompra BIGINT,
	@Monto DECIMAL(11,5),
	@CodigoError TINYINT OUT,
	@Mensaje VARCHAR(100) OUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @SaldoActual DECIMAL(10,5)

	-- Obtener el saldo actual
	SELECT @SaldoActual = Saldo 
	FROM dbo.Principal 
	WHERE Id_Compra = @IdCompra

	-- Validar que la compra exista
	IF @SaldoActual IS NULL
	BEGIN
		SET @CodigoError = 1
		SET @Mensaje = 'Compra no encontrada'
		RETURN
	END

	-- Validar que el abono no sea mayor al saldo
	IF @Monto > @SaldoActual
	BEGIN
		SET @CodigoError = 1
		SET @Mensaje = 'El abono no puede ser mayor al saldo actual'
		RETURN
	END

	BEGIN TRY
		BEGIN TRANSACTION

		-- Insertar el abono con la fecha actual del servidor
		INSERT INTO dbo.Abonos (Id_Compra, Monto, Fecha)
		VALUES (@IdCompra, @Monto, GETDATE())

		-- Actualizar el saldo
		UPDATE dbo.Principal
		SET Saldo = Saldo - @Monto
		WHERE Id_Compra = @IdCompra

		-- Si el nuevo saldo es cero, cambiar estado a Cancelado
		IF @SaldoActual = @Monto
		BEGIN
			UPDATE dbo.Principal
			SET Estado = 'Cancelado'
			WHERE Id_Compra = @IdCompra
		END

		COMMIT TRANSACTION

		SET @CodigoError = 0
		SET @Mensaje = 'Abono registrado con éxito'
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @CodigoError = 2
		SET @Mensaje = 'Error al registrar el abono'
	END CATCH
END
GO


-----------------------------------------------------------
CREATE OR ALTER PROCEDURE SP_ObtenerSaldoCompra
	@IdCompra BIGINT
AS
BEGIN
	SELECT Saldo
	FROM dbo.Principal
	WHERE Id_Compra = @IdCompra
END
GO

