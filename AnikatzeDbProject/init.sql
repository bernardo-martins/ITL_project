-- Create the Payment table
CREATE TABLE Payment (
    PaymentID INT PRIMARY KEY,  -- Primary Key
    PaymentGuid NVARCHAR(36) NOT NULL,  -- Unique GUID
    BillID INT NOT NULL,  -- Foreign Key to Bill
    PaymentStatusID INT NOT NULL,  -- Foreign Key to PaymentStatus
    Amount DECIMAL(18, 2) NOT NULL,  -- Amount field
    PaidAt DATETIME NOT NULL DEFAULT GETUTCDATE(),  -- Date when payment was made
    CONSTRAINT FK_Payment_Bill FOREIGN KEY (BillID) REFERENCES Bill(BillID),  -- Foreign Key constraint
    CONSTRAINT FK_Payment_PaymentStatus FOREIGN KEY (PaymentStatusID) REFERENCES PaymentStatus(PaymentStatusID)  -- Foreign Key constraint
);

-- Assuming you also have a PaymentStatus table, you would have:
CREATE TABLE PaymentStatus (
    PaymentStatusID INT PRIMARY KEY,  -- Primary Key
    StatusName NVARCHAR(50) NOT NULL  -- Status name or description
);

-- Assuming the Bill table has already been created:
CREATE TABLE Bill (
    BillID INT PRIMARY KEY,  -- Primary Key
    BillGuid NVARCHAR(36) NOT NULL,  -- Unique GUID
    UserID INT NOT NULL,  -- Foreign Key to User
    IssuedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),  -- Issued at time
    CONSTRAINT FK_Bill_User FOREIGN KEY (UserID) REFERENCES User(UserID)
);

-- Create the User table
CREATE TABLE User (
    UserID INT PRIMARY KEY,  -- Primary Key
    UserGuid NVARCHAR(36) NOT NULL,  -- Unique GUID
    Username NVARCHAR(50) NOT NULL,  -- Username
    Email NVARCHAR(255) NOT NULL,  -- Email address
    Password NVARCHAR(255) NOT NULL,  -- Password (hashed or encrypted)
    FirstName NVARCHAR(50) NOT NULL,  -- First name
    LastName NVARCHAR(50) NOT NULL,  -- Last name
    CONSTRAINT UC_User_Email UNIQUE (Email)  -- Ensure email uniqueness
);

-- Create the Cart table with foreign key to User (assuming the Cart table schema)
CREATE TABLE Cart (
    CartID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for Cart
    CONSTRAINT FK_Cart_User FOREIGN KEY (UserID) REFERENCES User(UserID)
);

-- Create the Review table with foreign key to User (assuming the Review table schema)
CREATE TABLE Review (
    ReviewID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for Review
    CONSTRAINT FK_Review_User FOREIGN KEY (UserID) REFERENCES User(UserID)
);

-- Create the UserQuiz table with foreign key to User (assuming the UserQuiz table schema)
CREATE TABLE UserQuiz (
    UserQuizID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for UserQuiz
    CONSTRAINT FK_UserQuiz_User FOREIGN KEY (UserID) REFERENCES User(UserID)
);

-- Create the UserCourse table with foreign key to User (assuming the UserCourse table schema)
CREATE TABLE UserCourse (
    UserCourseID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for UserCourse
    CONSTRAINT FK_UserCourse_User FOREIGN KEY (UserID) REFERENCES User(UserID)
);

-- Create the UserLectionCompletion table with foreign key to User (assuming the UserLectionCompletion table schema)
CREATE TABLE UserLectionCompletion (
    UserLectionCompletionID INT PRIMARY KEY,  -- Primary Key
    UserID INT NOT NULL,  -- Foreign Key to User
    -- Other fields for UserLectionCompletion
    CONSTRAINT FK_UserLectionCompletion_User FOREIGN KEY (UserID) REFERENCES User(UserID)
);
