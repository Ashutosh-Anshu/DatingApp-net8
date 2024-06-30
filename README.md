# Creating a Dating App Using C#, .NET Core, and Angular

## Prerequisites

### For .NET Framework

- **Swashbuckle.AspNetCore** Version="6.4.0"
- **System.IdentityModel.Tokens.Jwt** Version="7.6.2"
- **Microsoft.EntityFrameworkCore.Sqlite** Version="8.0.6"
- **Microsoft.AspNetCore.Authentication.JwtBearer** Version="8.0.6"
- **Microsoft.EntityFrameworkCore.Design** Version="8.0.6"

### For Angular

- **Node.js** "v20.14.0"
- **npm** "10.8.1"
- **Angular** "18.0.3"
- **bootstrap** "^5.3.3"
- **bootstrap-icons** "^1.11.3"
- **bootswatch** "^5.3.3"
- **font-awesome** "^4.7.0"
- **mkcert** "^3.2.0"
- **ngx-bootstrap** "^6.2.0"
- **ngx-toastr** "^19.0.0"

## Installation

1. Clone the project from [GitHub](https://github.com/Ashutosh-Anshu/DatingApp-net8).
   
2. Open your VS Code.

## Running the Backend

1. Navigate to the API directory : `cd API`

3. **Database Migration**: If this is the first time you are setting up the project or if there are new migrations,
  you need to apply these migrations to your local database. Ensure your database server (e.g., SQLite) is running and then execute : `dotnet ef database update`
This command will apply any pending migrations to your database.

2. Run the backend using the following command : `dotnet watch`

## Running the Angular App

1. Navigate to the client directory : `cd client`

2. Serve the Angular app using the following command : `ng serve -o`

---

### Notes:

- Ensure all dependencies are installed and up-to-date before running the application.
- Adjust versions as necessary based on updates and compatibility.
- Database migrations ensure that the database schema matches the expected structure defined in the project's code. Run migrations whenever there are new changes or when setting up the project for the first time.
