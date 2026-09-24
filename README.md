# 🥗 GulDiyet — Dietitian Clinic Management System

A web application for running a dietitian clinic — patients, dietitians, appointments, laboratory tests, diet plans and patient feedback — built with **ASP.NET Core MVC (.NET 8)** on a layered, onion-style architecture.

The repository also contains the project's requirements, design and test & maintenance reports (in Turkish).

## ✨ Features

- **Role-based access** — Admin, Assistant and Dietitian roles with session-based login and registration
- **Patients & dietitians** — create, edit, list and delete records, including photo upload
- **Appointments** — booking by day and time, consultation flow and status tracking
  (`PendingConsultation → PendingResults → ResultsPending → Completed`)
- **Laboratory tests & results** — define tests, attach results to an appointment and review them
- **Diet plans** — dietitians prepare plans for their patients
- **Feedback & evaluations** — patients rate appointments and leave comments
- **PDF reports** — appointment / lab result reports generated with PDFsharp
- **E-mail notifications** — SMTP e-mails published to a **RabbitMQ** queue (`emailQueue`) and sent asynchronously
- **Real-time updates** — SignalR hubs for appointment and evaluation notifications
- **Flexible storage** — Entity Framework Core with SQL Server, or an in-memory database for quick demos

## 🏗️ Architecture

```text
GulDiyet.sln
├── GulDiyet.Core.Domain                # Entities: Patient, Diyetisyen, Appointment, DietPlan, LaboratoryTest, ...
├── GulDiyet.Core.Application           # Services, interfaces, view models, helpers, SignalR hubs
├── GulDiyet.Infrastructure.Persistence # EF Core DbContext, generic & specific repositories, migrations
└── GulDiyet                            # ASP.NET Core MVC app: controllers, Razor views, middlewares
```

## 🧰 Tech stack

| Layer | Technologies |
| --- | --- |
| Web | ASP.NET Core MVC (.NET 8), Razor views, session auth |
| Data | Entity Framework Core 8, SQL Server, EF Core In-Memory |
| Messaging | RabbitMQ (RabbitMQ.Client, MassTransit) |
| Real-time | SignalR |
| Reporting | PDFsharp |

## 🚀 Getting started

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download), SQL Server (or the in-memory option) and RabbitMQ:

```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

1. **Clone the repository**

   ```bash
   git clone https://github.com/emirhankaplan/EmirhanKaplan-GulDiyet-ASP.net-Core--master.git
   cd EmirhanKaplan-GulDiyet-ASP.net-Core--master
   ```

2. **Configure** `GulDiyet/appsettings.json` / `appsettings.Development.json`
   - `ConnectionStrings:DefaultConnection` — your SQL Server connection string
     (or add `"UseInMemoryDatabase": true` to skip SQL Server)
   - `RabbitMQConfiguration` — host, username and password
   - `SmtpConfig` — SMTP host, port, user and password

   > 🔐 Keep real credentials out of source control — use `dotnet user-secrets` or environment variables.

3. **Create the database**

   ```bash
   dotnet ef database update --project GulDiyet.Infrastructure.Persistence --startup-project GulDiyet
   ```

4. **Run**

   ```bash
   dotnet run --project GulDiyet
   ```

   The app starts on the login page (`/User/Login`).

## 📄 Project documents

- [System definition & requirements report](GulDiyet-SistemTan%C4%B1m%C4%B1%20VeGereksinim-Raporu.pdf)
- [Design report](GulDiyet-Tasar%C4%B1m-Raporu.pdf)
- [Test & maintenance report](GulDiyet-TestVeBak%C4%B1m-Raporu.pdf)
