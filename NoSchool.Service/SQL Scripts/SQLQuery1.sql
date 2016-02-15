CREATE TABLE CourseTemplates(
	TemplateID NVARCHAR(10) PRIMARY KEY NOT NULL,
	Name NVARCHAR(MAX) NOT NULL
);

CREATE TABLE Courses(
	ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	TemplateID NVARCHAR(10) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	Semester NVARCHAR(5) NOT NULL,
	MaxStudent INT,
	FOREIGN KEY (TemplateID) REFERENCES CourseTemplates(TemplateID)
);

CREATE TABLE Students(
	ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	SSN NVARCHAR(10) NOT NULL,
	Name NVARCHAR(44)
);

CREATE TABLE StudentsInCourses(
	StudentID INT NOT NULL,
	CourseID INT NOT NULL,
	IsActive BIT NOT NULL, -- 0|1 = inactive | active  
	CONSTRAINT pk_StudentInCourseID PRIMARY KEY (StudentID,CourseID),
	FOREIGN KEY(StudentID) REFERENCES Students(ID),
	FOREIGN KEY(CourseID) REFERENCES Courses(ID)
);

CREATE TABLE WaitingLists(
	StudentID INT NOT NULL,
	CourseID INT NOT NULL,
	CONSTRAINT pk_WaitingList PRIMARY KEY (StudentID,CourseID),
	FOREIGN KEY(StudentID) REFERENCES Students(ID),
	FOREIGN KEY(CourseID) REFERENCES Courses(ID)
);

INSERT INTO CourseTemplates VALUES('T-514-VEFT','Vefþjónustur');


INSERT INTO Students VALUES('1234567890', 'Herp McDerpsson 1');
INSERT INTO Students VALUES('1234567891', 'Herpina Derpy 1');
INSERT INTO Students VALUES('1234567892', 'Herp McDerpsson 2');
INSERT INTO Students VALUES('1234567893', 'Herpina Derpy 2');
INSERT INTO Students VALUES('1234567894', 'Herp McDerpsson 3');
INSERT INTO Students VALUES('1234567895', 'Herpina Derpy 3');
INSERT INTO Students VALUES('1234567896', 'Herp McDerpsson 4');
INSERT INTO Students VALUES('1234567897', 'Herpina Derpy 4');
INSERT INTO Students VALUES('1234567898', 'Herp McDerpsson 5');
INSERT INTO Students VALUES('1234567899', 'Herpina Derpy 5');


--DROP TABLE WaitingLists;
--DROP TABLE StudentsInCourses;
--DROP TABLE Students;
--DROP TABLE Courses;
--DROP TABLE CourseTemplates;
