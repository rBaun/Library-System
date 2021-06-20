/*
	This script is used to create the stored procedures needed for the GTL library system to function.
	The stored procedures made for this project is going to send out notifications to those who did not return the loaned item.
	The rules enforced is based on the library protocol for sending out notifications and reminders.
*/

/*Finds the relevant loans to give notifications or the cards about to expire. Executed every night*/
CREATE PROCEDURE createLoanNotification @CurrentDate DATE, @isProfessor BIT, @periodType INT
AS
	DECLARE @cardId INT
	DECLARE @loanId INT
	DECLARE db_cursor CURSOR FOR
		SELECT l.id, p.card_id FROM loan l
		INNER JOIN person p ON p.card_id = l.card_id AND p.isProfessor = @isProfessor
		INNER JOIN librarian li ON li.employeeId = l.librarian_id
		INNER JOIN library lib ON lib.name = li.library_name
		INNER JOIN rules r ON r.name = lib.name AND r.periodType = @periodType
		WHERE l.dueDate = DATEADD(DAY, -r.durationDays, @currentDate) AND l.isActive = 1

			OPEN db_cursor
				FETCH NEXT FROM db_cursor INTO @loanId, @cardId

				WHILE @@FETCH_STATUS = 0
					BEGIN
						INSERT INTO notification(sendDate, notificationType, loan_id, card_id) VALUES (@CurrentDate, 2, @loanId, @cardId);
						FETCH NEXT FROM db_cursor INTO @loanId, @cardId
					END
			CLOSE db_cursor
			DEALLOCATE db_cursor
GO

/*Creates all the notifications using the createLoanNotification procedure from before*/
CREATE PROCEDURE createAllNotifications AS
	DECLARE @currentDate date = CAST(GETDATE() AS date)
	DECLARE @cardId INT
	DECLARE db_cursor CURSOR FOR
		SELECT id FROM card WHERE expires = DATEADD(MONTH, 1, @currentDate)

			OPEN db_cursor
				FETCH NEXT FROM db_cursor INTO @cardId

				WHILE @@FETCH_STATUS = 0
					BEGIN
						INSERT INTO notification(sendDate, notificationType, card_id) VALUES (@currentDate, 1, @cardId)
						FETCH NEXT FROM db_cursor INTO @cardId
					END
			CLOSE db_cursor
			DEALLOCATE db_cursor

	EXEC CreateLoanNotification @CurrentDate = @currentDate, @isProfessor = 0, @periodType = 2;
	EXEC CreateLoanNotification @CurrentDate = @currentDate, @isProfessor = 1, @periodType = 4;
GO