# Clothing Store Management Application [Admin Dashboard] - Three-Tier Architecture

## Overview  
This project implements a **Three-Tier Architecture** for a clothing store management system, featuring:  
- **Presentation Tier**: Responsive web UI (HTML/CSS/JavaScript) for product/category management.  
- **Application Tier**: ASP.NET Core Web API with business logic, validation, and RESTful endpoints.  
- **Data Tier**: SQL Server database for persistent storage, managed via Entity Framework Core.  

## Features  
- **Product Management**: CRUD operations for clothing products.  
- **Category Management**: Add/remove product categories.  
- **RESTful API**: Secure endpoints for data operations.  
- **Validation**: Input validation using FluentValidation.  
- **Error Handling**: Global middleware for consistent error responses.  

---

## Technologies  
- **Frontend**: HTML, CSS, JavaScript (Fetch API)  
- **Backend**: ASP.NET Core, Entity Framework Core, FluentValidation, AutoMapper
- **Database**: SQL Server  
- **Tools**: Swagger (testing)  

---

## Setup Instructions  

### Prerequisites
- .NET 8 SDK
- SQL Server
- Web browser

### Steps  
1. **Clone the Repository**  
   ```bash  
   git clone https://github.com/wyndale/Three-Tier.git
   ```
2. **Database Setup**
   - Update connection string in `server/appsettings.json`:
     ```json
     "ConnectionStrings": {
       "ClothingStoreConnection": "Server=your-server;Database=ClothingStoreDB;Trusted_Connection=True;"
     }
     ```
   - Run migrations:
     ```bash
     cd server
     dotnet ef database update
     ```
3. **Run the Server**
   ```bash
   dotnet run
   ```
4. **Access the UI**
   - Open `client/` in a IDE like VS Code then run
   - The API will be running at `https://localhost:5005`
  
### Testing the API

Endpoints:
- `GET /api/Product` - List all products
- `GET /api/Product/{id}` - Fetch product by ID
- `POST /api/Product` - Create new product
- `PUT /api/Product/{id}` - Update product
- `DELETE /api/Product/{id}` - Delete product

- `GET /api/Category` - List all categories
- `GET /api/Category/{id}` - Fetch category by ID
- `POST /api/Category` - Create new category
- `PUT /api/Category/{id}` - Update category
- `DELETE /api/Category/{id}` - Delete category
  
## Project Structure
- `server/`: ASP.NET Core backend
- `client/`: Web interface
- `docs/`: report and diagrams
