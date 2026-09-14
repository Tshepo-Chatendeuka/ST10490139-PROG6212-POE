-- Create Database (Optional depending on your SSMS setup)
CREATE DATABASE RaceDayDB;
GO
USE RaceDayDB;
GO

-- 1. SystemRole Table
CREATE TABLE SystemRole (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL UNIQUE
);

-- 2. AppUser Table
CREATE TABLE AppUser (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    RoleID INT NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    CONSTRAINT FK_AppUser_Role FOREIGN KEY (RoleID) REFERENCES SystemRole(RoleID)
);

-- 3. Event Table
CREATE TABLE Event (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserID INT NOT NULL,
    Name VARCHAR(200) NOT NULL,
    Description TEXT,
    EventDate DATETIME NOT NULL,
    Location VARCHAR(200) NOT NULL,
    Distance DECIMAL(5,2) NOT NULL,
    EventType VARCHAR(50) NOT NULL, -- Run, Walk, or Cycle
    BannerImageURL VARCHAR(255),
    CONSTRAINT FK_Event_Organiser FOREIGN KEY (OrganiserID) REFERENCES AppUser(UserID)
);

-- 4. EventCategory Table
CREATE TABLE EventCategory (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    CategoryName VARCHAR(100) NOT NULL,
    CriteriaDetails VARCHAR(200),
    CONSTRAINT FK_EventCategory_Event FOREIGN KEY (EventID) REFERENCES Event(EventID) ON DELETE CASCADE
);

-- 5. Enrolment Table
CREATE TABLE Enrolment (
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantID INT NOT NULL,
    CategoryID INT NOT NULL,
    Status VARCHAR(50) DEFAULT 'Confirmed',
    EnrolmentDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Enrolment_Participant FOREIGN KEY (ParticipantID) REFERENCES AppUser(UserID),
    CONSTRAINT FK_Enrolment_Category FOREIGN KEY (CategoryID) REFERENCES EventCategory(CategoryID) ON DELETE CASCADE
);

-- 6. RaceResult Table
CREATE TABLE RaceResult (
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentID INT NOT NULL UNIQUE,
    FinishTime TIME NOT NULL,
    FinishingPosition INT NOT NULL,
    CONSTRAINT FK_RaceResult_Enrolment FOREIGN KEY (EnrolmentID) REFERENCES Enrolment(EnrolmentID) ON DELETE CASCADE
);

-- ==========================================
-- SEED DATA
-- ==========================================

-- Seed Roles
INSERT INTO SystemRole (RoleName) VALUES ('Organiser'), ('Participant');

-- Seed Users (2 Organisers, 2 Participants)
-- Note: PasswordHashes are simplified for the sake of the DB script. In API, use BCrypt/Argon2.
INSERT INTO AppUser (RoleID, FirstName, LastName, Email, PasswordHash) VALUES 
(1, 'John', 'Smith', 'john.organiser@raceday.com', 'hashed_pw_1'),
(1, 'Sarah', 'Connor', 'sarah.organiser@raceday.com', 'hashed_pw_2'),
(2, 'Mike', 'Johnson', 'mike.runner@mail.com', 'hashed_pw_3'),
(2, 'Emma', 'Watson', 'emma.cyclist@mail.com', 'hashed_pw_4');

-- Seed Events (3 Events)
INSERT INTO Event (OrganiserID, Name, Description, EventDate, Location, Distance, EventType) VALUES 
(1, 'Soweto Marathon', 'Annual road running event in Soweto.', '2026-11-01 06:00:00', 'Soweto, Johannesburg', 42.2, 'Run'),
(1, 'Cape Town Cycle Tour', 'Iconic cycling tour around the Cape Peninsula.', '2027-03-14 06:00:00', 'Cape Town', 109.0, 'Cycle'),
(2, 'Pretoria Charity Walk', 'A fun 5km walk for charity.', '2026-10-15 08:00:00', 'Pretoria, Gauteng', 5.0, 'Walk');

-- Seed Categories
INSERT INTO EventCategory (EventID, CategoryName, CriteriaDetails) VALUES 
(1, 'Full Marathon - Senior', '42.2km, Age 20-39'),
(1, 'Half Marathon - Open', '21.1km, All ages'),
(2, 'Elite Cyclist', '109km, Professional'),
(3, 'Fun Walk - Family', '5km, All ages');

-- Seed Enrolments
INSERT INTO Enrolment (ParticipantID, CategoryID, Status) VALUES 
(3, 1, 'Confirmed'),
(4, 3, 'Confirmed');

-- Seed Results (assuming the event has passed)
INSERT INTO RaceResult (EnrolmentID, FinishTime, FinishingPosition) VALUES 
(1, '03:15:22', 45);