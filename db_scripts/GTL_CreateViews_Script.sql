/*
	This script is used to create the views needed for the GTL library system to function.
	The views has been made to abstract unneccessary data out. 
	This can be for security matters or simply to avoid showing data, that is not needed for the end user.

	This script should be run after the tables has been created.
*/

/*Fetch the most popular books, based on number of total loans*/
CREATE VIEW top10books AS
SELECT TOP 10
	m.title, m.author, m.subjectarea, 
	b.isbn, b.bookdescription,
	COUNT(lc.loanid) amountOfLoans
FROM Book AS b
INNER JOIN material AS m ON b.id = m.id
INNER JOIN copy AS c ON c.book_id = b.id
INNER JOIN loanCopy AS lc ON lc.copyBarcode = c.barcode
GROUP BY
	m.Title, m.Author, m.SubjectArea,
	b.isbn, b.bookdescription
ORDER BY
	amountOfLoans DESC;

/*Get the 10 most active members with at least 10 loans*/
CREATE VIEW top10activeMembers AS 
SELECT TOP 10
	p.card_id, p.firstName, p.lastName, p.isProfessor,
	COUNT(lc.loanId) AS amountOfLoans
FROM person AS p
INNER JOIN loan AS l ON l.card_id = p.card_id
INNER JOIN loanCopy AS lc ON lc.loanId = l.id
WHERE
	lc.returnDate > DATEADD( year, -1, GETDATE() )
GROUP BY
	p.card_id, p.firstName, p.lastName, p.isProfessor
HAVING
	COUNT(lc.loanId) >= 10
ORDER BY
	amountOfLoans DESC;

/*Fetch the catalog of books available, displaying only the needed information*/
CREATE VIEW catalogOfBooks AS
SELECT
	b.ISBN, b.bookDescription,
	m.title, m.author, m.subjectArea,
	c.isAvailable, c.barcode
FROM book AS b
INNER JOIN copy AS c ON c.book_id = b.id
INNER JOIN material AS m ON m.id = b.id;

/*Display Person Informations needed for Librarians*/
CREATE VIEW personInfo AS
SELECT card_id, firstname, lastname, isprofessor
FROM person