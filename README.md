# Library (My First Api)

## 📌 Overview
This is a database-driven application built with **C#** and **ADO.NET**, following a layered architecture:
- **API Layer**
- **Business Logic Layer (BLL)**
- **Data Access Layer (DAL)**

All **business logic** is centralized in **SQL Server stored procedures**, while the C# layers handle API endpoints, data access, and communication with the database.

---

## 🛠 Technologies Used
- **C# (.NET)**
- **ADO.NET**
- **SQL Server**
- **T-SQL Stored Procedures**
- **Layered Architecture (API → BLL → DAL)**

---

## 📂 Architecture
1. **API Layer**  
   - Exposes endpoints for client communication  
   - Calls BLL methods for data processing  

2. **Business Logic Layer (BLL)**  
   - Acts as a bridge between the API and DAL  
   - Passes calls directly to stored procedures via the DAL  

3. **Data Access Layer (DAL)**  
   - Uses ADO.NET to connect to SQL Server  
   - Executes stored procedures and returns results  

4. **Database Layer**  
   - All CRUD operations and business rules in stored procedures  
   - Relational schema with indexes and constraints for performance

---

## ⚙ Features
- Centralized business rules in the database
- Secure and parameterized queries
- Clear separation of application layers
- High performance due to database-level processing

---

## 🚀 How to Run
1. Clone the repository.
2. Set up the SQL Server database using the provided scripts.
3. Configure the connection string in the `ConnectionString Class`.
4. Build and run the project from Visual Studio or via the CLI.

---

## 📌 Author
**Ahmed Esam Abuhussien**  
Learning Project `(My First Api)` — Layered Architecture with Stored Procedures
