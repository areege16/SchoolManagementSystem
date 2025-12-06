# SchoolManagementSystem

## Setup Instructions

1. **Create the project structure**  
   - Created a new ASP.NET Core Web API project named `SchoolManagementSystem` (version 8.0)
   - Followed **Clean Architecture** by adding three additional layers:
     - `SchoolManagementSystem.Domain`
     - `SchoolManagementSystem.Application`
     - `SchoolManagementSystem.Infrastructure`
     - `SchoolManagementSystem.Web`

2. **Add project references**  
   - Added necessary references between the layers to allow proper interaction:
     - `Web` references `Application` and `Infrastructure`and `Infrastructure`
     - `Application` references `Domain` and `Infrastructure`
     - `Infrastructure` references `Domain`

3. **Install required NuGet packages (Installed packages suitable for .NET 8.0)**  
   - Installed necessary packages for each layer (e.g., Entity Framework Core, AutoMapper, Identity, FluentValidation, MediatR, AutoMapper etc.)
  
4. **Registered services in `Program.cs`**

5. **Create entities and custom ApplicationUser and ApplicationRole**  
   - Defined all domain models in the `Domain` layer (e.g., `Student`, `Teacher`, `Assignment`, `Submission`).
   - Created a custom `ApplicationUser` class extending `IdentityUser` to manage users (students and teachers).

## DB migration 

1. **Configure database connection**  
   - Added the database connection string in `appsettings.json` for Entity Framework Core:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=Areege;Initial Catalog=SchoolManagement;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
   }
   
2. **Create ApplicationContext**
   - Created `ApplicationContext` in `Infrastructure` layer, inheriting `IdentityDbContext<ApplicationUser, ApplicationRole, string>`.
   - Defined `DbSet`s for all entities and applied default values / query filters as needed.
3. **Apply migrations** 
   - Created initial migration: 
       Add-Migration InitialCreate
     
     - Disabled cascade delete to avoid cycles.
       
   - Applied migration:
     Update-Database

## Sample API Requests

### 1. Login (Account)
**POST** `https://localhost:7058/api/Account/login`  
**Headers:**  
Content-Type: application/json
**Body (raw JSON):**  
```json
{
  "userName": "string",
  "password": "string"
}

2. Submit Assignment (Student)
POST /api/student/assignments/{id}/submit
Headers:
Authorization: Bearer <token>
File: <select file>
AssignmentId: <assignment id>


3. Get All Classes
GET https://localhost:7058/api/Classes/GetAllClasses
Headers:
Authorization: Bearer <token>

4. View Enrolled Classes (Student)
GET /api/student/classes
Headers:
Authorization: Bearer <token>

5. View Attendance (Student)
GET /api/student/attendance
Headers:
Authorization: Bearer <token>

6. View Grades (Student)
GET /api/student/grades
Headers:
Authorization: Bearer <token>

7. Create Assignment (Teacher)
POST /api/teacher/assignments
Headers:
Authorization: Bearer <token>
Content-Type: application/json
Body (raw JSON):
{
  "title": "string",
  "description": "string",
  "dueDate": "2025-12-01T22:45:31.276Z",
  "classId": 0,
  "createdByTeacherId": "string",
  "createdDate": "2025-12-01T22:45:31.276Z"
}

 8. Grade Student Submission (Teacher)
POST /api/teacher/assignments/{id}/grade
Headers:
Authorization: Bearer <token>
Content-Type: application/json
Body (raw JSON):
{
  "id": 0,
  "assignmentId": 0,
  "studentId": "string",
  "grade": 0,
  "remarks": "string"
}

9. Create Department (Admin)
POST /api/Classes
Headers:
Authorization: Bearer <token>
Content-Type: application/json
Body (raw JSON):
{
  "name": "string",
  "isActive": true,
  "semester": 1,
  "startDate": "2025-12-01",
  "endDate": "2025-12-01",
  "courseId": 0,
  "teacherId": "string"
}

10. Create Department (Admin)
Get /api/Departments
Headers:
Authorization: Bearer <token>
