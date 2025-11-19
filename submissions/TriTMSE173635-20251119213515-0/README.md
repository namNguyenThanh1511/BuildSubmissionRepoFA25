# Leopard API

A complete ASP.NET Core Web API for managing Leopard profiles with JWT authentication and role-based authorization.

## Features

- **JWT Authentication**: Secure login with role-based access control
- **LeopardProfile Management**: Full CRUD operations with validation
- **Role-Based Authorization**: Different access levels for different user roles
- **Swagger Documentation**: Complete API documentation with JWT support
- **Error Handling**: Consistent error responses with specific error codes
- **Validation**: Input validation with custom regex patterns

## User Roles

- **administrator (RoleId=5)**: Full access (CRUD + search)
- **moderator (RoleId=6)**: Full access (CRUD + search)
- **developer (RoleId=7)**: Read + search only
- **member (RoleId=4)**: Read + search only
- **Other roles**: No token issued

## API Endpoints

### Authentication
- `POST /api/auth` - Login with email and password

### LeopardProfile
- `GET /api/LeopardProfile` - Get all profiles (all roles with token)
- `GET /api/LeopardProfile/{id}` - Get profile by ID (all roles with token)
- `POST /api/LeopardProfile` - Create new profile (administrator, moderator only)
- `PUT /api/LeopardProfile/{id}` - Update profile (administrator, moderator only)
- `DELETE /api/LeopardProfile/{id}` - Delete profile (administrator, moderator only)
- `GET /api/LeopardProfile/search` - Search by name and weight (all roles with token)

## Setup Instructions

### Prerequisites
- .NET 8.0 SDK
- SQL Server (Express or higher)
- Visual Studio 2022 or VS Code

### Database Setup
1. Ensure SQL Server is running
2. Create database `SU25LeopardDB`
3. Update connection string in `appsettings.json` if needed
4. Run Entity Framework migrations (if using migrations)

### Running the Application
1. Open the solution in Visual Studio or VS Code
2. Restore NuGet packages
3. Build the solution
4. Run the application
5. Access Swagger UI at: `https://localhost:7001/swagger`

## Testing with Postman

1. Import the `LeopardAPI.postman_collection.json` file into Postman
2. Set the environment variable `base_url` to your API URL (default: `https://localhost:7001`)
3. Run the tests in order:
   - Login Success
   - Login Failed
   - Get All LeopardProfiles
   - Get LeopardProfile by ID
   - Create LeopardProfile (Authorized)
   - Update LeopardProfile
   - Delete LeopardProfile
   - Search LeopardProfile

## Validation Rules

### LeopardProfile Validation
- **LeopardName**: Must follow regex pattern `^([A-Z0-9][a-zA-Z0-9#]*\s)+([A-Z0-9][a-zA-Z0-9]*)$`
- **Weight**: Must be greater than 15
- **All fields**: Required

## Error Codes

- **HB40001**: Missing/invalid input (400)
- **HB40101**: Token missing/invalid (401)
- **HB40301**: Permission denied (403)
- **HB40401**: Resource not found (404)
- **HB50001**: Internal server error (500)

## Sample Data

### Login Credentials
```json
{
  "email": "administrator@leopard.com",
  "password": "@1"
}
```

### Create LeopardProfile
```json
{
  "leopardProfileId": 0,
  "leopardTypeId": 1,
  "leopardName": "Panthera tigris tigris",
  "weight": 35,
  "characteristics": "The leopard possesses a tawny or rusty yellow-colored coat with close-set rosettes and dark spots",
  "careNeeds": "These animals are classified as endangered by the IUCN"
}
```

## JWT Token Usage

After successful login, include the JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## Swagger Documentation

The API includes comprehensive Swagger documentation with:
- All endpoints listed
- JWT token insertion support
- Request/response examples
- Authentication requirements

Access Swagger UI at: `https://localhost:7001/swagger` 