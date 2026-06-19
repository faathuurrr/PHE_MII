# Supply Management System

A web-based Supply Management application built for a Technical Test. This project handles the end-to-end vendor registration and approval workflow, featuring a two-tier approval process (Admin -> Logistic Manager).

## Tech Stack
- **Backend**: .NET 8 Web API, SQL Server
- **Frontend**: Blazor WebAssembly
- **Authentication**: JWT Bearer Token

## Key Features
- **Vendor Registration**: Upload company details, profile photo, and create login credentials.
- **Two-Tier Approval**: 
  1. Admin approves the initial registration.
  2. Vendor logs in to complete their specific profile (Business Sector & Company Type).
  3. Logistic Manager reviews and approves the completed profile.
- **Role-Based Access Control**: Separate dashboards and access rights for `Admin`, `LogisticManager`, and `Vendor`.
- **CRUD Operations**: Full management of vendor data (Create, Read, Update, Delete) accessible via the API and Admin UI. Vendor can also update their own profile.
- **Repository Pattern**: Clean data access layer separating business logic from EF Core DbContext.

## Prerequisites
- .NET 8 SDK
- SQL Server 

## Setup Instructions

### 1. Database Configuration
Update the connection string in `SupplyManagement.API/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=SupplyManagementDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True"
}
```

### 2. Apply Migrations
Navigate to the API folder and run the EF Core migrations to build the schema and seed initial data:
```bash
cd SupplyManagement.API
dotnet ef database update
```
*Note: This will automatically seed two default accounts: `admin` and `manager` (password: `password123` for both).*

### 3. Running the Application

**Start the API:**
```bash
cd SupplyManagement.API
dotnet run
```
The API will run on `http://localhost:5293` (or similar, check console output). Swagger UI is available at `/swagger`.

**Start the Web UI:**
```bash
cd SupplyManagement.Web
dotnet run
```
The Blazor app will run on `http://localhost:5108`.

## Default Credentials
- **Admin**: `admin` / `password123`
- **Logistic Manager**: `manager` / `password123`

---
*Developed for Technical Assessment.*
