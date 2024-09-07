# NLayer Currency Exchange Rate API

## Introduction
The **NLayer Currency Exchange Rate API** is a .NET-based web API that provides exchange rate data for different currencies. It integrates with RabbitMQ as a message broker and Redis for caching. The system fetches live exchange rates from the Turkish Central Bank (TCMB) and allows you to manage exchange rates (create, update, delete, and retrieve) through an API.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Architecture](#architecture)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Dependencies](#dependencies)
- [Contributors](#contributors)

## Features
- Fetch live exchange rates from the Turkish Central Bank.
- Store exchange rates in Redis for fast access.
- Use RabbitMQ to publish exchange rate changes for consumption by other services.
- CRUD operations for managing exchange rates via RESTful API.

## Architecture
The project follows a **multi-layered architecture**:
1. **Core Layer**: Contains entity models and interfaces.
2. **Infrastructure Layer**: Implements services for exchange rate management, Redis integration, and RabbitMQ messaging.
3. **Service Layer**: Manages business logic and orchestration between external systems like RabbitMQ and Redis.
4. **API Layer**: Exposes API endpoints to interact with exchange rates.

## Service Layer
The **Service Layer** in this project orchestrates interactions between RabbitMQ, Redis, and external sources like the Turkish Central Bank. It is responsible for fetching and caching exchange rates, publishing rate changes to RabbitMQ, and consuming messages from the queue.

### Key Components of the Service Layer:
- **ExchangeRateService**: This service handles the business logic of fetching exchange rates from the Turkish Central Bank, storing rates in Redis, and publishing changes to RabbitMQ.
  
    - `GetExchangeRateAsync(string currencyCode)`: Fetches the exchange rate for a specific currency from Redis or the Central Bank if not cached.
    - `GetExchangeRateFromQueue()`: Retrieves exchange rate data from the RabbitMQ queue.

- **RedisService**: Provides methods to interact with Redis for storing, retrieving, and deleting exchange rates.

- **RabbitMQPublisher**: Publishes messages (exchange rate updates) to a RabbitMQ queue.
  
- **RabbitMQConsumer**: Consumes messages from RabbitMQ and retrieves exchange rate data.

## Installation
1. **Clone the repository**:
    ```bash
    git clone https://github.com/mertomer/NLayerProject.git
    cd NLayerCurrencyAPI
    ```
2. **Install Redis**:
    - Follow the installation instructions on the [Redis website](https://redis.io/download) to set up Redis on your system.

3. **Install RabbitMQ**:
    - Download and install RabbitMQ from the [RabbitMQ website](https://www.rabbitmq.com/download.html).

4. **Install .NET Core SDK**:
    - Ensure that .NET Core SDK is installed on your system. You can download it from [here](https://dotnet.microsoft.com/download).

5. **Restore NuGet Packages**:
    ```bash
    dotnet restore
    ```

6. **Run the Application**:
    ```bash
    dotnet run
    ```

## Configuration
### RabbitMQ
- By default, RabbitMQ is configured to run on `localhost`. Update the RabbitMQ connection details in `RabbitMQConsumer` and `RabbitMQPublisher` classes if necessary.
  
### Redis
- Update the Redis connection string in the `RedisService` constructor:
    ```csharp
    public RedisService(string connectionString) {
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _db = _redis.GetDatabase();
    }
    ```

## Dependencies
- [.NET Core](https://dotnet.microsoft.com/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Redis](https://redis.io/)
- [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)
- [RabbitMQ.Client](https://www.nuget.org/packages/RabbitMQ.Client)

## Contributors
- [Mert Ömer Bakar](https://github.com/mertomer)

