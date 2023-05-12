USE [master];
GO 

ALTER DATABASE [AuthDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO 

DROP DATABASE [AuthDB];
GO

CREATE DATABASE [AuthDB];
GO

USE [AuthDB];
GO

ALTER DATABASE [AuthDB] SET TRUSTWORTHY ON
GO

CREATE TABLE Auth (
	auth_id nvarchar(max),
	auth_token nvarchar(max),
  -- url nvarchar(max)
	)

INSERT INTO Auth VALUES('auth_id', 'auth_token' /*, ''*/))

SELECT * FROM Auth
