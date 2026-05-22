# CarRentalConsole

A small C# console application for learning basic application structure, Entity Framework Core, SQLite persistence, services, interfaces, and simple menu-driven workflows.

This repository is for learning purposes. The code is intentionally simple and is still evolving.

## Tech Stack

- C#
- .NET 10
- Entity Framework Core
- SQLite
- Console application architecture

## Project Structure

```text
Controllers/   Console flow and menu handling
Data/          EF Core DbContext and database initialization
Enums/         Menu option and screen enums
Helpers/       Console input and view factory helpers
Interfaces/    Service and view contracts
Models/        Car, Customer, and Rental entities
Services/      Business/data access operations
Views/         Console menu rendering
```

## Setup

Make sure the .NET SDK is installed, then restore and run the project:

```bash
dotnet restore
dotnet run
```

The app uses SQLite with this connection string:

```text
Data Source=car-rental.db
```

The database is created automatically when the app starts by using `EnsureCreated()`.

## Database Notes

This project does not currently use migrations.

Because of that, if you change the models and the database already exists, SQLite will not automatically update the existing schema. During development, the simplest option is to delete `car-rental.db` and run the app again.

The app currently defines these tables through EF Core models:

- `Cars`
- `Customers`
- `Rentals`

The initial car list is seeded in `AppDbContext`.

## Current Functionality

When the app starts, it displays a main menu:

```text
1. View Available Cars
2. Rent a Car
3. Return a Car
4. Exit
```

### View Available Cars

Shows cars where `IsAvailable` is `true`.

### Rent a Car

The rental flow asks the user to:

1. Select a car.
2. Enter a start date.
3. Enter an end date.
4. Enter an email address.

If the email does not already belong to a customer, the app creates a new customer.

After that, it creates a rental connected to:

- the selected car
- the customer
- the start and end dates
- the rental duration
- the total cost

Once a rental is created, the selected car is marked as unavailable.

### Return a Car

The menu option exists, but the return flow is not implemented yet.

## Entity Relationships

The main relationship is:

```text
Customer 1 -> many Rentals
Car      1 -> many Rentals
Rental   1 -> one Customer
Rental   1 -> one Car
```

`Rental` stores the foreign keys:

- `CustomerId`
- `CarId`

