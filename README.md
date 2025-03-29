# E-commerce Website for Local Specialties (ASP.NET Core MVC)
![image](https://github.com/user-attachments/assets/715880b8-8330-4800-8060-824a6586d3e7)
![image](https://github.com/user-attachments/assets/3e206af1-558a-4599-874f-b0a0093e1740)
![image](https://github.com/user-attachments/assets/47f1910c-237b-4f14-bbf2-9cee73ccc608)

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

