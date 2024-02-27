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