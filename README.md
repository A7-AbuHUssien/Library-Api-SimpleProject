Project Description:
This project follows a layered architecture consisting of a Data Access Layer (DAL),
Business Logic Layer (BLL), and API layer built with C# and ADO.NET.
While the structure is layered, all business logic is implemented inside T-SQL stored procedures in SQL Server,
with the BLL serving primarily as a bridge between the API and the database.

The DAL uses ADO.NET to execute stored procedures, handle parameterized queries,
and map results to application models. The API exposes endpoints for data interaction,
ensuring a clean separation between the presentation and database layers.
Centralizing the business rules in stored procedures ensures data integrity,
performance optimization, and easier database-level maintenance.


Key Points:

Languages & Tools: C# (.NET), ADO.NET, T-SQL, SQL Server
Architecture: API layer → BLL → DAL → Stored Procedures
Business Logic: Fully implemented in stored procedures
Database: Relational schema with indexing and constraints for efficiency
Focus: Centralized logic, secure data access, maintainable layered design
