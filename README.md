# garg-marble-server

### To create a new project run
1. dotnet new webapi -n <project_name>
2. cd <project_name>
3. dotnet run

### Settings
1. to update the settings of the project use appsettings.json file
2. you can use in case to update your databse connection or any other connection
3. program.cs is being used for configuration  of the application
4. Controllers are to kep all endpoints for the application

### Installations
1. dotnet add package Swashbuckle.AspNetCore --version 6.5.0 (for swagger)
    > (Make changes in Program.cs after that for configuration)
2. for setting up MongoDB in the project
    > dotnet add package MongoDB.Driver
    > add all required files in Controllers and Models folder for different collection that you'll be creating
    > update appsettings.json
    > update Program.cs
    > start adding Models for all the tables you require

3. for JWT token
    > dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
4. > dotnet add package System.IdentityModel.Tokens.Jwt
5. for Bcrypt
    > dotnet add package BCrypt.Net-Next