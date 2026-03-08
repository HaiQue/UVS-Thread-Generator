UVS Multi-Thread Data Engine
A WPF application designed to demonstrate robust multi-threading, database persistence, and modern UI/UX principles.

Key Features
1. Dynamic Multi-threading: Supports 2 to 15 independent background threads, each generating random data at varying intervals.
2. Real-time Data Persistence: All generated data is saved to a Microsoft SQL Server database using Entity Framework Core.
3. Modern UI: Built with Material Design 2, providing a clean, responsive, and intuitive interface.
4. Unit Tested: Includes an xUnit test suite ensuring business logic and validation integrity.

Technical Stack
Platform: .NET 10.0 Windows.
Pattern: MVVM (Model-View-ViewModel) using the CommunityToolkit.Mvvm.
ORM: Entity Framework Core (SQL Server).
UI Library: Material Design in XAML Toolkit.
Testing: xUnit.

Requirements & Setup
SQL Server: The application expects a local SQL Server Express instance at localhost\SQLEXPRESS.
Database Auto-Creation: No manual SQL scripts are required. The application will automatically create the JuniorTaskDb and necessary tables upon the first successful run using db.Database.EnsureCreated().
Trust Certificate: The connection string includes TrustServerCertificate=True to facilitate immediate local development without SSL configuration issues.

Running Tests
Open the solution in Visual Studio.
Navigate to Test > Test Explorer.
Click Run All to verify thread validation and data generation logic.