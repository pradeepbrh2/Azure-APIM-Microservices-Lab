# Azure API Management – Microservices Lab

A hands-on lab for learning Azure API Management (APIM) by building and deploying independent ASP.NET Core microservices.

## Services

| Service | Local HTTP URL | Responsibility |
|---|---|---|
| UserService | http://localhost:5001 | User endpoints |
| OrderService | http://localhost:5002 | Order endpoints |
| PaymentService | http://localhost:5003 | Payment endpoints |

Each service exposes Swagger UI at `/swagger` when running with the Development environment.

## Prerequisites

- .NET 10 SDK
- Git
- VS Code or Visual Studio
- An Azure subscription for the deployment/APIM modules

## Run locally

From the repository root, start each service in a separate terminal:

```powershell
dotnet run --project src/UserService --launch-profile http
dotnet run --project src/OrderService --launch-profile http
dotnet run --project src/PaymentService --launch-profile http
```

Open the Swagger pages:

- UserService: http://localhost:5001/swagger
- OrderService: http://localhost:5002/swagger
- PaymentService: http://localhost:5003/swagger

## Build the solution

```powershell
dotnet restore AzureAPIMLab.slnx
dotnet build AzureAPIMLab.slnx
```

## Learning roadmap

1. Build and test the microservices locally.
2. Deploy each service independently to Azure App Service.
3. Create an Azure API Management instance.
4. Import each service's OpenAPI definition into APIM.
5. Configure gateway routing and backend services.
6. Practise APIM policies: subscription keys, JWT validation, rate limits, transformations, caching and versioning.
7. Configure diagnostics and monitoring.

## Notes

- The current services use in-memory sample data for learning. Data is reset when a service restarts.
- These services are intentionally independent; this lab does not yet implement persistence or service-to-service calls.
- Do not commit generated `bin/` or `obj/` output, secrets, publish profiles or credentials.
