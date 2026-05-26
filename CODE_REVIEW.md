# Code Review - CarRentalConsole

Reviewed on: 2026-05-25

Scope reviewed:

- EF Core model and relationship design
- Service layer behavior
- Controller/menu flow
- Console view rendering
- Input handling
- Performance and maintainability

Build result:

```text
dotnet build
Build succeeded with 0 warnings and 0 errors.
```

## Overall Assessment

This project is in a good learning stage. The structure is already moving in the right direction: models, services, interfaces, controllers, views, helpers, and data access are separated into clear folders. EF Core is being used in a mostly understandable way, and the app now has the core rental flow: list cars, rent a car, create/find customers, and return a car.

The next level is not "make it more complicated." The next level is making the current design more reliable:

- await async view rendering correctly
- use IDs consistently instead of mixing IDs and list indexes
- keep business rules inside services, not only in controllers
- avoid N+1 database queries in menu rendering
- separate data retrieval from presentation strings as the app grows
- use `decimal` for money calculations

## High Priority Findings

### 1. Async view rendering is not awaited

Location:

- `Helpers/ConsoleViewFactory.cs:35`
- `Helpers/ConsoleViewFactory.cs:38`
- `Program.cs:30`

Current flow:

```csharp
consoleViewFactory.DisplayView(currentScreen);
string? menuSelection = Console.ReadLine();
```

Inside `DisplayView`:

```csharp
view.Display();
```

The problem is that `IView.Display()` returns `Task`, but `ConsoleViewFactory.DisplayView` is `void` and does not await it.

Why this matters:

- `MainMenu` may appear to work because its async code completes synchronously most of the time.
- `AvailableCarsMenu`, `RentCarMenu`, and `ReturnCarMenu` perform async database calls.
- The app can start waiting for user input before the menu has finished rendering.
- This creates timing bugs that feel random.

Tutor note:

When a method returns `Task`, think of it as "work that may still be running." If you call it without `await`, you started the work but did not wait for the result.

Recommended fix:

- Make `ConsoleViewFactory.DisplayView` return `Task`.
- Await `view.Display()`.
- Await `consoleViewFactory.DisplayView(currentScreen)` in `Program.Main`.

### 2. Rent menu displays car IDs, but controller treats input as a list index

Location:

- `Views/RentCarMenu.cs:26`
- `Views/RentCarMenu.cs:28`
- `Controllers/CarController.cs:31`
- `Controllers/CarController.cs:39`

The menu displays:

```csharp
availableCars[i].Id
```

But the controller does this:

```csharp
int carIndex = selectedOption - 1;
Car selectedCar = availableCars[carIndex];
```

This works only while all car IDs are continuous and all cars are available. After car `1` is rented, the available list might start with car `2`, but index `0`. If the user types `2`, the controller selects index `1`, which could be car `3`.

Why this matters:

- Users can rent the wrong car.
- This is a real behavioral bug, not just a style issue.

Tutor note:

Do not mix "database identity" and "screen position." They are different concepts.

Recommended fix:

Pick one approach:

1. Display numbered menu options using `i + 1`, then keep index-based selection.
2. Display real car IDs, then find the selected car by `car.Id == selectedOption`.

For a database-backed app, option 2 is usually better because IDs remain stable even when the list is filtered.

### 3. Return flow says it is returning to main menu, but the screen does not change

Location:

- `Views/ReturnCarMenu.cs:23`
- `Views/ReturnCarMenu.cs:25`
- `Program.cs:32`
- `Controllers/MenuController.cs:49`

When there are no open rentals, the view prints:

```text
No cars are currently rented. Returning to main menu...
```

But the application still stays on `EMenuScreen.ReturnCar` and waits for input. The view has no way to change the screen.

Why this matters:

- The UI tells the user one thing while the app state does another.
- This creates confusing console behavior.

Tutor note:

Views should usually render the current state. Controllers decide what state comes next.

Recommended fix:

- Move the "no open rentals" decision into the controller/menu flow, or
- Let the controller ask the service whether there are open rentals before navigating to the return screen, or
- Make screen transitions explicit instead of hiding them inside view text.

### 4. Rental creation does not enforce business rules inside the service

Location:

- `Services/RentalService.cs:24`
- `Services/RentalService.cs:26`
- `Services/RentalService.cs:33`
- `Services/RentalService.cs:36`
- `Controllers/CarController.cs:44`
- `Controllers/CarController.cs:63`

The controller validates dates and selects from available cars, but `RentalService.CreateRental` only checks whether the car exists.

Missing service-level checks:

