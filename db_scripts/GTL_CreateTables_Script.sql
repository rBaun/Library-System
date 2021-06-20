/*
	This script is used to create all the tables needed for the GTL library system to function.
	Note that this script does not insert dummy data, but sets the database structure up.
*/

CREATE TABLE card (
	id INT PRIMARY KEY IDENTITY(1,1),
	expires DATE NOT NULL,
	photoUrl VARCHAR(500) NOT NULL
);

CREATE TABLE person(
	ssn INT PRIMARY KEY,
	firstName VARCHAR(50) NOT NULL,
	lastName VARCHAR(100) NOT NULL,
	campusAddress VARCHAR(50) NOT NULL,
	email VARCHAR(100),
	homeAddress VARCHAR(50),
	isProfessor BIT NOT NULL,
	card_id INT FOREIGN KEY (card_id) REFERENCES card(id) ON DELETE CASCADE NOT NULL
);

CREATE TABLE phone(
	ssn INT NOT NULL,
	number INT NOT NULL,
	PRIMARY KEY(ssn, number),
	FOREIGN KEY (ssn) REFERENCES person(ssn) ON DELETE CASCADE
);

CREATE TABLE library(
	name VARCHAR(50) PRIMARY KEY
);

CREATE TABLE rules(
	name VARCHAR(50) NOT NULL,
	periodType INT NOT NULL,
	durationDays INT NOT NULL,
	PRIMARY KEY(name, periodType),
	FOREIGN KEY (name) REFERENCES library(name) ON DELETE CASCADE
);

CREATE TABLE librarian(
	employeeId INT PRIMARY KEY IDENTITY(1,1),
	library_name VARCHAR(50) FOREIGN KEY (library_name) REFERENCES library(name) NOT NULL
);

CREATE TABLE material(
	id INT PRIMARY KEY IDENTITY(1,1),
	author VARCHAR(100) NOT NULL,
	title VARCHAR(200) NOT NULL,
	subjectArea VARCHAR(200) NOT NULL
);

CREATE TABLE book(
	id INT PRIMARY KEY,
	isbn INT NULL,
	bookDescription VARCHAR(MAX) NOT NULL,
	FOREIGN KEY (id) REFERENCES material(id) ON DELETE CASCADE
);

CREATE TABLE rareMaterial(
	id INT PRIMARY KEY,
	rarity INT NOT NULL,
	FOREIGN KEY (id) REFERENCES material(id) ON DELETE CASCADE
);

CREATE TABLE loan(
	id INT PRIMARY KEY IDENTITY(1,1),
	startDate DATE NOT NULL,
	dueDate DATE NOT NULL,
	isActive BIT NOT NULL,
	librarian_id INT FOREIGN KEY (librarian_id) REFERENCES librarian(employeeId) NOT NULL,
	card_id INT FOREIGN KEY(card_id) REFERENCES card(id) NOT NULL
);

CREATE TABLE copy(
	barcode INT PRIMARY KEY IDENTITY(1,1),
	isAvailable BIT NOT NULL,
	book_id INT FOREIGN KEY (book_id) REFERENCES book(id) ON DELETE CASCADE NOT NULL
);

CREATE TABLE loanCopy(
	loanId INT NOT NULL,
	copyBarcode INT NOT NULL,
	returnDate DATE,
	PRIMARY KEY(loanId, copyBarcode),
	FOREIGN KEY (loanId) REFERENCES loan(id) ON DELETE CASCADE,
	FOREIGN KEY (copyBarcode) REFERENCES copy(barcode) ON DELETE CASCADE
);

CREATE TABLE notification(
	id INT PRIMARY KEY IDENTITY(1,1),
	sendDate DATE NOT NULL,
	notificationType INT NOT NULL,
	card_id INT FOREIGN KEY (card_id) REFERENCES card(id) ON DELETE CASCADE NOT NULL,
	loan_id INT FOREIGN KEY (loan_id) REFERENCES loan(id) ON DELETE CASCADE
);