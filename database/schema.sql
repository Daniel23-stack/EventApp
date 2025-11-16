-- Smart Inventory Management System Database Schema
-- MySQL 8.0+

CREATE DATABASE IF NOT EXISTS EvenAppInventory;
USE EvenAppInventory;

-- Users table for authentication
CREATE TABLE Users (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(50) UNIQUE NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL DEFAULT 'User' CHECK (Role IN ('Admin', 'Manager', 'User')),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_username (Username),
    INDEX idx_email (Email)
);

-- Suppliers table
CREATE TABLE Suppliers (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    ContactName VARCHAR(100),
    Email VARCHAR(100),
    Phone VARCHAR(20),
    Address TEXT,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_name (Name)
);

-- Products table
CREATE TABLE Products (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    SKU VARCHAR(50) UNIQUE NOT NULL,
    Name VARCHAR(200) NOT NULL,
    Description TEXT,
    Category VARCHAR(100),
    UnitPrice DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    ReorderLevel INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_sku (SKU),
    INDEX idx_name (Name),
    INDEX idx_category (Category)
);

-- Product-Supplier relationship (many-to-many)
CREATE TABLE ProductSuppliers (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    SupplierId INT NOT NULL,
    SupplierSKU VARCHAR(50),
    UnitCost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id) ON DELETE CASCADE,
    UNIQUE KEY unique_product_supplier (ProductId, SupplierId),
    INDEX idx_product (ProductId),
    INDEX idx_supplier (SupplierId)
);

-- Inventory table
CREATE TABLE Inventory (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 0,
    Location VARCHAR(100),
    LastUpdated DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UpdatedBy INT,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    FOREIGN KEY (UpdatedBy) REFERENCES Users(Id) ON DELETE SET NULL,
    INDEX idx_product (ProductId),
    INDEX idx_location (Location)
);

-- Orders table
CREATE TABLE Orders (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    SupplierId INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Confirmed', 'Shipped', 'Delivered', 'Cancelled')),
    TotalAmount DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    CreatedBy INT,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id) ON DELETE RESTRICT,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id) ON DELETE SET NULL,
    INDEX idx_supplier (SupplierId),
    INDEX idx_status (Status),
    INDEX idx_order_date (OrderDate)
);

-- OrderItems table
CREATE TABLE OrderItems (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    TotalPrice DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE RESTRICT,
    INDEX idx_order (OrderId),
    INDEX idx_product (ProductId)
);

-- Alerts table
CREATE TABLE Alerts (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Type VARCHAR(50) NOT NULL CHECK (Type IN ('LowStock', 'Reorder', 'Expiring', 'Custom')),
    ProductId INT,
    Message TEXT NOT NULL,
    IsResolved BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ResolvedAt DATETIME,
    ResolvedBy INT,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    FOREIGN KEY (ResolvedBy) REFERENCES Users(Id) ON DELETE SET NULL,
    INDEX idx_type (Type),
    INDEX idx_product (ProductId),
    INDEX idx_resolved (IsResolved),
    INDEX idx_created (CreatedAt)
);

-- Inventory History table for tracking changes and analytics
CREATE TABLE InventoryHistory (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    PreviousQuantity INT NOT NULL,
    NewQuantity INT NOT NULL,
    ChangeType VARCHAR(20) NOT NULL CHECK (ChangeType IN ('Purchase', 'Sale', 'Adjustment', 'Return')),
    ChangeAmount INT NOT NULL,
    Location VARCHAR(100),
    ChangedBy INT,
    ChangedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    FOREIGN KEY (ChangedBy) REFERENCES Users(Id) ON DELETE SET NULL,
    INDEX idx_product (ProductId),
    INDEX idx_change_type (ChangeType),
    INDEX idx_changed_at (ChangedAt)
);

-- Sales/Transactions table for analytics
CREATE TABLE Transactions (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    TransactionType VARCHAR(20) NOT NULL CHECK (TransactionType IN ('Sale', 'Purchase', 'Return', 'Adjustment')),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    TransactionDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy INT,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE RESTRICT,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id) ON DELETE SET NULL,
    INDEX idx_product (ProductId),
    INDEX idx_transaction_type (TransactionType),
    INDEX idx_transaction_date (TransactionDate)
);

