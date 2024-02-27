namespace eu.andrewdev.ws_base.interfaces;

public interface IEndpoint
{
    string GetBasePath();

    string GetApiVersion();

    string GetEndpointPath();
    public string BuildPath(string path);
    public void SetApp(WebApplication app);
    public void Map();
}