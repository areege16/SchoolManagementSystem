# School Management System API

Backend API for a school management system built with **.NET 8**, **Clean Architecture**, and **Entity Framework Core**.  
Supports role-based access (Admin, Teacher, Student). This API enables administrators, teachers, and students to manage departments, courses, classes, attendance, assignments, grades, and notifications.

## ✨ Features

- **Role-Based Access Control** (Admin / Teacher / Student)
- **JWT Authentication with refresh token support** 
- **Full CRUD** for departments, courses, classes, assignments, attendance and more
- **Real-time notifications** via **Server-Sent Events (SSE)** when:
  - An assignment is graded
  - A new class is created
- **Async/await** for all DB operations
- **In-memory caching** for departments and courses
- **Pagination** for classes
- **Soft delete** support (`IsActive` flag)
- **File upload** for assignment submissions (IFormFile)
- **Comprehensive validation** (FluentValidation) and **custom error responses**
- **Logging with Serilog** for structured logging
- **Swagger UI** with Bearer Token authorization

## 🛠️ Tech Stack

- **Framework**: .NET 8 Web API  
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, Web), MediatR, CQRS  
- **ORM**: Entity Framework Core  
- **Database**: SQL Server  
- **Auth**: JWT + Refresh Tokens  
- **Tools**: MediatR, AutoMapper, FluentValidation, Serilog, Swagger  

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (or LocalDB)

### Setup
1. Clone the repository
2. Update the connection string in `Web/appsettings.json` (all other settings are pre-configured)
3. Apply database migrations:
   ```bash
   dotnet ef database update --project Infrastructure --startup-project Web

 **project references**  
   - `Web` references `Application` , `Domain`, and `Infrastructure`
   - `Application` references `Domain` and `Infrastructure`
   - `Infrastructure` references `Domain`

> 💡 API documentation is available via Swagger UI:  
> 🖥️ **Local**: [http://localhost:7058/swagger](http://localhost:7058/swagger)
> 
> 🌐 **Live**: [https://school-mgmt-sys.runasp.net/swagger](https://school-mgmt-sys.runasp.net/swagger)  
>   
> 🔑 **To test secured endpoints**:  
> 1. Register a new user via `/api/auth/register`  
> 2. Log in via `/api/auth/login` to get a JWT token  
> 3. In Swagger, click **Authorize** and enter: `Bearer <your_token>`  
>    *(Example: `Bearer eyJhbGciOi...`)*

## 📡 Key API Endpoints

### 🔐 Authentication
- `POST /api/auth/register` – Register new user (Admin/Teacher/Student)
- `POST /api/auth/login` – Login and receive JWT token
- `POST /api/auth/refresh-token` – Refresh expired token

### 👨‍💼 Admin
- `POST /api/admin/departments` – Create department
- `PUT /api/admin/departments/{id}` – Update department
- `POST /api/admin/courses` – Create course 

### 👩‍🏫 Teacher
- `POST /api/teacher/classes` – Create class 
- `POST /api/teacher/assignments` – Create assignment
- `POST /api/teacher/assignments/{id}/grade` – Grade student submission
- `POST /api/teacher/attendance` – Mark student attendance

### 👨‍🎓 Student
- `GET /api/student/classes` – View enrolled classes
- `POST /api/student/assignments/{id}/submit` – Submit assignment file
- `GET /api/student/grades` – View graded assignments
- `GET /api/student/notifications/stream` – **SSE stream** for real-time notifications
