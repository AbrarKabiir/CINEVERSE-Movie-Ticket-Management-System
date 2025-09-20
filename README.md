# CINEVERSE---A-Movie-Ticket-Management-System
Cineverse is a .NET 8 Windows Forms movie-ticket booking app backed by SQL Server. Users browse current and upcoming films, watch trailers, pick seats, pay, view booking history, buy merchandise, and post reviews. Admins manage movies, users, bookings, and revenue; vendors manage inventory with stock and sales tracking.

Cineverse is a Windows Forms application built with C# and .NET Framework 4.7.2. It provides a complete solution for movie ticket booking, merchandise management, and user administration for cinema businesses.

## Features

- **User Registration & Login:** Secure authentication for users and admins.
- **Movie Listings:** Browse currently showing and upcoming movies with details.
- **Ticket Booking:** Select movies, showtimes, and book tickets.
- **Booking History:** View past ticket purchases and transaction details.
- **Merchandise Store:** Browse and purchase movie-related merchandise.
- **Admin Panel:** Manage users, movies, merchandise, and view transactions.
- **Vendor Management:** Vendors can add, edit, and delete merchandise items.
- **Review System:** Users can write and view reviews for movies.
- **Data Visualization:** Charts and statistics for sales and revenue.

## Technologies Used

- C# 7.3
- .NET Framework 4.7.2
- Windows Forms (WinForms)
- SQL Server (Database)
- Guna UI2 & MaterialSkin (UI Libraries)

## Getting Started

1. **Clone the repository:**
2. **Open the solution in Visual Studio 2022.**

3. **Restore NuGet packages** if prompted.

4. **Set up the database:**
- Ensure you have SQL Server installed.
- Create a database named `CINE_VERSE_DB`.
- Import the required tables and data (see `dbo.User_Details.sql` and other SQL files if provided).

5. **Update the connection string:**
- In files like `VEndor2.cs`, update the `ConnString` variable if your SQL Server name or authentication is different.

6. **Build and run the project.**
- Press `F5` in Visual Studio to start the application.

---

## Database

- The application uses SQL Server for data storage.
- Ensure the database `CINE_VERSE_DB` is created and tables are set up as per the schema.

### Prerequisites
- Windows 10 or later  
- Visual Studio 2022  
- .NET Framework 4.7.2  
- SQL Server 2019 or later  
- NuGet packages: **Guna UI2**, **MaterialSkin**
   
## Screenshots



## License

This project is licensed under the MIT License.

## Author

Developed by Abrar Kabir.

---

For any issues or contributions, please open an issue or submit a pull request on GitHub.
