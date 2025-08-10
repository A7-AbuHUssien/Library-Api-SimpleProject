-- Drop the database if it already exists to allow for a clean re-creation
IF DB_ID('LibraryManagementDb') IS NOT NULL
BEGIN
    ALTER DATABASE LibraryManagementDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE LibraryManagementDb;
END
GO

-- Create the new database
CREATE DATABASE LibraryManagementDb;
GO

-- Use the newly created database
USE LibraryManagementDb;
GO

-- Create Tables

-- Authors Table
CREATE TABLE Authors (
    AuthorID BIGINT IDENTITY(1,1) PRIMARY KEY,
    AuthorName VARCHAR(100) NOT NULL UNIQUE
);
GO

-- Categories Table
CREATE TABLE Categories (
    CategoryID BIGINT IDENTITY(1,1) PRIMARY KEY,
    CategoryName VARCHAR(100) NOT NULL UNIQUE
);
GO

-- Books Table
CREATE TABLE Books (
    BookID BIGINT IDENTITY(1,1) PRIMARY KEY,
    BookTitle VARCHAR(255) NOT NULL,
    AuthorID BIGINT NOT NULL,
    CategoryID BIGINT NOT NULL,
    PublicationYear DATE,
    IsFree BIT NOT NULL DEFAULT 1, -- Added IsFree column as discussed
    FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);
GO

-- Clients Table
CREATE TABLE Clients (
    ClientID BIGINT IDENTITY(1,1) PRIMARY KEY,
    ClientName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    Phone VARCHAR(20) UNIQUE,
    RegistrationDate DATE NOT NULL DEFAULT GETDATE()
);
GO

-- Borrowing Table
CREATE TABLE Borrowing (
    BorrowingID BIGINT IDENTITY(1,1) PRIMARY KEY,
    ClientID BIGINT NOT NULL,
    BookID BIGINT NOT NULL,
    BorrowDate DATE NOT NULL,
    ReturnDate DATE,
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID)
);
GO

CREATE FUNCTION IsBookExists
(
    @BookID BIGINT 
)
RETURNS BIT
AS
BEGIN
    DECLARE @Exists BIT;
    IF EXISTS (SELECT 1 FROM Books WHERE BookID = @BookID)
        SET @Exists = 1;
    ELSE
        SET @Exists = 0;
    RETURN @Exists;
END;
GO

CREATE FUNCTION IsClientExistsByID
(
    @ClientID BIGINT 
)
RETURNS BIT
AS
BEGIN
    DECLARE @Exists BIT;
    IF EXISTS (SELECT 1 FROM Clients WHERE ClientID = @ClientID)
        SET @Exists = 1;
    ELSE
        SET @Exists = 0;
    RETURN @Exists;
END;
GO

CREATE FUNCTION IsClientExistsByPhoneOrEmail
(
    @Phone VARCHAR(20), 
    @Email VARCHAR(100), 
    @IgnoreID BIGINT = NULL 
)
RETURNS BIT
AS
BEGIN
    DECLARE @Exists BIT;
    IF EXISTS (
        SELECT 1
        FROM Clients
        WHERE (Phone = @Phone OR Email = @Email)
          AND (@IgnoreID IS NULL OR ClientID != @IgnoreID)
    )
        SET @Exists = 1;
    ELSE
        SET @Exists = 0;
    RETURN @Exists;
END;
GO

CREATE PROCEDURE SP_GetAllBooks
AS
BEGIN
    SELECT b.BookID, b.BookTitle, a.AuthorName, c.CategoryName, b.PublicationYear, b.IsFree
    FROM Books b
    JOIN Authors a ON b.AuthorID = a.AuthorID
    JOIN Categories c ON c.CategoryID = b.CategoryID;
END;
GO

CREATE PROCEDURE SP_GetBookByID
    @ID BIGINT 
AS
BEGIN
    IF (dbo.IsBookExists(@ID) = 0)
    BEGIN
        RETURN 0;
    END

    BEGIN TRY
        SELECT b.BookID, b.BookTitle, a.AuthorName, c.CategoryName, b.PublicationYear, b.IsFree
        FROM Books b
        JOIN Authors a ON b.AuthorID = a.AuthorID
        JOIN Categories c ON c.CategoryID = b.CategoryID
        WHERE b.BookID = @ID;
    END TRY
    BEGIN CATCH
        RETURN -1;
    END CATCH
