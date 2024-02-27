using eu.andrewdev.ws_base.interfaces;

public static class AppEndpointExtension
{
    public static WebApplication RegisterEndpoint(this WebApplication app, IEndpoint endpoint)
    {   
        endpoint.SetApp(app);
        endpoint.Map();
        return app;
    }
}