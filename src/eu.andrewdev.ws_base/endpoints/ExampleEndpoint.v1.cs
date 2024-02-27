
using eu.andrewdev.ws_base.models;
using eu.andrewdev.ws_base.services;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Endpoint = eu.andrewdev.ws_base.common.Endpoint;

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