END;
GO

CREATE PROCEDURE SP_AddBook
    @Title VARCHAR(255), 
    @AuthorName VARCHAR(100),
    @CategoryName VARCHAR(100),
    @PublicationYear DATE,
    @NewBookID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IDOfAuthor BIGINT, @IDOfCategory BIGINT;
    BEGIN TRY
        SELECT @IDOfAuthor = AuthorID
        FROM Authors
        WHERE AuthorName = @AuthorName;
        IF (@IDOfAuthor IS NULL)
        BEGIN
            INSERT INTO Authors (AuthorName)
            VALUES (@AuthorName);
            SET @IDOfAuthor = SCOPE_IDENTITY();
        END;

        SELECT @IDOfCategory = CategoryID
        FROM Categories
        WHERE CategoryName = @CategoryName;
        IF (@IDOfCategory IS NULL)
        BEGIN
            INSERT INTO Categories (CategoryName)
            VALUES (@CategoryName);
            SET @IDOfCategory = SCOPE_IDENTITY();
        END;

        INSERT INTO Books (BookTitle, AuthorID, CategoryID, PublicationYear)
        VALUES (@Title, @IDOfAuthor, @IDOfCategory, @PublicationYear);
        SET @NewBookID = SCOPE_IDENTITY();

    END TRY
    BEGIN CATCH
        SET @NewBookID = -1;
    END CATCH
END;
GO

CREATE PROCEDURE SP_UpdateBook
    @ID BIGINT,
    @Title VARCHAR(255),
    @AuthorID BIGINT,
    @CategoryID BIGINT,
    @PublishYear DATE
AS
BEGIN
    SET NOCOUNT ON;
    IF (dbo.IsBookExists(@ID) = 0)
    BEGIN
        RETURN 0;
    END

    BEGIN TRY
        UPDATE Books
        SET BookTitle = @Title,
            AuthorID = @AuthorID,
            CategoryID = @CategoryID,
            PublicationYear = @PublishYear
        WHERE BookID = @ID;
    END TRY
    BEGIN CATCH
        RETURN -1;
    END CATCH
END;
GO

CREATE PROCEDURE SP_DeleteBook
    @ID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    IF (dbo.IsBookExists(@ID) = 0)
    BEGIN
        RETURN 0;
    END

    IF EXISTS (
        SELECT 1
        FROM Borrowing
        WHERE BookID = @ID
    )
    BEGIN
        RETURN -2; 
    END

    BEGIN TRY
        DELETE FROM Books
        WHERE BookID = @ID;

        IF @@ROWCOUNT = 0
        BEGIN
            RETURN 0;
        END
        ELSE
        BEGIN
            RETURN 1;
        END
    END TRY
    BEGIN CATCH
        RETURN -1;
    END CATCH
END;
GO


CREATE PROCEDURE SP_AddAuthor
    @AuthorName VARCHAR(100) 
AS
BEGIN
    SET NOCOUNT ON; 
    IF EXISTS (
        SELECT 1
        FROM Authors
        WHERE AuthorName = @AuthorName
    )
    BEGIN
        RETURN -1;
    END
    BEGIN TRY
        INSERT INTO Authors(AuthorName) VALUES(@AuthorName);
        RETURN SCOPE_IDENTITY(); 
    END TRY
    BEGIN CATCH
        RETURN -2; 
    END CATCH
END;
GO

CREATE PROCEDURE SP_DeleteAuthor
    @ID BIGINT
AS
BEGIN
    SET NOCOUNT ON; 
    IF EXISTS (
        SELECT 1
        FROM Books
        WHERE AuthorID = @ID
    )
    BEGIN
        RETURN -1; 
    END
    ELSE
    BEGIN
        BEGIN TRY 
            DELETE FROM Authors WHERE AuthorID = @ID;
            IF @@ROWCOUNT = 0 RETURN 0; 
            RETURN 1; 
        END TRY
        BEGIN CATCH
            RETURN -2; 
        END CATCH
    END
END;
GO

