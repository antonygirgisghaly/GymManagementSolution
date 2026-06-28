🏋️ Gym Management System

A full-featured web application for managing gym operations — members, trainers, sessions, plans, memberships, and bookings — built with ASP.NET Core MVC following clean N-Tier Architecture.


Built as part of the Route Training Program (MVC Track).




📋 Table of Contents


Overview
Architecture
Features
Tech Stack
Project Structure
Getting Started
Database
Security
Design Patterns



Overview

GymManagementSolution is a multi-layer MVC application that covers the core needs of a gym: registering members and trainers, scheduling sessions, managing subscription plans, tracking memberships and bookings, and maintaining member health records. Access to the system is secured with ASP.NET Core Identity — staff accounts are seeded at startup and protected by role-based authorization.


Architecture

The solution follows a strict 3-Layer N-Tier Architecture:

PL  (Presentation Layer)   →  Controllers, Views, Tag Helpers
 ↓
BLL (Business Logic Layer) →  Services, Interfaces, ViewModels, AutoMapper Profiles
 ↓
DAL (Data Access Layer)    →  Repositories, Unit of Work, EF Core DbContext

Dependency direction is always PL → BLL → DAL. The BLL never references the PL, keeping the business logic fully isolated and testable.


Features

👥 Member Management


Create, read, update, and delete gym members
Profile photo upload with validation (JPG/JPEG/PNG, max 5 MB)
Photos stored outside the public folder and served through a dedicated controller action


🏃 Trainer Management


Full CRUD for trainers
Assignment of trainers to sessions


📅 Session & Plan Management


Define and manage training sessions
Create subscription plans with pricing and duration


📊 Memberships & Bookings


Track member subscriptions to plans
Book members into sessions


🩺 Health Records


Log and update health data per member


🔐 Security (ASP.NET Core Identity)


Cookie-based authentication
Role-based authorization (SuperAdmin, Admin)
Seeded roles and first admin user on startup
Login / Logout via AccountController
Access Denied page for unauthorized role access
Account lockout after 5 failed attempts



Tech Stack

LayerTechnologyFrameworkASP.NET Core MVC (.NET 8)ORMEntity Framework CoreAuthASP.NET Core IdentityMappingAutoMapperDatabaseSQL ServerFrontendRazor Views, Bootstrap, Tag HelpersVersion ControlGit & GitHub


Project Structure

GymManagementSolution/
│
├── GymManagment.DAL/
│   ├── Data/
│   │   └── GymDbContext.cs          # Inherits IdentityDbContext
│   ├── Models/                       # Domain entities
│   ├── Repositories/
│   │   ├── IGenericRepository.cs
│   │   ├── GenericRepository.cs
│   │   └── [Entity]Repository.cs
│   └── UnitOfWork/
│       ├── IUnitOfWork.cs
│       └── UnitOfWork.cs
│
├── GymManagment.BLL/
│   ├── Interfaces/                   # Service contracts
│   ├── Services/                     # Business logic implementations
│   ├── ViewModels/                   # DTOs passed to/from PL
│   ├── Mapping/                      # AutoMapper profiles
│   └── Helpers/
│       └── AttachmentService.cs      # File upload / delete / serve
│
└── GymManagment.PL/
    ├── Controllers/
    │   ├── AccountController.cs      # Login & Logout
    │   ├── MemberController.cs       # [Authorize(Roles = "SuperAdmin")]
    │   └── ...
    ├── Views/
    └── Program.cs                    # DI registration, Identity config, seeding


Getting Started

Prerequisites


.NET 8 SDK
SQL Server (LocalDB or full instance)
Visual Studio 2022 (recommended)


Setup


Clone the repository


bash   git clone https://github.com/antonygirgisghaly/GymManagementSolution.git
   cd GymManagementSolution


Configure the connection string
Open GymManagment.PL/appsettings.json and update:


json   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=GymDB;Trusted_Connection=True;"
   }


Apply migrations


bash   dotnet ef database update --project GymManagment.DAL --startup-project GymManagment.PL


Run the application


bash   dotnet run --project GymManagment.PL


Login with the seeded admin account
A SuperAdmin account is automatically seeded on first run. Check the seeding configuration in Program.cs for the default credentials.



Database

The application uses Entity Framework Core Code-First migrations. The GymDbContext inherits from IdentityDbContext, so Identity tables (Users, Roles, UserRoles) are created alongside domain tables in the same database.

Key domain tables: Members, Trainers, Sessions, Plans, Memberships, Bookings, HealthRecords.


Security

Authentication uses ASP.NET Core Cookie Authentication — the natural fit for a server-rendered MVC app.

ScenarioResultNot logged in → protected pageRedirect to /Account/Login (302)Logged in, wrong roleRedirect to /Account/AccessDenied (403)5 failed login attemptsAccount locked for 2 minutes


Note: Hiding a navbar link is not security. The real protection is the [Authorize] attribute on the controller.




Design Patterns

PatternWhere UsedGeneric RepositoryDAL — one base repo for all entitiesUnit of WorkDAL — single SaveChanges call across reposService / InterfaceBLL — IXService injected into controllers, never the concrete classAutoMapperBLL — ViewModel ↔ Model mapping profilesResult PatternBLL — services return success/failure with a message, no exceptionsAttachment ServiceBLL — centralized file upload/delete/serve logicDependency InjectionPL — all services registered in Program.cs


📄 License

This project is built for educational purposes as part of the Route Training Program.
