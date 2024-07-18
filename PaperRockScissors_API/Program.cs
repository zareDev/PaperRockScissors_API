using FluentValidation.AspNetCore;
using PaperRockScissors_API.Handlers;
using PaperRockScissors_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<GetAllChoices>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type =>
    {
        var t = type.ToString().Split(".")[2].Replace("+", "");
        return t;
    });
});

builder.Services.AddTransient<ChoiceService>();
builder.Services.AddHttpClient<ChoiceService>((serviceProvider, client) => { });
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());


builder.Services.AddCors(options =>
{
    // this defines a CORS policy called "default"
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.WebHost.UseUrls("http://*:8080");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
//app.UseAuthorization();

app.MapControllers();

app.Run();