- Is the car still available?
- Does the customer exist?
- Is `EndDate` after `StartDate`?
- Is `Duration` greater than 0?
- Is `TotalCost` consistent with duration and daily rate?

Why this matters:

The controller currently protects the happy path, but services are the business boundary. If another controller, test, or future menu calls `CreateRental`, invalid rentals can still be created.

Tutor note:

Controllers answer: "What did the user ask for?"

Services answer: "Is this operation valid for the business?"

Recommended fix:

- Keep user input validation in the controller/helper.
- Move rental business validation into `RentalService.CreateRental`.
- Let the service calculate duration and total cost, or at least verify them.

## Medium Priority Findings

### 5. Returning a car is checked twice and loaded twice

Location:

- `Controllers/CarController.cs:106`
- `Controllers/CarController.cs:113`
- `Controllers/CarController.cs:119`
- `Services/RentalService.cs:57`

The controller loads the rental to check whether it exists and whether it is returned. Then `ConcludeRental` loads it again with `Include(rental => rental.Car)`.

Why this matters:

- Extra database work.
- Business rules are split between controller and service.
- `ConcludeRental` can still be called directly for an already returned rental.

Recommended fix:

Let `ConcludeRental` own the full operation:

- find the rental
- verify it exists
- verify it is not already returned
- load the related car
- set return date
- make the car available
- save changes
- return a clear result

The controller should mostly translate that result into console messages.

### 6. `ConcludeRental` return value is technically correct but not expressive

Location:

- `Services/RentalService.cs:55`
- `Services/RentalService.cs:69`
- `Interfaces/IRentalService.cs:11`

The method returns the result of `SaveChangesAsync()`, which is the number of affected state entries.

Why this matters:

The controller uses this as "success or failure":

```csharp
if (success < 1)
```

But affected row count is not the same as a business outcome. For example:

- rental not found
- rental already returned
- related car missing
- database save failed

All of these deserve different meanings.

Recommended fix:

Return something more intentional later:

- `bool`
- an enum like `RentalReturnResult`
- a small result object with success/message

Since this is a learning app, a simple enum would be a great next step.

### 7. Interface parameter name is misleading

Location:

- `Interfaces/IRentalService.cs:11`

The interface says:

```csharp
Task<int> ConcludeRental(int carId);
```

But the implementation expects a rental ID:

```csharp
public async Task<int> ConcludeRental(int rentalId)
```

Why this matters:

The compiler does not care about parameter names in interface matching, but humans do. This is exactly the kind of small mismatch that causes wrong calls later.

Recommended fix:

Rename the interface parameter to `rentalId`.

### 8. Money is stored as `double`

Location:

- `Models/Rental.cs:25`
- `Controllers/CarController.cs:64`
- `Models/Car.cs:11`

`Car.DailyRate` is `decimal`, which is good for money. But rental total cost is `double`:

```csharp
public double TotalCost { get; set; }
```

And the controller converts:

```csharp
double totalCost = rentalDays * (double)selectedCar.DailyRate;
```

Why this matters:

`double` is a binary floating-point type and can introduce rounding behavior that is bad for money.

Recommended fix:

Use `decimal` for all currency values:

- `Car.DailyRate`
- `Rental.TotalCost`
- total cost calculations

Tutor note:

Use `double` for measurements and scientific values. Use `decimal` for money.

### 9. `GetRentalDetails` returns formatted text from the service

Location:

- `Services/RentalService.cs:72`
- `Services/RentalService.cs:94`
- `Interfaces/IRentalService.cs:9`
- `Views/ReturnCarMenu.cs:33`

Current method:

```csharp
Task<string> GetRentalDetails(int rentalId)
```

This works, especially while learning. But it couples the service layer to presentation formatting.

Why this matters:

The service now decides the exact text shown to the user. Later, if you want:

- a different console layout
- JSON output
- tests around the rental data
- a GUI

you will need to undo that coupling.

Recommended next step:

When you are ready, return a named read model such as `RentalDetails`, `RentalSummary`, or `OpenRentalListItem`. The view can format it into a string.

Tutor note:

This is the same idea we discussed earlier:

- services should return data
- views should format data

For now, keeping `string` is acceptable for a small console app, but it is worth knowing the tradeoff.

### 10. `GetOpenRentals` plus `GetRentalDetails` creates an N+1 query pattern

Location:

- `Views/ReturnCarMenu.cs:21`
- `Views/ReturnCarMenu.cs:33`
- `Services/RentalService.cs:17`
- `Services/RentalService.cs:72`

The return menu first fetches all open rentals:

```csharp
List<Rental> openRentals = await rentalService.GetOpenRentals();
```

