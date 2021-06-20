/*
	This script is used to create the triggers needed for the GTL library system to function.
*/

GO
	CREATE TRIGGER afterUpdateTrigger ON [loanCopy] FOR UPDATE
	AS
		DECLARE @loanId INT, 
				@copyBarcode INT,
				@returnDate DATE;

		SELECT @loanId = ins.loanId FROM INSERTED ins;
		SELECT @copyBarcode = ins.copyBarcode FROM INSERTED ins;
		SELECT @returnDate = ins.returnDate FROM INSERTED ins;

		UPDATE copy SET isAvailable = 1 WHERE barcode = @copyBarcode;

		DECLARE @numbersOfBooksStillToBeReturned INT;
		SELECT @numbersOfBooksStillToBeReturned = COUNT(*) FROM loanCopy lc
			INNER JOIN copy c ON c.barcode = lc.copyBarcode AND c.isAvailable = 0 WHERE lc.loanid = @loanId;

		if @numbersOfBooksStillToBeReturned < 1
			BEGIN
				UPDATE loan SET isActive = 0 WHERE id = @loanId;
				PRINT 'Updated Loan'
			END
GO