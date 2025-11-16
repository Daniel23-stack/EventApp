-- Seed data for Smart Inventory Management System

USE EvenAppInventory;

-- Insert default admin user (password: Admin@123)
-- NOTE: These are placeholder password hashes. In production, use the registration endpoint
-- or generate proper BCrypt hashes. For testing, you can register users through the API.
-- Example BCrypt hash for "Admin@123": $2a$11$YourGeneratedHashHere
INSERT INTO Users (Username, Email, PasswordHash, Role) VALUES
('admin', 'admin@evenapp.com', '$2a$11$K7L1OJ45/4Y2nI3RV2sXJOe7ZqN8q5V5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z', 'Admin'),
('manager', 'manager@evenapp.com', '$2a$11$K7L1OJ45/4Y2nI3RV2sXJOe7ZqN8q5V5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z', 'Manager'),
('user', 'user@evenapp.com', '$2a$11$K7L1OJ45/4Y2nI3RV2sXJOe7ZqN8q5V5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z5Z', 'User');

-- Insert sample suppliers
INSERT INTO Suppliers (Name, ContactName, Email, Phone, Address) VALUES
('Tech Supplies Co.', 'John Smith', 'john@techsupplies.com', '555-0101', '123 Tech Street, Tech City, TC 12345'),
('Office Depot Pro', 'Jane Doe', 'jane@officedepot.com', '555-0102', '456 Office Ave, Business District, BD 67890'),
('Global Electronics', 'Bob Johnson', 'bob@globalelectronics.com', '555-0103', '789 Electronics Blvd, Digital City, DC 11111');

-- Insert sample products
INSERT INTO Products (SKU, Name, Description, Category, UnitPrice, ReorderLevel) VALUES
('LAP-001', 'Laptop Computer', 'High-performance laptop for business use', 'Electronics', 999.99, 10),
('MON-001', '27" Monitor', '4K Ultra HD monitor', 'Electronics', 299.99, 15),
('KEY-001', 'Mechanical Keyboard', 'RGB mechanical gaming keyboard', 'Electronics', 129.99, 20),
('MOU-001', 'Wireless Mouse', 'Ergonomic wireless mouse', 'Electronics', 49.99, 30),
('DESK-001', 'Standing Desk', 'Adjustable height standing desk', 'Furniture', 499.99, 5),
('CHAIR-001', 'Ergonomic Chair', 'Office ergonomic chair with lumbar support', 'Furniture', 349.99, 8),
('PAPER-001', 'A4 Paper Ream', '500 sheets of A4 paper', 'Office Supplies', 8.99, 50),
('PEN-001', 'Ballpoint Pen Set', 'Set of 12 ballpoint pens', 'Office Supplies', 12.99, 100);

-- Link products to suppliers
INSERT INTO ProductSuppliers (ProductId, SupplierId, SupplierSKU, UnitCost) VALUES
(1, 3, 'GE-LAP-001', 750.00),
(2, 3, 'GE-MON-001', 200.00),
(3, 1, 'TS-KEY-001', 80.00),
(4, 1, 'TS-MOU-001', 25.00),
(5, 2, 'OD-DESK-001', 350.00),
(6, 2, 'OD-CHAIR-001', 250.00),
(7, 2, 'OD-PAPER-001', 5.00),
(8, 1, 'TS-PEN-001', 7.00);

-- Insert initial inventory
INSERT INTO Inventory (ProductId, Quantity, Location, UpdatedBy) VALUES
(1, 25, 'Warehouse A', 1),
(2, 40, 'Warehouse A', 1),
(3, 60, 'Warehouse B', 1),
(4, 100, 'Warehouse B', 1),
(5, 12, 'Warehouse A', 1),
(6, 20, 'Warehouse A', 1),
(7, 200, 'Warehouse B', 1),
(8, 500, 'Warehouse B', 1);

