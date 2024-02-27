namespace eu.andrewdev.ws_base.services;

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