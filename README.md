# 📦 Microservices Order Processing System (RabbitMQ + MassTransit)

## 🧠 Overview

This project demonstrates a **Microservices Architecture** using:

* **ASP.NET Core Web API**
* **MassTransit**
* **RabbitMQ**
* **Entity Framework Core**
* **SMTP Email Service**

The system is designed based on **Event-Driven Architecture**, where services communicate asynchronously through message queues instead of direct API calls.

---

## 🏗️ Architecture

The solution consists of 3 independent services:

### 1. ProductAPI

* Manages product data
* Publishes product events to RabbitMQ

### 2. OrderAPI

* Handles order creation
* Consumes product events
* Publishes email notification events

### 3. EmailNotificationWebhook

* Consumes email events
* Sends email via SMTP

---

## 🔄 System Flow

### Step 1: Create Product

* Client sends request to `ProductAPI`
* Product is saved to database
* Event is published to RabbitMQ

```
ProductAPI → RabbitMQ → OrderAPI (ProductConsumer)
```

---

### Step 2: Sync Product to Order Service

* `OrderAPI` listens to product events
* Saves product into its own database

---

### Step 3: Create Order

* Client sends request to `OrderAPI`
* Order is saved
* System builds order summary
* Publishes email event

```
OrderAPI → RabbitMQ → Email Service
```

---

### Step 4: Send Email

* Email service consumes event
* Sends HTTP request to internal webhook
* Webhook triggers SMTP email sending

```
Consumer → Webhook → EmailService → SMTP
```

---

## 🧩 Technologies Used

* ASP.NET Core Web API
* Entity Framework Core (SQL Server)
* MassTransit (Message Bus)
* RabbitMQ (Message Broker)
* MailKit (SMTP Email)
* Swagger (API Testing)

---

## 📁 Project Structure

```
Solution
│
├── ProductAPI
│   ├── Controllers
│   ├── Service
│   ├── Repository
│   └── Data
│
├── OrderAPI
│   ├── Controllers
│   ├── Consumer
│   ├── Service
│   ├── Repository
│   └── Data
│
├── EmailNotificationWebhook
│   ├── Consumer
│   ├── Service
│   └── Repository
│
├── Shared (DTOs & Models)
│   ├── EmailDTOs
│   ├── OrderSummary
│   ├── ServiceResponse
│   ├── Product
│   └── Order
```

---

## 📬 Messaging Pattern

This project uses **Publish/Subscribe pattern**:

* Producers publish messages
* Consumers subscribe to message types
* RabbitMQ routes messages automatically

---

## 🔑 Key Concepts Demonstrated

* Microservices architecture
* Event-driven communication
* Loose coupling between services
* Asynchronous processing
* Data replication across services
* Separation of concerns

---

## ⚠️ Known Limitations

* SMTP credentials are hardcoded (should use environment variables)
* No retry or error handling for message failures
* Basic RabbitMQ configuration (no dead-letter queue)
* Potential performance issues in database queries
* Order cleanup logic may cause data loss

---

## 🚀 Future Improvements

* Add retry policies and circuit breaker
* Implement logging (Serilog)
* Use environment-based configuration
* Introduce API Gateway
* Add authentication & authorization
* Improve database query performance
* Replace webhook with direct consumer processing

---

## ▶️ How to Run

### Prerequisites

* .NET 6+
* SQL Server
* RabbitMQ

### Steps

1. Start RabbitMQ
2. Update connection strings in `appsettings.json`
3. Run all 3 services:

   * ProductAPI
   * OrderAPI
   * EmailNotificationWebhook
4. Use Swagger to test APIs

---

## 🎯 Conclusion

This project is a practical example of building a **distributed system using microservices and message queues**.

It demonstrates how services can remain independent while still collaborating efficiently through asynchronous communication.

---

## 👨‍💻 Author

Developed for learning and demonstrating microservices architecture with .NET.
