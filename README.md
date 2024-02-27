# ws_base

This is a template for a web service using .NET Core 8 and C#.

## Quick Navigation

- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Application setup](#application-setup)
- [Base Classes](#base-classes)
  - [Standard Response](#standard-response)
  - [AppEndpointExtension](#appendpointextension)
  - [Endpoint](#endpoint)
- [Usage](#usage)
    - [Registering a new endpoint](#registering-a-new-endpoint)
    - [Add a new Service](#add-a-new-service)

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio Code
3. Run the application

## Prerequisites

- .NET Core 8
- Visual Studio Code

## Application setup

In the `src/eu.andrewdev.ws_base` folder you will find these folders:

- `common`: Contains common classes, utilities, global variables, etc.
- `endpoint`: Contains the API controllers with all the endpoints;
- `enums`: Cointains all the enums used in the application;
- `models`: Contains all the models used in the application;
- `services`: Contains all the services used in the application;

Always in the `src` folder you will find 3 files:

- `appsettings.Development.json`: Contains the configuration for the development environment;
- `appsettings.json`: Contains the configuration for the production environment;
- `Program.cs`: Contains the main method that runs the application;

## Base Classes

### Standard Response

Is the class that represents the standard response of the application. It contains the data, the error message, the status code and the error object. The server should **ALWAYS** return a response of this type.

```csharp
public class StandardResponse<T> where T : class
{
    public T? data { get; set; }
    public string? errorMessage { get; set; } = "";
    public int statusCode { get; set; } = 0;
    public StandardError? error { get; set; }
    public StandardResponse(T? data, string? message = "")
    {
        this.data = data;
        errorMessage = message;

    }
}
/**
* Errore standard
*/
public class StandardError
{
    public string description { get; set; } = "";
    public int code { get; set; } = 0;
}
```

### AppEndpointExtension

This class extends the `WebApplication` class from `Microsoft.AspNetCore.Builder` and adds the `RegisterEndpoint` method. This method is used to register the endpoints of the application.

```csharp
public static class AppEndpointExtension
{
    public static WebApplication RegisterEndpoint(this WebApplication app, IEndpoint endpoint)
    {
        endpoint.SetApp(app);
        endpoint.Map();
        return app;
    }
}
```

### Endpoint

The base class for every endpoint. It contains the base path, the api version, the endpoint path and the `BuildPath` method that is used to build the path of the endpoint.

This class implements the interface `IEndpoint`

```csharp
using eu.andrewdev.ws_base.interfaces;

namespace eu.andrewdev.ws_base.common;

public abstract class Endpoint : IEndpoint
{
    private string _basePath = "/api";

    public string GetBasePath()
    {
        return _basePath;
    }

    public void SetBasePath(string value)
    {
        _basePath = value;
    }

    private string _apiVersion = "";

    public string GetApiVersion()
    {
        return _apiVersion;
    }

    public void SetApiVersion(string value)
    {
        _apiVersion = value;
    }

    private string _endpointPath ="";

    public string GetEndpointPath()
    {
        return _endpointPath;
    }


    public Endpoint(string apiVersion, string endpointPath)
    {
        _apiVersion = apiVersion.StartsWith("/") ? apiVersion : "/" + apiVersion;
        _endpointPath = endpointPath.StartsWith("/") ? endpointPath : "/" + endpointPath;
    }

    protected WebApplication? _app;

    public string BuildPath(string path)
    {
        return GetBasePath()
        + (GetApiVersion().StartsWith("/") ? GetApiVersion() : "/" + GetApiVersion())
        + (GetEndpointPath().StartsWith("/") ? GetEndpointPath() : "/" + GetEndpointPath())
        + (path.StartsWith("/") ? path : "/" + path);
    }

    protected WebApplication getApp()
    {
        if(_app == null)
        {
            throw new Exception("App not set");
        }
        return _app;
    }

    public abstract void Map();

    public void SetApp(WebApplication app)
    {
        _app = app;
    }
}
```

# Usage

This is an example of how to use the application with the implementation of new endpoints, new services, etc.

## Registering a new endpoint

_Scenario_: You need to create a new endpoint that return the current datetime.

1. Create a new class that extends the `Endpoint` class and implements the `IEndpoint` interface.

```csharp
public class ExampleEndpointV1(string apiVersion, string endpointPath)
    : Endpoint(apiVersion, endpointPath)
{
    public override void Map()
    {
        getApp()
            .MapGet(BuildPath("time"), GetTime)
            .WithDescription("Get time")
            .WithSummary("Get time")
            .WithName("GetTime")
            .WithDisplayName("GetTime")
            .WithOpenApi();

    }

    public StandardResponse<string> GetTime()
    {
        return new StandardResponse<string>(DateTime.Now.ToString());
    }
}
```

As you can see, this class extends the `Endpoint` class and implements the `IEndpoint` interface. The `Map` method is used to map the endpoint to the application. The `GetTime` method is the method that will be called when the endpoint is called.

The `StandardResponse` class is used to return the response of the endpoint.

2. Register the endpoint in the `Program.cs` file.

```csharp

app.RegisterEndpoint(new ExampleEndpointV1("v1","badge")).Run();

```

As last instruction in your `Program.cs` file you must execute the `Run()` method of the `WebApplication` class, but this must be done after all the endpoints are registered.

As you can see, the endpoint is registered using the `RegisterEndpoint` method of the `WebApplication` class. This method is an extension of the `WebApplication` class and is defined in the `AppEndpointExtension` class. You must pass to the constructor the _api version_ and the _base path_ of the endpoint.

## Add a new Service

_Scenario_: You need to create a new service that return the datetime

1. Create a new class that contains the service.

```csharp
public class DateTimeService
{
    public string GetDateTime()
    {
        return DateTime.Now.ToString();
    }
}
```

and register the service in the `Program.cs` file.

```csharp

builder.Services.AddSingleton<DateTimeService>();

```

And now we make a little change in the `ExampleEndpointV1` class.

```csharp
public class ExampleEndpointV1(string apiVersion, string endpointPath)
    : Endpoint(apiVersion, endpointPath)
{
    public override void Map()
    {
        getApp()
            .MapGet(BuildPath("time"), GetTime)
            .WithDescription("Get time")
            .WithSummary("Get time")
            .WithName("GetTime")
            .WithDisplayName("GetTime")
            .WithOpenApi();

    }

    public StandardResponse<string> GetTime(DateTimeService dateTimeService)
    {
        return new StandardResponse<string>(dateTimeService.GetDateTime().ToString());
    }
}
```

As you can see, the `GetTime` method now accepts a `DateTimeService` object as parameter. This is possible because the `DateTimeService` is registered as a service in the `Program.cs` file and use the `Dependency Injection` pattern.

## Add a new service with service dependency

_Scenario_: You need to create a new service that return the datetime from the `DateTimeService` service.

Create a class that contains the service.

```csharp
public class ExampleServiceWithDep {

    private readonly DateTimeService _dateTimeService;
    public ExampleServiceWithDep(DateTimeService assetService)
    {
        _dateTimeService = assetService;
    }

    public Example Example(){
        return new Example(){Message = $"Example with dependency injection for the date time that is {_dateTimeService.GetDateTime()}", Number = 1};
    }
}
```
and register it in the `Program.cs` file.

```csharp
builder.Services.AddSingleton<DateTimeService>();

// Services registration
builder.Services.AddScoped<ExampleServiceWithDep>(provider => {
    var noDepService = provider.GetRequiredService<DateTimeService>();
    return new ExampleServiceWithDep(noDepService);
});
```

**NOTE**: The `DateTimeService` is registered as a `Singleton` and the `ExampleServiceWithDep` is registered as a `Scoped` service. This is because the `DateTimeService` is a service that is used in the entire application and the `ExampleServiceWithDep` is a service that is used only in the endpoint that is called. The `Scoped` service is created every time the endpoint is called.
Is important to note that the `ExampleServiceWithDep` class has a constructor that accepts a `DateTimeService` object as parameter. This is possible because the `DateTimeService` is registered as a service in the `Program.cs` file and use the `Dependency Injection` pattern.

Now we need to modify the `ExampleEndpointV1` class to use the `ExampleServiceWithDep` service.

```csharp
public class ExampleEndpointV1(string apiVersion, string endpointPath)
    : Endpoint(apiVersion, endpointPath)
{
    public override void Map()
    {
        getApp()
            .MapGet(BuildPath("time"), GetTime)
            .WithDescription("Get time")
            .WithSummary("Get time")
            .WithName("GetTime")
            .WithDisplayName("GetTime")
            .WithOpenApi();
        getApp()
            .MapGet(BuildPath("example"), GetExample)
            .WithDescription("Get example")
            .WithSummary("Get example")
            .WithName("GetExample")
            .WithDisplayName("GetExample")
            .WithOpenApi();
    }

    public StandardResponse<string> GetTime(DateTimeService dateTimeService)
    {
        return new StandardResponse<string>(dateTimeService.GetDateTime().ToString());
    }

    public StandardResponse<Example> GetExample(ExampleServiceWithDep exampleService)
    {
        StandardResponse<Example> response = new StandardResponse<Example>(null);
        try
        {
            
            if(exampleService == null)
            {
                throw new Exception("ExampleServiceWithDep not found");
            }
            response.data = exampleService.Example();
            return response;
        }
        catch (System.Exception e){
            response.error = new StandardError(){description = e.Message, code = 500};
            return response;
        }
    }
}
```
As you can see we have added a new endpoint called `GetExample` that uses the `ExampleServiceWithDep` service. The `GetExample` method accepts a `ExampleServiceWithDep` object as parameter. This is possible because the `ExampleServiceWithDep` is registered as a service in the `Program.cs` file and use the `Dependency Injection` pattern.
Another change is about the mapping of the `GetExample` method. We have added a new `MapGet` method that maps the `GetExample` method to the application.

## Middlewares

If you need to add middlewares you can simply a *static class* (eg. `Middlewares`) and add a *static method* (eg. `ApiKeyCheck`) that accepts a `HttpContext` object and `Func<Task>` as parameter. An example can be found below. This is a middleware that controll if the apikey is present in the request header.

```csharp
public static class Middlewares
{
    public static async Task ApiKeyCheck(HttpContext context, Func<Task> next)
    {
        if (!context.Request.Headers.ContainsKey("API-KEY"))
        {
            context.Response.StatusCode = 401; // Unauthorized
            context.Response.ContentType = "application/json"; // Imposta il tipo di contenuto su JSON

            var response = new StandardResponse<string>("API-KEY is missing");
            var jsonResponse = JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );

            await context.Response.WriteAsync(jsonResponse);
            return;
        }
        await next.Invoke();
    }
}
```

To use the middleware you need to add the `Use` method in the `Program.cs` file. The `Use` method is used to add a middleware to the application.

```csharp
if(!app.Environment.IsDevelopment()) 
    app.Use(Middlewares.ApiKeyCheck);
```

In this example the `ApiKeyCheck` middleware is added to the application (only in production environment).


## Ending

Now you are ready to create new amazing application 🎉 
Have fun!
