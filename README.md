# CarRentalConsole

A C# console application for managing a simple car rental workflow with Entity Framework Core, SQLite, and MSTest.

The application uses a menu-driven console interface where users can view available cars, rent a car, and return a rented car.

## Tech Stack

- C#
- .NET 10
- Entity Framework Core
- SQLite
- MSTest

## Solution Structure

```text
CarRentalConsole/
  CarRentalConsole.slnx
  README.md
  .gitignore

  CarRentalConsole/
    CarRentalConsole.csproj
    Program.cs
    Controllers/
    Data/
    Enums/
    Helpers/
    Interfaces/
    Migrations/
    Models/
    Services/
    Views/

  CarRentalConsole.Tests/
    CarRentalConsole.Tests.csproj
```

## Projects

### CarRentalConsole

The main console application.

Main responsibilities:

- render console menus
- read user input
- manage rental workflows
- persist cars, customers, and rentals with EF Core
- use SQLite as the local database

### CarRentalConsole.Tests

The MSTest project for automated tests.

The test project references the main application project:

```xml
<ProjectReference Include="..\CarRentalConsole\CarRentalConsole.csproj" />
```

## Getting Started

Run these commands from the solution root:

```powershell
dotnet restore
dotnet build
```

Run the application:

```powershell
dotnet run --project CarRentalConsole\CarRentalConsole.csproj
```

Run the tests:

```powershell
dotnet test
```

## Database

The application uses SQLite through Entity Framework Core.

Current connection string:

```text
Data Source=car-rental.db
```

The database schema is managed with EF Core migrations.

Apply migrations:

```powershell
dotnet ef database update --project CarRentalConsole\CarRentalConsole.csproj
```

Create a new migration after changing the EF model:

```powershell
dotnet ef migrations add MigrationName --project CarRentalConsole\CarRentalConsole.csproj
```

The initial car list is seeded in `AppDbContext`.

## Current Functionality

When the app starts, it displays:

```text
1. View Available Cars
2. Rent a Car
3. Return a Car
4. Exit
```

### View Available Cars

Displays cars where `IsAvailable` is `true`.

### Rent a Car

The rental flow asks the user to:

1. Select a car.
2. Enter a start date.
3. Enter an end date.
4. Enter an email address.

If the email does not belong to an existing customer, the app creates a new customer.

When a rental is created, the selected car is marked as unavailable.

### Return a Car

The return flow lists open rentals and allows the user to conclude a rental. When a rental is returned, the return date is saved and the car is marked as available again.

## Domain Model

The core entities are:

- `Car`
- `Customer`
- `Rental`

Relationships:

```text
Customer 1 -> many Rentals
Car      1 -> many Rentals
Rental   1 -> one Customer
Rental   1 -> one Car
```

`Rental` stores:

- `CustomerId`
- `CarId`
- `StartDate`
- `EndDate`
- `ReturnDate`
- `Duration`
- `TotalCost`

## Local Files

Generated files such as `bin/`, `obj/`, `TestResults/`, and local SQLite database files should stay out of source control.
