-- Create the database if it does not exist
SELECT pg_terminate_backend(procpid) FROM pg_stat_activity WHERE datname = 'mydb';

DROP DATABASE anikatze_postgres;
CREATE DATABASE anikatze_postgres;

-- Connect to the newly created database
\c anikatze_postgres

-- Create root user
DROP USER root;
CREATE USER root WITH PASSWORD 'anikatze';

-- Grant privileges to root user
DROP OWNED BY root;
GRANT ALL PRIVILEGES ON DATABASE anikatze_postgres TO root;

-- Create schema
DROP SCHEMA anikatze_schema;
CREATE SCHEMA anikatze_schema;


-------------------------- Create everything --------------------------

drop table if exists anikatze_schema.UserAccount cascade;
-- Create the UserAccount table
CREATE TABLE ANIKATZE_SCHEMA.UserAccount (
    UserID INT PRIMARY KEY,  -- Primary Key
    UserGuid VARCHAR(36) NOT NULL,  -- Unique GUID
    Username VARCHAR(50) NOT NULL,  -- Username
    Email VARCHAR(255) NOT NULL,  -- Email address
    Password VARCHAR(255) NOT NULL,  -- Password (hashed or encrypted)
    FirstName VARCHAR(50) NOT NULL,  -- First name
    LastName VARCHAR(50) NOT NULL,  -- Last name
    CONSTRAINT UC_User_Email UNIQUE (Email)  -- Ensure email uniqueness
);

drop table if exists anikatze_schema.Cart cascade;
-- Create the Cart table with foreign key to User (assuming the Cart table schema)
CREATE TABLE ANIKATZE_SCHEMA.Cart (
    CartID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for Cart
    CONSTRAINT FK_Cart_User FOREIGN KEY (UserID) REFERENCES ANIKATZE_SCHEMA.UserAccount(UserID)
);

drop table if exists anikatze_schema.Review cascade;
-- Create the Review table with foreign key to User (assuming the Review table schema)
CREATE TABLE ANIKATZE_SCHEMA.Review (
    ReviewID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for Review
    CONSTRAINT FK_Review_User FOREIGN KEY (UserID) REFERENCES ANIKATZE_SCHEMA.UserAccount(UserID)
);

drop table if exists anikatze_schema.UserQuiz cascade;
-- Create the UserQuiz table with foreign key to User (assuming the UserQuiz table schema)
CREATE TABLE ANIKATZE_SCHEMA.UserQuiz (
    UserQuizID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for UserQuiz
    CONSTRAINT FK_UserQuiz_User FOREIGN KEY (UserID) REFERENCES ANIKATZE_SCHEMA.UserAccount(UserID)
);

drop table if exists anikatze_schema.UserCourse cascade;
-- Create the UserCourse table with foreign key to User (assuming the UserCourse table schema)
CREATE TABLE ANIKATZE_SCHEMA.UserCourse (
    UserCourseID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for UserCourse
    CONSTRAINT FK_UserCourse_User FOREIGN KEY (UserID) REFERENCES ANIKATZE_SCHEMA.UserAccount(UserID)
);

drop table if exists anikatze_schema.UserLectionCompletion cascade;
-- Create the UserLectionCompletion table with foreign key to User (assuming the UserLectionCompletion table schema)
CREATE TABLE ANIKATZE_SCHEMA.UserLectionCompletion (
    UserLectionCompletionID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for UserLectionCompletion
    CONSTRAINT FK_UserLectionCompletion_User FOREIGN KEY (UserID) REFERENCES ANIKATZE_SCHEMA.UserAccount(UserID)
);

drop table if exists anikatze_schema.PaymentStatus cascade;
-- Assuming you also have a PaymentStatus table, you would have:
CREATE TABLE ANIKATZE_SCHEMA.PaymentStatus (
    PaymentStatusID INT PRIMARY KEY,  -- Primary Key
    StatusName varchar(50) NOT NULL  -- Status name or description
);

drop table if exists anikatze_schema.Bill cascade;
-- Assuming the Bill table has already been created:
CREATE TABLE ANIKATZE_SCHEMA.Bill (
    BillID INT PRIMARY KEY,  -- Primary Key
    BillGuid varchar(36) NOT NULL,  -- Unique GUID
    UserID INT NOT NULL,  -- Foreign Key to User
    IssuedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,  -- Issued at time
    CONSTRAINT FK_Bill_User FOREIGN KEY (UserID) REFERENCES ANIKATZE_SCHEMA.UserAccount(UserID)
);

drop table if exists anikatze_schema.Payment cascade;
-- Create the Payment table
CREATE TABLE ANIKATZE_SCHEMA.Payment (
    PaymentID INT PRIMARY KEY,  -- Primary Key
    PaymentGuid varchar(36) NOT NULL,  -- Unique GUID
    BillID INT NOT NULL,  -- Foreign Key to Bill
    PaymentStatusID INT NOT NULL,  -- Foreign Key to PaymentStatus
    Amount DECIMAL(18, 2) NOT NULL,  -- Amount field
    PaidAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,  -- Date when payment was made
    CONSTRAINT FK_Payment_Bill FOREIGN KEY (BillID) REFERENCES ANIKATZE_SCHEMA.Bill(BillID),  -- Foreign Key constraint
    CONSTRAINT FK_Payment_PaymentStatus FOREIGN KEY (PaymentStatusID) REFERENCES ANIKATZE_SCHEMA.PaymentStatus(PaymentStatusID)  -- Foreign Key constraint
);
