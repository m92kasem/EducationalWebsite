# EducationalWebsite

Educational quizzes Test Website

## Key Features

- **User Registration and Login**: Secure registration and login system with role-based access (Admin and Customer).
- **Quiz Management**: Admins can create, edit, and manage quizzes and questions.
- **Quiz Taking**: Users can select and take quizzes, receiving instant feedback on their performance.
- **Results Evaluation**: Automatic evaluation of quiz answers with detailed results and explanations.
- **Responsive Design**: User-friendly interface compatible with various devices and screen sizes.
- **Secure Data Handling**: Secure storage and handling of user data and quiz information.

## Techniques and Technologies Used

### 1. Backend Development with .NET 8

The backend of the application is built using .NET 8, ensuring high performance and scalability. The backend is responsible for handling data operations, business logic, and secure communication between the frontend and the database. Key technologies and techniques used include:

- **ASP.NET Core**: For creating RESTful APIs and handling HTTP requests.
- **MediatR**: Implementing the mediator pattern to manage and decouple the application's command and query responsibilities.
- **AutoMapper**: For mapping between data transfer objects (DTOs) and domain entities, ensuring clean and maintainable code.

### 2. Data Storage with MongoDB

MongoDB is used for data storage, providing a flexible and scalable NoSQL database solution. This choice allows for efficient handling of the data related to users, quizzes, questions, and results.

- **MongoDB.Driver**: A .NET driver for MongoDB used for database operations like CRUD (Create, Read, Update, Delete).

### 3. Frontend Development with Blazor

The frontend is built using Blazor, a framework for building interactive web UIs with C#. Blazor allows for the creation of a dynamic and responsive user interface that communicates seamlessly with the backend.

- **Blazor Pages**: For developing the user interface components and handling client-side logic.

### 4. Clean Architecture and SOLID Principles

The project is structured according to clean architecture and SOLID principles, ensuring that the application is modular, maintainable, and scalable. The architecture separates concerns into different layers:

- **Domain Layer**: Contains core business logic and domain entities.
- **Application Layer**: Implements business logic, application services, and command/query handling.
- **Infrastructure Layer**: Handles data access, repositories, and external services.
- **Web Layer**: Contains the Blazor frontend and API endpoints.
