using eu.andrewdev.ws_base.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DateTimeService>();

// Services registration
builder.Services.AddScoped<ExampleServiceWithDep>(provider => {
    var noDepService = provider.GetRequiredService<DateTimeService>();
    return new ExampleServiceWithDep(noDepService);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if(!app.Environment.IsDevelopment()) 
    app.Use(Middlewares.ApiKeyCheck);



app.RegisterEndpoint(new ExampleEndpointV1("v1","badge")).Run();
