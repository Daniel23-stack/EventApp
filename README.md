# Smart Inventory Management System

A comprehensive, enterprise-grade inventory management system built with .NET 8 Web API, MySQL, and Vue.js 3. Features real-time inventory tracking, automated restocking alerts, advanced analytics, and a modern, responsive user interface.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Vue.js](https://img.shields.io/badge/Vue.js-3.0-4FC08D?logo=vue.js)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql)
![License](https://img.shields.io/badge/License-MIT-green)

## 🚀 Features

### Core Functionality
- **User Authentication & Authorization**
  - JWT-based authentication
  - Role-based access control (Admin, Manager, User)
  - Secure password hashing with BCrypt
  - Session management

- **Real-Time Inventory Tracking**
  - Live inventory updates via SignalR WebSockets
  - Multi-location inventory management
  - Inventory history tracking
  - Low stock alerts

- **Product Management**
  - Complete CRUD operations for products
  - SKU management
  - Category organization
  - Product search and filtering

- **Supplier Management**
  - Supplier information management
  - Product-supplier relationships
  - Contact information tracking

- **Order Management**
  - Order creation and tracking
  - Order status workflow (Pending → Confirmed → Shipped → Delivered)
  - Order history
  - Multi-item order support

- **Automated Alert System**
  - Low stock alerts
  - Reorder level notifications
  - Background service for automated checks
  - Real-time alert notifications

- **Advanced Analytics Dashboard**
  - **Sales Forecasting**: Predictive analytics using time series analysis
  - **Inventory Turnover Rates**: Calculate and visualize product turnover
  - **ABC Analysis**: Categorize products by value and importance
  - **Product Performance Metrics**: Sales, revenue, and stock value analysis
  - **Inventory Trends**: Historical inventory tracking and visualization

## 🏗️ Architecture

This project follows **Clean Architecture** principles with **SOLID** design patterns:

```
EvenApp/
├── src/
│   ├── EvenApp.Domain/              # Domain Layer
│   │   ├── Entities/                 # Domain entities
│   │   └── Interfaces/              # Repository interfaces
│   ├── EvenApp.Application/         # Application Layer
│   │   ├── DTOs/                    # Data Transfer Objects
│   │   ├── Services/                # Business logic & use cases
│   │   └── Interfaces/               # Service interfaces
│   ├── EvenApp.Infrastructure/      # Infrastructure Layer
│   │   ├── Data/                    # Database connection factory
│   │   ├── Repositories/            # Dapper repository implementations
│   │   ├── Hubs/                    # SignalR hubs
│   │   └── Services/                 # Background services
│   └── EvenApp.API/                 # Presentation Layer
│       ├── Controllers/             # REST API controllers
│       ├── Program.cs               # Startup configuration
│       └── appsettings.json        # Configuration
├── frontend/                        # Vue.js 3 Application
│   ├── src/
│   │   ├── views/                   # Page components
│   │   ├── components/              # Reusable components
│   │   ├── services/                # API & SignalR clients
│   │   ├── stores/                  # Pinia state management
│   │   └── router/                  # Vue Router configuration
├── database/                        # Database scripts
│   ├── schema.sql                   # Database schema
│   └── seed.sql                     # Seed data
└── tests/                           # Unit & integration tests
```

### Layer Responsibilities

- **Domain Layer**: Core business entities and repository interfaces (no dependencies)
- **Application Layer**: Use cases, DTOs, and business logic (depends on Domain)
- **Infrastructure Layer**: Data access, external services (depends on Application & Domain)
- **Presentation Layer**: Controllers, middleware, API configuration (depends on all layers)

## 🛠️ Technology Stack

### Backend
- **.NET 8** - Latest .NET framework
- **Dapper** - Lightweight ORM for high-performance data access
- **MySQL 8.0+** - Reliable relational database
- **SignalR** - Real-time WebSocket communication
- **JWT Bearer Authentication** - Secure token-based auth
- **BCrypt.Net** - Password hashing
- **Swashbuckle** - Swagger/OpenAPI documentation

### Frontend
- **Vue.js 3** - Progressive JavaScript framework
- **Vue Router** - Client-side routing
- **Pinia** - State management
- **Axios** - HTTP client
- **SignalR Client** - Real-time communication
- **Chart.js** - Data visualization (ready for analytics)

### Architecture Patterns
- **Clean Architecture** - Separation of concerns
- **SOLID Principles** - Maintainable, extensible code
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Loose coupling

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.0+](https://dev.mysql.com/downloads/mysql/)
- [Node.js 18+](https://nodejs.org/)
- [npm](https://www.npmjs.com/) or [yarn](https://yarnpkg.com/)
- Git

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Daniel23-stack/EventApp.git
cd EventApp
```

### 2. Database Setup

#### Create the Database

```bash
mysql -u root -p < database/schema.sql
```

#### Seed Initial Data (Optional)

```bash
mysql -u root -p < database/seed.sql
```

#### Update Connection String

Edit `src/EvenApp.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EvenAppInventory;User=root;Password=yourpassword;"
  }
}
```

### 3. Backend Setup

#### Restore Dependencies

```bash
dotnet restore
```

#### Build the Solution

```bash
dotnet build
```

#### Run the API

```bash
cd src/EvenApp.API
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:7000`
- **Swagger UI**: `https://localhost:7000/swagger`
- **SignalR Hub**: `https://localhost:7000/inventoryhub`

### 4. Frontend Setup

#### Install Dependencies

```bash
cd frontend
npm install
```

#### Update API Configuration (if needed)

Edit `frontend/src/services/api.js` if your API runs on a different port:

```javascript
const api = axios.create({
  baseURL: 'https://localhost:7000/api',
  // ...
});
```

#### Start Development Server

```bash
npm run dev
```

The frontend will be available at `http://localhost:5173` (or the port shown in terminal).

## 📖 API Documentation

### Authentication Endpoints

#### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "SecurePassword123!",
  "role": "User"
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "john_doe",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "john_doe",
  "email": "john@example.com",
  "role": "User"
}
```

### Product Endpoints

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/sku/{sku}` - Get product by SKU
- `GET /api/products/search?term={term}` - Search products
- `GET /api/products/category/{category}` - Get products by category
- `POST /api/products` - Create product (Manager/Admin)
- `PUT /api/products/{id}` - Update product (Manager/Admin)
- `DELETE /api/products/{id}` - Delete product (Admin)

### Inventory Endpoints

- `GET /api/inventory` - Get all inventory
- `GET /api/inventory/product/{productId}` - Get inventory by product
- `GET /api/inventory/location/{location}` - Get inventory by location
- `GET /api/inventory/low-stock` - Get low stock items
- `PUT /api/inventory` - Update inventory (Manager/Admin)
- `POST /api/inventory/adjust` - Adjust inventory (Manager/Admin)

### Supplier Endpoints

- `GET /api/suppliers` - Get all suppliers
- `GET /api/suppliers/{id}` - Get supplier by ID
- `GET /api/suppliers/search?term={term}` - Search suppliers
- `POST /api/suppliers` - Create supplier (Manager/Admin)
- `PUT /api/suppliers/{id}` - Update supplier (Manager/Admin)
- `DELETE /api/suppliers/{id}` - Delete supplier (Admin)

### Order Endpoints

- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/supplier/{supplierId}` - Get orders by supplier
- `GET /api/orders/status/{status}` - Get orders by status
- `POST /api/orders` - Create order (Manager/Admin)
- `PUT /api/orders/{id}/status` - Update order status (Manager/Admin)

### Alert Endpoints

- `GET /api/alerts` - Get active alerts
- `GET /api/alerts/{id}` - Get alert by ID
- `GET /api/alerts/product/{productId}` - Get alerts by product
- `POST /api/alerts` - Create alert (Manager/Admin)
- `POST /api/alerts/{id}/resolve` - Resolve alert (Manager/Admin)
- `POST /api/alerts/check-reorder-levels` - Check reorder levels (Manager/Admin)

### Analytics Endpoints

- `GET /api/analytics/sales-forecast/{productId}?days={days}` - Get sales forecast
- `GET /api/analytics/turnover-rates?startDate={date}&endDate={date}` - Get turnover rates
- `GET /api/analytics/abc-analysis` - Get ABC analysis
- `GET /api/analytics/inventory-trends/{productId}?startDate={date}&endDate={date}` - Get inventory trends
- `GET /api/analytics/product-performance?startDate={date}&endDate={date}` - Get product performance

### SignalR Hub

- **Endpoint**: `/inventoryhub`
- **Events**:
  - `InventoryUpdated` - Fired when inventory changes
  - `NewAlert` - Fired when a new alert is created
  - `AlertResolved` - Fired when an alert is resolved

## 🔐 Authentication

All protected endpoints require a JWT token in the Authorization header:

```http
Authorization: Bearer {your_jwt_token}
```

### Roles

- **Admin**: Full access to all features
- **Manager**: Can manage products, inventory, suppliers, and orders
- **User**: Read-only access to most features

## 🗄️ Database Schema

### Core Tables

- **Users**: User accounts and authentication
- **Products**: Product catalog
- **Inventory**: Current inventory levels
- **Suppliers**: Supplier information
- **Orders**: Purchase orders
- **OrderItems**: Order line items
- **Alerts**: System alerts and notifications
- **InventoryHistory**: Inventory change history
- **Transactions**: Sales and purchase transactions
- **ProductSuppliers**: Product-supplier relationships

See `database/schema.sql` for complete schema definition.

## 🧪 Testing

### Backend Tests

```bash
dotnet test
```

### Frontend Tests

```bash
cd frontend
npm run test
```

## 📦 Building for Production

### Backend

```bash
dotnet publish -c Release -o ./publish
```

### Frontend

```bash
cd frontend
npm run build
```

The production build will be in `frontend/dist/`.

## 🚀 Deployment

### Backend Deployment

1. Update `appsettings.json` with production connection strings
2. Set environment variables for sensitive data
3. Configure HTTPS certificates
4. Deploy to your hosting platform (Azure, AWS, etc.)

### Frontend Deployment

1. Build the production bundle: `npm run build`
2. Deploy the `dist/` folder to a static hosting service (Vercel, Netlify, etc.)
3. Update API base URL in production environment

## 🔧 Configuration

### JWT Settings

Configure in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!",
    "Issuer": "EvenApp",
    "Audience": "EvenApp",
    "ExpirationMinutes": "60"
  }
}
```

### CORS Configuration

Update allowed origins in `Program.cs`:

```csharp
policy.WithOrigins("http://localhost:5173", "https://yourdomain.com")
```

## 📝 Development Guidelines

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Follow Vue.js style guide for frontend code

### Git Workflow

1. Create a feature branch: `git checkout -b feature/your-feature`
2. Make your changes
3. Commit: `git commit -m "Add: your feature description"`
4. Push: `git push origin feature/your-feature`
5. Create a Pull Request

### Commit Message Format

- `Add:` - New feature
- `Fix:` - Bug fix
- `Update:` - Update existing feature
- `Refactor:` - Code refactoring
- `Docs:` - Documentation changes

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add: Some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Authors

- **Daniel23-stack** - *Initial work* - [GitHub](https://github.com/Daniel23-stack)

## 🙏 Acknowledgments

- .NET team for the excellent framework
- Vue.js team for the amazing frontend framework
- All open-source contributors whose libraries made this project possible

## 📞 Support

For support, email support@evenapp.com or open an issue in the repository.

## 🔄 CI/CD Pipeline

This project includes comprehensive GitHub Actions workflows for continuous integration and deployment:

### Workflows

1. **Backend CI/CD** (`.github/workflows/backend-ci.yml`)
   - Builds .NET solution
   - Runs tests
   - Security scanning
   - Code analysis
   - Publishes build artifacts

2. **Frontend CI/CD** (`.github/workflows/frontend-ci.yml`)
   - Installs dependencies
   - Runs linter
   - Builds production bundle
   - Uploads build artifacts

3. **Deployment** (`.github/workflows/deploy.yml`)
   - Builds and packages backend
   - Builds frontend for production
   - Ready for Azure, AWS, Vercel, Netlify deployment
   - Triggered on main branch pushes or version tags

4. **Database Migration** (`.github/workflows/database-migration.yml`)
   - Manual workflow for database migrations
   - Supports development, staging, and production environments

5. **Code Quality** (`.github/workflows/code-quality.yml`)
   - SonarCloud integration
   - ESLint checks
   - Code formatting validation

6. **Release** (`.github/workflows/release.yml`)
   - Automated release creation on version tags
   - Generates changelog
   - Creates release assets

7. **Main CI Pipeline** (`.github/workflows/ci.yml`)
   - Combined workflow for all checks
   - Runs on every push and pull request

### Setting Up CI/CD

The workflows are ready to use. To enable deployment, add the following secrets to your GitHub repository:

**For Azure Deployment:**
- `AZURE_WEBAPP_PUBLISH_PROFILE`

**For Vercel Deployment:**
- `VERCEL_TOKEN`
- `VERCEL_ORG_ID`
- `VERCEL_PROJECT_ID`

**For Netlify Deployment:**
- `NETLIFY_AUTH_TOKEN`
- `NETLIFY_SITE_ID`

**For Database Migrations:**
- `MYSQL_HOST`
- `MYSQL_USER`
- `MYSQL_PASSWORD`
- `MYSQL_DATABASE`

**For Code Quality:**
- `SONAR_TOKEN`

**For Frontend Environment:**
- `VITE_API_BASE_URL`

### Viewing Workflow Status

Visit the **Actions** tab in your GitHub repository to see workflow runs and their status.

## 🔮 Roadmap

- [x] CI/CD pipeline
- [ ] Unit test coverage
- [ ] Integration tests
- [ ] Docker containerization
- [ ] Mobile app (React Native)
- [ ] Barcode scanning integration
- [ ] Multi-warehouse support
- [ ] Advanced reporting
- [ ] Email notifications
- [ ] Export to Excel/PDF

---

**Built with ❤️ using Clean Architecture and SOLID Principles**