Then inside a loop it calls:

```csharp
await rentalService.GetRentalDetails(openRentals[i].Id)
```

If there are 20 open rentals, this becomes:

- 1 query to get rentals
- 20 more queries to get details

That is the N+1 problem.

Recommended fix:

Create one service method that returns all open rental details in a single query. It can project from `Rentals` to the exact fields the menu needs.

Tutor note:

N+1 means: "I asked for a list, then accidentally made one query per item." It is one of the most common ORM performance issues.

## EF Core and Data Modeling Notes

### 11. Required relationships are mostly modeled correctly

Location:

- `Models/Rental.cs:9`
- `Models/Rental.cs:10`
- `Models/Rental.cs:12`
- `Models/Rental.cs:13`

This is good:

```csharp
public int CustomerId { get; set; }
public Customer? Customer { get; set; }

public int CarId { get; set; }
public Car? Car { get; set; }
```

The non-nullable `int` foreign keys make the relationship required at the database/model level. The nullable navigation properties are honest because the related objects may not be loaded into memory.

Recommended enhancement:

Add explicit Fluent API relationship configuration later if you want the model to teach future readers:

- one customer has many rentals
- one car has many rentals
- each rental has one required customer
- each rental has one required car

EF can infer this today, but explicit configuration becomes useful as the project grows.

### 12. `[Required]` on non-nullable value types does not add much

Location:

- `Models/Rental.cs:15`
- `Models/Rental.cs:17`
- `Models/Rental.cs:19`
- `Models/Rental.cs:24`

Properties like `int`, `DateOnly`, and `double` are already non-nullable value types.

Examples:

```csharp
public int Duration { get; set; }
public DateOnly StartDate { get; set; }
public double TotalCost { get; set; }
```

`[Required]` mainly matters for nullable/reference properties. It does not prevent invalid defaults like:

- `Duration = 0`
- default `DateOnly`
- `TotalCost = 0`

Recommended fix:

Use business validation for meaningful rules:

- duration must be greater than 0
- end date must be after start date
- total cost must be greater than 0

### 13. No unique constraint on customer email

Location:

- `Models/Customer.cs:9`
- `Services/CustomerService.cs:31`
- `Services/CustomerService.cs:34`

The app looks customers up by email, but the database does not enforce uniqueness.

Why this matters:

Two rows can have the same email. Then `FirstOrDefaultAsync` returns whichever one the database finds first.

Recommended fix:

Add a unique index for `Customer.Email` when you start using migrations.

Also consider normalizing emails before saving/searching:

- trim
- lowercase
- maybe store a normalized email column

### 14. `EnsureCreated` is fine for learning, but migrations are the next step

Location:

- `Data/DatabaseInitializer.cs:18`
- `README.md:43`
- `README.md:47`

`EnsureCreated()` is okay for early learning. But it bypasses migrations.

Why this matters:

When your model changes, the database schema will not evolve automatically. You may need to delete the database and recreate it.

Recommended next step:

When you feel comfortable with the current EF basics, move to migrations:

- `dotnet ef migrations add InitialCreate`
- `dotnet ef database update`

Do not rush this if you are still learning relationships. But it is the right path for a real deliverable app.

### 15. Connection string is hard-coded and relative

Location:

- `Data/AppDbContext.cs:12`
- `Data/AppDbContext.cs:14`

Current connection string:

```csharp
Data Source=car-rental.db
```

Why this matters:

The physical database file depends on the working directory used to start the app. This can create confusion where your DB browser is looking at one file and the app is using another.

Recommended fix:

Later, move configuration outside `OnConfiguring`, or at least build an explicit path so you always know where the database file lives.

## Architecture and Maintainability

### 16. DbContext lifetime is too broad for a larger app

Location:

- `Program.cs:12`
- `Program.cs:16`
- `Program.cs:17`
- `Program.cs:18`

The app creates one `AppDbContext` and reuses it for the entire application lifetime.

Why this matters:

EF DbContext is designed as a unit-of-work object. Long-lived contexts can:

- track more and more entities over time
- keep stale data in memory
- make behavior harder to reason about

For a small console app, this is acceptable. For the next learning step, consider creating a fresh context per operation or using dependency injection.

Also, the context should be disposed when the app exits.

### 17. Views currently fetch data

Location:

- `Views/AvailableCarsMenu.cs:24`
- `Views/RentCarMenu.cs:18`
- `Views/ReturnCarMenu.cs:21`
- `Views/ReturnCarMenu.cs:33`

The views call services directly.

This is okay for a small console app, but it blurs responsibilities:

