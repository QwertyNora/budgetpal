# BudgetTracker

A personal budget tracking application built with .NET 9 using clean architecture principles and layered design.

## Project Structure

```
BudgetTracker/
├── BudgetTracker.API/              # ASP.NET Core Web API
│   ├── Controllers/                # API endpoints
│   ├── Middleware/                 # Custom middleware (exception handling)
│   └── Program.cs                  # Application entry point
├── BudgetTracker.Core/             # Domain layer
│   ├── Entities/                   # Domain entities (Transaction, Category)
│   ├── Enums/                      # Enumerations
│   └── Interfaces/                 # Repository interfaces
├── BudgetTracker.Application/      # Application layer
│   ├── DTOs/                       # Data transfer objects
│   ├── Interfaces/                 # Service interfaces
│   └── Services/                   # Business logic implementation
└── BudgetTracker.Infrastructure/   # Infrastructure layer
    ├── Data/                       # DbContext
    └── Repositories/               # Repository implementations
```

## Prerequisites

-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   SQLite (included with .NET)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd BudgetTracker
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Install EF Core Tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef
```

### 4. Apply Database Migrations

Create the initial migration:

```bash
dotnet ef migrations add InitialCreate -p BudgetTracker.Infrastructure -s BudgetTracker.API
```

Update the database:

```bash
dotnet ef database update -p BudgetTracker.Infrastructure -s BudgetTracker.API
```

This will create the SQLite database (`budgettracker.db`) in the API project directory with all necessary tables and seed data.

### 5. Run the Application

```bash
cd BudgetTracker.API
dotnet run
```

The API will start and be available at:

-   **HTTPS**: `https://localhost:7xxx` (port varies)
-   **HTTP**: `http://localhost:5xxx` (port varies)

### 6. Access Swagger Documentation

Navigate to the Swagger UI for interactive API documentation:

```
https://localhost:<port>/swagger
```

## API Endpoints

### Transactions

| Method | Endpoint                                     | Description                | Request Body           |
| ------ | -------------------------------------------- | -------------------------- | ---------------------- |
| GET    | `/api/transactions?pageNumber=1&pageSize=20` | Get paginated transactions | -                      |
| GET    | `/api/transactions/{id}`                     | Get transaction by ID      | -                      |
| POST   | `/api/transactions`                          | Create new transaction     | `CreateTransactionDto` |
| PUT    | `/api/transactions/{id}`                     | Update transaction         | `UpdateTransactionDto` |
| DELETE | `/api/transactions/{id}`                     | Delete transaction         | -                      |

### Categories

| Method | Endpoint               | Description            | Request Body        |
| ------ | ---------------------- | ---------------------- | ------------------- |
| GET    | `/api/categories`      | Get all categories     | -                   |
| GET    | `/api/categories/{id}` | Get category by ID     | -                   |
| POST   | `/api/categories`      | Create custom category | `CreateCategoryDto` |
| PUT    | `/api/categories/{id}` | Update custom category | `UpdateCategoryDto` |
| DELETE | `/api/categories/{id}` | Delete custom category | -                   |

### Statistics

| Method | Endpoint                      | Description                | Query Parameters                  |
| ------ | ----------------------------- | -------------------------- | --------------------------------- |
| GET    | `/api/statistics`             | Get overall statistics     | `startDate`, `endDate` (optional) |
| GET    | `/api/statistics/by-category` | Get statistics by category | `startDate`, `endDate` (optional) |
| GET    | `/api/statistics/monthly`     | Get monthly statistics     | `year` (optional)                 |

## Example API Requests

### Create a Transaction

```json
POST /api/transactions
{
  "date": "2025-11-25T10:00:00Z",
  "description": "Grocery shopping",
  "amount": 85.50,
  "type": 1,
  "categoryId": 1,
  "notes": "Weekly groceries"
}
```

**Transaction Types:**

-   `0` = Income
-   `1` = Expense

### Create a Custom Category

```json
POST /api/categories
{
  "name": "Crypto Trading",
  "type": 0
}
```

**Category Types:**

-   `0` = Income
-   `1` = Expense
-   `2` = Both

### Get Statistics

```
GET /api/statistics?startDate=2025-01-01&endDate=2025-12-31
```

Response:

```json
{
    "totalIncome": 50000.0,
    "totalExpenses": 35000.0,
    "balance": 15000.0,
    "transactionCount": 145
}
```

## Seeded Categories

The application comes with 23 predefined categories that cannot be modified or deleted:

### Income Categories (5)

-   Salary
-   Freelance
-   Investment Returns
-   Gifts
-   Other Income

### Expense Categories (17)

-   Groceries
-   Dining Out
-   Transportation
-   Utilities
-   Rent/Mortgage
-   Healthcare
-   Insurance
-   Entertainment
-   Shopping
-   Education
-   Travel
-   Personal Care
-   Subscriptions
-   Gifts & Donations
-   Home Maintenance
-   Pet Care
-   Other Expenses

### Both Categories (1)

-   Miscellaneous

**Note:** You can create custom categories, which can be edited or deleted (as long as they have no associated transactions).

## Validation Rules

### Transactions

-   Date cannot be in the future
-   Amount must be greater than 0
-   Description is required (max 200 characters)
-   CategoryId must reference an existing category
-   Notes are optional (max 500 characters)

### Categories

-   Name must be unique (case-insensitive)
-   Only custom categories can be updated or deleted
-   Categories with existing transactions cannot be deleted

## Technology Stack

-   **Framework**: .NET 9
-   **Database**: SQLite with Entity Framework Core 9
-   **API Documentation**: Swagger/OpenAPI
-   **Architecture**: Clean Architecture with layered design
-   **Pattern**: Repository Pattern with Dependency Injection

## License

[Add your license here]