CREATE PROCEDURE SP_GetAuthors
AS
BEGIN
    SELECT a.AuthorID, a.AuthorName, b.BookTitle
    FROM Authors a
    LEFT JOIN Books b ON a.AuthorID = b.AuthorID;
END;
GO

CREATE PROCEDURE SP_GetCategories
AS
BEGIN
    SELECT * FROM Categories;
END;
GO

CREATE PROCEDURE SP_AddCategory
    @CategoryName VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON; 
    IF EXISTS (
        SELECT 1 FROM Categories WHERE CategoryName = @CategoryName
    )
    BEGIN
        RETURN -2; 
    END

    BEGIN TRY
        INSERT INTO Categories(CategoryName)
        VALUES (@CategoryName);
        RETURN SCOPE_IDENTITY(); 
    END TRY
    BEGIN CATCH
        RETURN -1; 
    END CATCH
END;
GO

CREATE PROCEDURE SP_DeleteCategory
    @CategoryID BIGINT 
AS
BEGIN
    SET NOCOUNT ON; 
    IF EXISTS (
        SELECT 1 FROM Books WHERE CategoryID = @CategoryID
    )
    BEGIN
        RETURN -1; 
    END
    ELSE
    BEGIN
        BEGIN TRY 
            DELETE FROM Categories WHERE CategoryID = @CategoryID;
            IF @@ROWCOUNT = 0 RETURN 0; 
            RETURN 1; 
        END TRY
        BEGIN CATCH
            RETURN -2; 
        END CATCH
    END
END;
GO

CREATE PROCEDURE SP_AddClient
    @ClientName VARCHAR(100),
    @Phone VARCHAR(20), 
    @Email VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON; 
    BEGIN TRY
        IF dbo.IsClientExistsByPhoneOrEmail(@Phone, @Email, NULL) = 1
            RETURN -1; 

        INSERT INTO Clients (ClientName, Phone, Email)
        VALUES (@ClientName, @Phone, @Email);

        RETURN SCOPE_IDENTITY(); 
    END TRY
    BEGIN CATCH
        RETURN -2; 
    END CATCH
END;
GO

CREATE PROCEDURE SP_GetAllClients
AS
BEGIN
    SELECT * FROM Clients;
END;
GO

CREATE PROCEDURE SP_GetClientByID
    @ClientID BIGINT 
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.IsClientExistsByID(@ClientID) = 0
        RETURN -1; 

    SELECT * FROM Clients WHERE ClientID = @ClientID;
END;
GO

CREATE PROCEDURE SP_UpdateClient
    @ClientID BIGINT, 
    @ClientName VARCHAR(100),
    @Phone VARCHAR(20), 
    @Email VARCHAR(100) 
AS
BEGIN
    SET NOCOUNT ON; 
    BEGIN TRY
        IF dbo.IsClientExistsByID(@ClientID) = 0
            RETURN -1; 

        IF dbo.IsClientExistsByPhoneOrEmail(@Phone, @Email, @ClientID) = 1
            RETURN -2;

        UPDATE Clients
        SET ClientName = @ClientName,
            Phone = @Phone,
            Email = @Email
        WHERE ClientID = @ClientID;

        RETURN 1; 
    END TRY
    BEGIN CATCH
        RETURN -3; 
    END CATCH
END;
GO

CREATE PROCEDURE SP_DeleteClient
    @ClientID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF dbo.IsClientExistsByID(@ClientID) = 0
            RETURN -1;

        IF EXISTS (
            SELECT 1
            FROM Borrowing
            WHERE ClientID = @ClientID
        )
        BEGIN
            RETURN -2; 
        END

        DELETE FROM Clients WHERE ClientID = @ClientID;
        IF @@ROWCOUNT = 0
        BEGIN
            RETURN -1; 
        END
        ELSE
        BEGIN
            RETURN 1; 
        END
    END TRY
    BEGIN CATCH
        RETURN -3; 
    END CATCH
END;
GO

CREATE PROCEDURE SP_GetClientByPhoneOrEmail
    @Phone VARCHAR(20), 
    @Email VARCHAR(100) 
