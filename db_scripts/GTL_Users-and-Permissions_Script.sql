-- CREATE ROLES
CREATE ROLE GuestLibraries;
CREATE ROLE OwnerLibrary;

-- CREATE LOGINS
CREATE LOGIN guestLibrary WITH PASSWORD = 'guestpassword';
CREATE LOGIN gtl WITH PASSWORD = 'gtlpassword';

-- CREATE USERS FOR LOGIN (this can be for different employees, but is made for entire library for simplifications)
CREATE USER guestLibrary FOR LOGIN guestLibrary;
CREATE USER gtl FOR LOGIN gtl;

-- ADD USERS TO DIFFERENT ROLES
EXEC sp_addrolemember @rolename = 'GuestLibraries',
					  @memberName = 'guestLibrary';
EXEC sp_addrolemember @rolename = 'OwnerLibrary',
					  @memberName = 'gtl';

-- GRANT PERMISSIONS
GRANT SELECT ON top10activeMembers TO GuestLibraries;
GRANT SELECT ON top10books TO GuestLibraries;
GRANT SELECT ON personInfo TO GuestLibraries;
GRANT SELECT ON catalogOfBooks TO GuestLibraries;
