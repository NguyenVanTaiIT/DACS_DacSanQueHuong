# E-commerce Website for Local Specialties (ASP.NET Core MVC)
![Screenshot 2025-03-29 124823](https://github.com/user-attachments/assets/843a22d7-27ca-4e4e-ba67-0b9d60458611)
![Screenshot 2025-03-29 124848](https://github.com/user-attachments/assets/46b21489-611e-4b43-9b6f-b744f8478f7b)
![Screenshot 2025-03-29 124900](https://github.com/user-attachments/assets/68105cf0-325f-4294-bcbe-519d18c63487)
![Screenshot 2025-03-29 125640](https://github.com/user-attachments/assets/c6bc26b2-cf5c-41f1-a69d-d388eba33eab)


## Technologies Used
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **Bootstrap** (for UI design)
- **SQL Server** (database)
- **VNPay API** (for payment processing)

## Features

### 1. **Authentication & Authorization**
- User registration & login
- Role-based access control (Admin & User)

### 2. **Product Management**
- CRUD operations for local specialties (Admin only)
- View product details

### 3. **Shopping Cart**
- Add/remove items from the cart
- Update item quantity

### 4. **Order & Payment Processing**
- Checkout process
- Online payment integration using **VNPay API**

### 5. **Sales Analytics & Reporting**
- Revenue statistics dashboard for admins
- Order management & tracking

## Installation & Setup

### Prerequisites
- .NET SDK (latest version)
- SQL Server
- Visual Studio / VS Code

### Steps to Run the Project
1. Clone this repository:
   ```sh
   git clone <repository_url>
   ```
2. Navigate to the project folder:
   ```sh
   cd <project_folder>
   ```
3. Set up the database:
   ```sh
   dotnet ef database update
   ```
4. Run the application:
   ```sh
   dotnet run
   ```

## Contribution
Feel free to contribute by submitting pull requests or opening issues.

## License
This project is licensed under the MIT License.