AS
BEGIN
    SET NOCOUNT ON; 
    IF dbo.IsClientExistsByPhoneOrEmail(@Phone, @Email, NULL) = 0
    BEGIN
        RETURN -1; 
    END

    SELECT *
    FROM Clients
    WHERE Phone = @Phone OR Email = @Email;
END;
GO

-- TR_BookNotFree_AfterBorrow: Sets IsFree to 0 after a book is borrowed
CREATE TRIGGER TR_BookNotFree_AfterBorrow
ON Borrowing
AFTER INSERT
AS
BEGIN
    UPDATE Books
    SET IsFree = 0
    WHERE BookID IN (SELECT BookID FROM inserted);
END;
GO

CREATE TRIGGER TRG_UpdateBookIsFree_OnReturn
ON Borrowing
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Books
    SET IsFree = 1
    FROM Books B
    INNER JOIN Inserted I ON B.BookID = I.BookID
    INNER JOIN Deleted D ON I.BorrowingID = D.BorrowingID
    WHERE
        I.ReturnDate IS NOT NULL
        AND D.ReturnDate IS NULL;
END;
GO

CREATE PROCEDURE SP_AddBorrowing
    @ClientID BIGINT, 
    @BookID BIGINT, 
    @BorrowDate DATE
AS
BEGIN
    SET NOCOUNT ON; 

    IF dbo.IsClientExistsByID(@ClientID) = 0
        RETURN -1; 

    IF NOT EXISTS (SELECT 1 FROM Books WHERE BookID = @BookID)
        RETURN -2; 

    IF EXISTS (SELECT 1 FROM Books WHERE BookID = @BookID AND IsFree = 0)
        RETURN -3; 

    BEGIN TRY
        INSERT INTO Borrowing (ClientID, BookID, BorrowDate)
        VALUES (@ClientID, @BookID, @BorrowDate);
        RETURN SCOPE_IDENTITY(); 
    END TRY
    BEGIN CATCH
        RETURN -4; 
    END CATCH
END;
GO

CREATE PROCEDURE SP_GetBorrowings
AS
BEGIN
    SELECT
        b.BorrowingID,
        c.ClientName,
        bk.BookTitle AS BookTitle,
        b.BorrowDate,
        b.ReturnDate,
        CASE
            WHEN b.ReturnDate IS NULL THEN 'Not Returned'
            ELSE 'Returned'
        END AS Status
    FROM Borrowing b
    INNER JOIN Clients c ON b.ClientID = c.ClientID
    INNER JOIN Books bk ON b.BookID = bk.BookID
    ORDER BY b.BorrowDate DESC;
END;
GO

CREATE PROCEDURE SP_ReturnBook
    @BorrowingID BIGINT, 
    @ReturnDate DATE
AS
BEGIN
    SET NOCOUNT ON; 

    IF NOT EXISTS (SELECT 1 FROM Borrowing WHERE BorrowingID = @BorrowingID)
    BEGIN
        RETURN -1; 
    END

    IF EXISTS (
        SELECT 1 FROM Borrowing
        WHERE BorrowingID = @BorrowingID AND ReturnDate IS NOT NULL
    )
    BEGIN
        RETURN -2; 

    BEGIN TRY
        UPDATE Borrowing
        SET ReturnDate = @ReturnDate
        WHERE BorrowingID = @BorrowingID;

        RETURN 1; 
    END TRY
    BEGIN CATCH
        RETURN -3; 
    END CATCH
END;
GO

CREATE PROCEDURE SP_GetAllBorrowingsDetails
AS
BEGIN
    SELECT
        B.BorrowingID,
        C.ClientName,
        BK.BookTitle,
        B.BorrowDate,
        B.ReturnDate,
        CASE
            WHEN B.ReturnDate IS NULL THEN 'Borrowed'
            ELSE 'Returned'
        END AS Status
    FROM Borrowing B
    INNER JOIN Clients C ON B.ClientID = C.ClientID
    INNER JOIN Books BK ON B.BookID = BK.BookID;
END;
GO

CREATE PROCEDURE SP_GetAvailableBooks
AS
BEGIN
    SELECT
        BookID,
        BookTitle,
        PublicationYear,
        AuthorID 
    FROM Books
    WHERE IsFree = 1;
END;
GO
