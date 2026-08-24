# 🚀 NexusErp — Multi-Branch Retail & Inventory Management API

A backend system for multi-branch retail operations — inventory, sales, procurement, and payments — built on .NET 10 with Clean Architecture and CQRS.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue?style=for-the-badge)
![Pattern](https://img.shields.io/badge/Pattern-CQRS%20%2B%20MediatR-orange?style=for-the-badge)

## Why this exists

Most starter ERP projects stop at basic CRUD. NexusErp goes further into the problems that actually break retail systems in production: two cashiers selling the last unit at once, stock costs drifting after partial returns, and payment confirmations that need to be tamper-proof. This project is an attempt to solve those properly, not just demo them.

## Architecture

Four layers, dependencies pointing inward:

```
NexusErp (API)           → Controllers, middleware, Swagger
NexusErp.Application      → CQRS commands/queries, validation, business rules
NexusErp.Infrastructure   → EF Core, Identity, repositories, external services
Nexus.Erp.Domain          → Entities — no dependency on anything else
```

## What it does

- **Inventory** — multi-branch stock tracking with batch-level detail (expiry dates, unit cost) and barcode lookup
- **Sales** — invoice creation with FIFO stock allocation across batches, multiple payment methods per invoice
- **Returns** — automatic restocking into the correct batch when a sale is reversed
- **Procurement** — purchase orders → goods receipt → automatic batch creation and weighted-average cost recalculation
- **Payments** — Paymob integration with signed, verified webhook callbacks

## Technical highlights

A few decisions worth calling out, since they don't show up in a plain feature list:

- **Optimistic concurrency control** — `BranchStock` and `ProductBatch` carry a `RowVersion`. Two cashiers can hit "sell" on the last unit at the exact same moment and the system won't oversell: one request wins, the other gets a clean conflict response instead of corrupting the stock count.
- **FIFO batch allocation** — stock is deducted oldest-batch-first, so unit cost and expiry tracking stay accurate as inventory turns over.
- **Claims-based authorization** — roles (Admin, Cashier, InventoryManager, Accountant) map to granular permissions, enforced through custom ASP.NET Core authorization policies — not just "logged in or not."
- **Verified payment webhooks** — Paymob callbacks are validated with an HMAC-SHA512 signature check using a constant-time comparison, so payment confirmations can't be spoofed or replayed.
- **Consistent error handling** — a global exception middleware plus a FluentValidation pipeline behavior mean every error response has the same shape, whether it's a validation failure, a not-found, or a concurrency conflict.

## Tech stack

| Layer | Tools |
|---|---|
| Framework | .NET 10, ASP.NET Core Web API |
| Data | Entity Framework Core, SQL Server |
| Patterns | CQRS + MediatR, Specification Pattern, Unit of Work, Result Pattern |
| Auth | ASP.NET Core Identity, JWT, Role & Policy-based Authorization |
| Validation | FluentValidation |
| Payments | Paymob |
| Docs | Swagger / OpenAPI |
| Logging | Serilog |

## Getting started

**Prerequisites:** .NET 10 SDK, SQL Server (or LocalDB), Git

```bash
git clone https://github.com/1TahaAhmed/NexusErp.git
cd NexusErp/NexusErp
```

Configure secrets locally (never commit these — use User Secrets, not appsettings.json):

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:MyInventoryDatabase" "<your-connection-string>"
dotnet user-secrets set "JwtSettings:Secret" "<your-secret>"
dotnet user-secrets set "PaymobSettings:ApiKey" "<your-paymob-key>"
dotnet user-secrets set "PaymobSettings:HmacSecret" "<your-paymob-hmac-secret>"
dotnet user-secrets set "InitialAdmin:Email" "<admin-email>"
dotnet user-secrets set "InitialAdmin:Password" "<admin-password>"
```

Apply migrations and run:

```bash
dotnet ef database update --project ../NexusErp.Infrastructure --startup-project .
dotnet run
```

Default roles (Admin, Cashier, InventoryManager, Accountant) and one admin account are seeded automatically on first run. Swagger UI is available at `/swagger` in development.

## Roles & permissions

| Role | Can do |
|---|---|
| Admin | Full access to all resources |
| Cashier | Create & view sales invoices, view products |
| InventoryManager | Manage products, view & create purchase orders |
| Accountant | Read-only access to sales, purchase orders, and returns |

---

Built by [Taha Ahmed](https://linkedin.com/in/taha-ahmed-backend) · [GitHub](https://github.com/1TahaAhmed)
