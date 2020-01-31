# VPets C# HTTP API

## A simple virtual pets game as a ASP.NET Core Web API

### [API Documentation](https://vpets.herokuapp.com/) | [Live Demo](https://vpets.herokuapp.com/api/v1/users)

## Instructions for running

Using Docker locally:

```
docker build . -t vpets -f Dockerfile

docker run -p 8080:80 -p 8001:443 --name vpets vpets
```

This will run tests and host it on your local machine.

Alternatively you can use the dotnet CLI, or open the project solution in Visual Studio.

### Approach

#### Technologies

This was my first time writing a web-service in C#, so I relied heavily on [Microsoft's documentation](https://docs.microsoft.com/en-us/dotnet/).

I chose to create the application using ASP.Net Core and an in-memory database, cross-applying design concepts from Java e.g. separation of concerns between aspects of the domain via interfaces, from an MVC pattern to services and repositories.

#### Models

I expressed the pets as POCOs with abstract parent classes - as a result, types of metrics and animals, in addition to the generation of their field values could all be extended to incorporate further game features.

#### Game Loop

The core game loop is achieved by using *time deltas* to compute the "actual" values of metrics. A combination of a background service to mutate pet metrics on the server (while the user is "offline"), and volatile computation on GET requests ensures the user always receives accurate information about their pets, without putting too much load on the service or breaking HTTP method specifications such as idempotency. Pet metrics degrade linearly, and when interacted with are set to their "best" value. Most virtual pet games react will close their game loop with further distinctions between "ALIVE" and "DEAD" based on player actions - I have designed the system with this in mind.

#### Defensiveness

In order to not expose too much information to the client, and to enforce strict contracts, I've used custom value mappers and field-level annotations. I've also created unit and integration tests where appropriate.

### Improvements

Some improvements that could be made given more time:

- More granular error handling and logging
- More comprehensive tests
- More production-ready database integration
- Separation of configuration and default values into centralised key-value store
- Authentication (OAuth 2.0 w/ JWT?)