- views render
- controllers coordinate workflow
- services run business/data operations

Recommended direction:

Eventually, have controllers fetch the data and pass it to views for rendering. This makes views easier to test and keeps screen decisions in one place.

### 18. `CarController` is doing more than car control

Location:

- `Controllers/CarController.cs:7`
- `Controllers/CarController.cs:22`
- `Controllers/CarController.cs:99`

This controller handles:

- renting cars
- returning cars
- customer creation/lookup
- rental creation

The name `CarController` is narrower than what it actually does.

Recommended fix:

Consider a name like:

- `RentalController`
- `RentalWorkflowController`
- `CarRentalController`

The best name is the one that describes the workflow, not just one entity.

### 19. Some async methods do not need to be async

Location:

- `Views/MainMenu.cs:16`
- `Views/MainMenu.cs:39`
- `Views/NotFoundMenu.cs:7`

Some view methods are async but do not await anything. This is not a major problem, but it adds noise.

Recommended fix:

Either:

- keep the interface async for consistency and return completed tasks where needed, or
- split sync and async rendering

For this app, keeping `IView.Display()` async is fine because several views do call async services. The important fix is to await the returned task.

### 20. Enums are in the global namespace

Location:

- `Enums/EMainMenuOption.cs:1`
- `Enums/EMenuScreen.cs:1`

Most files use namespaces, but the enum files do not.

Why this matters:

It works, but it is inconsistent and can create naming collisions later.

Recommended fix:

Put enums inside the project namespace, likely `CarRentalConsole.Enums`, then import that namespace where needed.

## Performance Notes

### Best quick wins

1. Add `AsNoTracking()` for read-only entity queries.

Good candidates:

- `Services/CarService.cs:19`
- `Services/RentalService.cs:19`
- `Services/RentalService.cs:49`

Why:

If you are only displaying data, EF does not need to track those entities for updates.

2. Replace the return menu N+1 query with one projection.

Good target:

- `Views/ReturnCarMenu.cs:21`
- `Views/ReturnCarMenu.cs:33`

Why:

One query that fetches all open rental details is cheaper and easier to reason about.

3. Avoid fetching the same rental twice during return.

Good target:

- `Controllers/CarController.cs:106`
- `Services/RentalService.cs:57`

Why:

The service can own the complete operation and return a meaningful result.

## Good Things Already Present

### Folder structure is readable

The separation into `Models`, `Services`, `Controllers`, `Views`, `Helpers`, `Interfaces`, and `Data` is a good learning structure.

### EF foreign keys are heading in the right direction

`Rental` has `CustomerId` and `CarId`, which is the correct foundation for required relationships.

### Query projection in `GetRentalDetails` is a good learning step

Location:

- `Services/RentalService.cs:74`
- `Services/RentalService.cs:76`

Using projection to fetch only the needed data is exactly the right instinct for read/display operations.

### Service interfaces are useful for practice

The interfaces are a bit more formal than a tiny app strictly needs, but they are helpful for learning dependency direction and future tests.

### Input parsing is centralized

Location:

- `Helpers/ConsoleInputReader.cs`

Having reusable methods for dates and email is better than duplicating `Console.ReadLine()` parsing everywhere.

## Suggested Next Steps

### Step 1: Fix correctness bugs

Do these first:

1. Await view rendering end-to-end.
2. Fix the car selection mismatch: ID vs index.
3. Make the return screen behave honestly when there are no open rentals.
4. Rename `ConcludeRental(int carId)` to `ConcludeRental(int rentalId)` in the interface.

### Step 2: Strengthen business rules

Move core rental rules into `RentalService`:

- car must exist
- car must be available
- customer must exist
- end date must be after start date
- total should be calculated consistently
- already returned rentals cannot be concluded again

### Step 3: Clean up data shapes

When you are ready:

- replace `GetRentalDetails` string return with a named read model
- create one method for open rental list details
- let views format data instead of services

### Step 4: Improve EF maturity

After the app behavior feels stable:

- add migrations
- add unique index on customer email
- make the database path/config explicit
- use `decimal` for all money values
- use `AsNoTracking()` on read-only queries

## Final Tutor Notes

The most important concept in this codebase right now is boundary placement.

Ask this question for each piece of logic:

```text
Is this user interaction, business rule, data access, or display formatting?
```

Then place it accordingly:

- user input and menu decisions: controller/helper
- rental rules: service/model
- database queries: service/repository-style methods
- text output: view

You do not need a huge architecture. You just need each layer to have a clear job. This project is close enough that a few focused corrections will make it feel much more solid.
