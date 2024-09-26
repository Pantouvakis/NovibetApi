Database createad with the Database Schema & Seed Data given:

    "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\MSSQLLocalDB;Database=NOVIBET;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true" }
dotnet ef migrations add InitialCreate dotnet ef database update

The application will start and listen on http://localhost:5000

API Endpoints
GET /api/ipinfo/{ip}
Retrieves IP information for the specified IP address.

GET /api/report/country-report
Returns a report of IP addresses per country. Query parameter countryCodes can be used to filter results.

EXAMPLES
    - **GET** http://localhost:5000/api/ipinfo/8.8.8.8
    - **GET** http://localhost:5000/api/report/country-report
    - **GET** http://localhost:5000/api/report/country-report?countryCodes=US&countryCodes=CA

IP Information REST API Overview This project is a RESTful API built using .NET Core, designed to provide information about IP addresses, including country name, two-letter code, and three-letter code. The API interacts with an SQL database to manage and store IP information, utilizing caching for performance optimization. The project also includes functionality for periodic updates of IP information and reporting capabilities.

Features IP Information Retrieval:

Exposes an endpoint to return details for a specific IP address. Information is fetched from cache, database, or an external IP2C service. Periodic Updates:

A background job that updates IP information every hour, ensuring data accuracy. Retrieves all IPs in batches and updates the database if changes are detected. Reporting Endpoint:

Provides a report on the number of IP addresses per country and the last update time. Allows filtering by specific country codes. Technologies Used Backend: .NET 6 (ASP.NET Core) Database: SQL Server ORM: Entity Framework Core Caching: Memory Cache External Service: IP2C API for IP information
