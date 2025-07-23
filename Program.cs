using Events.DbInit;
using Events.Extensions;
using Events.Utils;

var builder = WebApplication.CreateBuilder(args);

// inject custom service helper
new ApplicationHelperServices(builder.Services, builder.Configuration).AddApplicationServices();

// inject jwt method
builder.Services.AddScoped<JwtToken>();

// swagger ui support services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbInit = services.GetRequiredService<IDbInit>();
        dbInit.Initialize();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured creating the Db.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("dev");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("prod");
}

app.MapControllers();
app.UseSession();

app.UseHttpsRedirection();

app.Run();
