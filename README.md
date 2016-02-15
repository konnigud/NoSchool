# NoSchool

 
Assignment in week 4

In this assignment, you will build on the previous assignments. 

Each course should have a property called MaxStudents, indicating how many students can be enrolled in a given course at any given time. This should be a part of the Course Details object when requesting a single course, and should be a part of the create/update methods for a course.
A new subresource for courses should be added, called /waitinglist (i.e. /api/courses/{id}/waitinglist. It should be possible to add students to the waiting list via POST to that resource, and a GET request for that resource should return a list of all students on the waiting list.
The assignment will be graded according to how well you manage to implement the following business rules:

(10%) Rule 0: Only users/persons already in the system can be enrolled in a course or added to the waiting list.
(10%) Rule 1: A course should not allow more students to be registered than the MaxStudents property specifies.
(10%) Rule 2: A given student can only be registered once into a course, i.e. if a student is already an active student in a course, (s)he should not be enrolled again.
(10%) Rule 3: If a student is on the waiting list of a course, and is then later enrolled as a student, (s)he should be removed from the waiting list automatically.
(10%) Rule 4: When a student is removed from a course, (s)he should NOT be permanently deleted. The entry should still exist, but it should be marked as deleted.
(10%) Rule 5: If a student has been removed from a course, (s)he should be allowed to re-enroll.
(10%) Rule 6: When requesting a list of all students, only active students should be returned, i.e. it should NOT include students which have been removed from the course.
(10%) Rule 7: A student can not enter the waiting list if (s)he is already on the waiting list.
(10%) Rule 8: A student can not enter the waiting list if (s)he is already enrolled as a student.
(10%) Rule 9: In all cases, the system should return 404 if an operation is attempted on a nonexisting course.
 The following APIs will be tested (note: many of those should be already implemented in previous assignments:

POST /api/courses -> will create a course from the given data, present in the HTTP Body:
{ TemplateID: "T-514-VEFT",
  StartDate: "some date in ISO format",
  EndDate: "some date in ISO format",
  Semester: "20153",
  MaxStudents: 10
}
 
GET /api/courses/{id}/students -> returns a list of students in a course. The only parameter is the {id} specified in the URL.
Each entry in the list contains the name and ssn of the student. Example:
[{ 
    Name: "Herp McDerpsson",
    SSN: "1234567890"
}]
 
POST /api/courses/{id}/students -> adds a student to the course. It is assumed that the student already exists in the system, i.e. the SSN should be enough.The method accepts the ID of the course as its only parameter coming from the URL, plus the following JSON data coming from the request body:
{
    SSN: "1234567890"
}
It should return HTTP status 201 (Created), and the following should be in the body of the response:
{
    Name: "Herp McDerpsson",
    SSN: "1234567890"
}
 
DELETE /api/courses/{id}/students/{ssn} - removes a student from a course. The only parameters are the ID of the course and the SSN of the student, both present in the URL.
 
GET /api/courses/{id}/waitinglist - returns a list of all persons on the waiting list. The format should be the same as when requesting a list of students, i.e. an array where each item contains the properties "SSN" and "Name".
 
POST /api/courses/{id}/waitinglist -> adds a student to the waiting list. The input is the ID of the course, present in the URL, and a JSON object in the request body containing the SSN of the person being added to the waiting list. Example:
{
    SSN: "1234567890"
}
Implementation details and seed data

The application should maintain a list of users, only people in that list can be enrolled in a course, or registered to the waiting list. I.e. it should be enough to specify the SSN of a person when enrolling a student or adding a person to the waiting list of a course. If the client tries to enroll a student which doesn't exist, a 404 status code should be returned.

It is assumed that the following users/persons is present in the db initially:

1234567890 Herp McDerpsson 1
1234567891 Herpina Derpy 1
1234567892 Herp McDerpsson 2
1234567893 Herpina Derpy 2
1234567894 Herp McDerpsson 3
1234567895 Herpina Derpy 3
1234567896 Herp McDerpsson 4
1234567897 Herpina Derpy 4
1234567898 Herp McDerpsson 5
1234567899 Herpina Derpy 5

...and that the following course template is present:

T-514-VEFT Vefþjónustur

The following API calls will then be tested in the given order (see the attached WaitingListTester project). If your API responds correctly to each of them, you may consider the project to be done.

POST /api/courses 
Post data: { TemplateID: "T-514-VEFT", StartDate: "20160818T00:00:00", EndDate: "2016-11-10T00:00:00", Semester: "20163", MaxStudents: 4 }
-> should return 201 with the newly created course, including its ID. The "Location" header should be set (it will be used to construct the subsequent requests!)
POST /api/courses/{ID}/students 
Post data: { SSN: "9876543210"}
-> should return 404 (Person not found) (Rule 0)
POST /api/courses/{ID}/students 
Post data: { SSN: "1234567890"}
-> should return 201 (Herp McDerpsson 1 is now enrolled in the course)
POST /api/courses/{ID}/students 
Post data: { SSN: "1234567890"}
-> should return 412 (Student already registered) (Rule 2)
POST /api/courses/{ID}/students
Post data: { SSN: "1234567891"}
-> should return 201 (Herpina Derpy 1 is now enrolled in the course)
POST /api/courses/{ID}/students 
Post data: { SSN: "1234567892"}
-> should return 201 (Herp McDerpsson 2 is now enrolled in the course)
POST /api/courses/{ID}/students
Post data: { SSN: "1234567893"}
-> should return 201 (Herpina Derpy 2 is now enrolled in the course)
POST /api/courses/{ID}/students 
Post data: { SSN: "1234567894"}
-> should return 412 (Max students reached) (Rule 1)
DELETE /api/courses/{ID}/students/1234567890
-> should return 204 -> Herp McDerpsson is no longer registered in the course (entry should still exist, Rule 4)
GET /api/courses/{ID}/students
-> should return 200 and the list should include exactly 3 students (Rule 6)
POST /api/courses/{ID}/waitinglist
Post data: { SSN: "9876543210" }
-> should return 404 (Person not found) (Rule 0)
POST /api/courses/{ID}/waitinglist
Post data: { SSN: "1234567890" }
-> should return 200
GET /api/courses/{ID}/waitinglist
-> should return 200, and a list with exactly one student (Herp McDerpsson 1)
POST /api/courses/{ID}/students 
Post data: { SSN: "1234567890"}
-> should return 201 (Herp McDerpsson 1 is now enrolled in the course - again!) (Rule 5)
GET /api/courses/{ID}/waitinglist
-> should return 200, and an empty list (Rule 3)
POST /api/courses/{ID}/waitinglist
Post data: { SSN: "1234567895" }
-> should return 200 (Herpina Derpy 3 registered on the waiting list)
POST /api/courses/{ID}/waitinglist
Post data: { SSN: "1234567895" }
-> should return 412 (Person already on waiting list) (Rule 7)
POST /api/courses/{ID}/waitinglist
Post data: { SSN: "1234567891" }
-> should return 412 (Herpina Derpy 1 is already enrolled as a student) (Rule 8)
