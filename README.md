# 🚀 NexusErp - Enterprise Resource Planning System

![.NET 10](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20%26%20Decoupled-blue?style=for-the-badge)
![Pattern](https://img.shields.io/badge/Design%20Pattern-CQRS%20%2B%20MediatR-orange?style=for-the-badge)

**NexusErp** is a modern, modular Enterprise Resource Planning (ERP) Web API built with **.NET 8**. Designed following **Clean Architecture** principles and **CQRS**, it provides a solid foundation for managing Inventory, Multi-Branch Sales, Procurement, and Payment Gateway integration with robust error handling and transaction security.

---

## 🏛️ Architectural Overview

The project is structured around **Clean Architecture (Onion Architecture)** to ensure maintainability, testability, and decoupling of core business logic from frameworks and external services.


---

## 🛠️ Key Features & Modules

### 📦 Inventory & Product Management
* Multi-branch stock tracking with real-time updates.
* Category and Product cataloging with SKU & Barcode support.
* Batch management for stock operations.

### 🧾 Sales & Invoicing Cycle
* Comprehensive sales invoice generation and processing.
* Integrated returns module with automatic stock restocking upon authorization.
* Discount and tax calculations at item and invoice levels.

### 🛒 Procurement & Purchase Orders
* Purchase order workflow for managing supplier relationships.
* Automated goods receiving updates that reflect directly into branch stock.

### 💳 Payments & Gateway Integration
* Support for multiple payment methods (Cash, Card, Electronic).
* Paymob Webhook integration for handling asynchronous payment notifications.

---

## ⚙️ Tech Stack & Libraries

* **Framework:** [.NET 8 Web API](https://dotnet.microsoft.com/)
* **Database:** Entity Framework Core 8 with SQL Server
* **Architecture:** Clean Architecture + CQRS Pattern
* **Mediator:** [MediatR](https://github.com/jbogard/MediatR)
* **Design Patterns:** Unit of Work, Generic Repository, Specification Pattern, Result Pattern
* **Validation:** FluentValidation
* **Documentation:** Swagger / OpenAPI

---

## 🚀 Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server / LocalDB
* Git

### Installation & Setup

1. **Clone the Repository:**
   ```bash
   git clone [https://github.com/1TahaAhmed/NexusErp.git](https://github.com/1TahaAhmed/NexusErp.git)
   cd NexusErp
